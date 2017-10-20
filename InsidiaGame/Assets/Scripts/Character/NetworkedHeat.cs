using UnityEngine;
using UnityEngine.Networking;
using System;
using System.Collections;

/// <summary>
/// Put this on anything that needs to keep track of it's heat. Once the <see cref="Heat"/> reaches <see cref="heatCapacity"/>, <see cref="isOverheated"/> will become true.
/// The script will not exit out of being overheated until the Heat to heatCapacity ratio falls under the <see cref="overheatRecoveryPercentage"/>.
/// <para>Use <see cref="ChangeHeat(float)"/> and <see cref="SetHeat(float)"/> in order to change the Heat value. The changes will automatically be sent over the network.</para>
/// <para>There are <see cref="HeatChanged"/> and <see cref="Overheated"/> events (with both instance and static versions) that can be subscribed to in order to be notified of changes.</para>
/// Created by Christian Clark.
/// </summary>
[NetworkSettings(channel = 0, sendInterval = 1f / HEAT_UPDATES_PER_SECOND)]
public class NetworkedHeat : NetworkBehaviour
{
    private const float HEAT_UPDATES_PER_SECOND = 20f;

    public delegate void ChangeDelegate(HeatChangeArgs args);
    public delegate void OverheatedDelegate(OverheatedArgs args);

    [SyncEvent]
    public event ChangeDelegate EventHeatChanged;
    public static event ChangeDelegate HeatChanged;

    [SyncEvent]
    public event OverheatedDelegate EventOverheated;
    public static event OverheatedDelegate Overheated;

    [SyncVar]
    [SerializeField]
    private float _heat = 0f;
    public float Heat { get { return _heat; } }
    public float heatCapacity = 100f;

    [SyncVar]
    [SerializeField]
    [ReadOnly]
    private bool _isOverheated = false;
    public bool isOverheated { get { return _isOverheated; } }
    public float coolingPerSecond = 10f;
    [Range(0f, 1f)]
    public float overheatRecoveryPercentage = 0.7f;

    private Coroutine _heatUpdate;

    public void ChangeHeat(float changeAmount)
    {
        CmdChangeHeat(changeAmount);
    }

    [Command]
    private void CmdChangeHeat(float changeAmount)
    {
        float initalAmount = _heat;
        _heat = Mathf.Clamp(_heat + changeAmount, 0f, heatCapacity);

        if (initalAmount != _heat)
        {
            OnHeatChanged();
        }
    }

    public void SetHeat(float value)
    {
        CmdSetHeat(value);
    }

    [Command]
    private void CmdSetHeat(float value)
    {
        float initalAmount = _heat;
        _heat = Mathf.Clamp(value, 0f, heatCapacity);

        if (initalAmount != _heat)
        {
            OnHeatChanged();
        }
    }

    private void OnHeatChanged()
    {
        EventHeatChanged(new HeatChangeArgs(this, _heat));

        if (!isOverheated && _heat == heatCapacity)
        {
            _isOverheated = true;
            EventOverheated(new OverheatedArgs(this, _isOverheated));
        }
        else if (isOverheated && (_heat / heatCapacity) <= overheatRecoveryPercentage)
        {
            _isOverheated = false;
            EventOverheated(new OverheatedArgs(this, _isOverheated));
        }
    }

    private void CallStaticHeatChanged(HeatChangeArgs args)
    {
        if (HeatChanged != null)
            HeatChanged(args);
    }

    private void CallStaticOverheated(OverheatedArgs args)
    {
        if (Overheated != null)
            Overheated(args);
    }

    private void OnEnable()
    {
        EventHeatChanged += CallStaticHeatChanged;
        EventOverheated += CallStaticOverheated;

        //Due to execution order, isServer can be set to false the first time OnEnable is called. But afterwards it will be set correctly.
        if (isServer && _heatUpdate == null)
        {
            _heatUpdate = StartCoroutine(this.UpdateCoroutine(HEAT_UPDATES_PER_SECOND, UpdateHeat));
        }
    }

    // Use this for initialization
    private void Start()
    {
        //isServer is now set to the correct value.
        if (isServer && _heatUpdate == null)
        {
            _heatUpdate = StartCoroutine(this.UpdateCoroutine(HEAT_UPDATES_PER_SECOND, UpdateHeat));
        }
    }

    private void OnDisable()
    {
        EventHeatChanged -= CallStaticHeatChanged;
        EventOverheated -= CallStaticOverheated;

        if (_heatUpdate != null)
        {
            StopCoroutine(_heatUpdate);
            _heatUpdate = null;
        }
    }

    private void UpdateHeat(float deltaTime)
    {
        ChangeHeat(-coolingPerSecond * deltaTime);
    }

    private void OnValidate()
    {
        heatCapacity = Mathf.Max(0f, heatCapacity);
        _heat = Mathf.Clamp(_heat, 0f, heatCapacity);

        if (Application.isPlaying && isServer)
        {
            OnHeatChanged();
        }
    }
}

/// <summary>
/// Holds GameObject <see cref="senderObject"/>, NetworkedHeat <see cref="sender"/>, and float <see cref="newValue"/>
/// </summary>
[Serializable]
public struct HeatChangeArgs
{
    public GameObject senderObject;
    private NetworkedHeat _sender;
    public NetworkedHeat sender
    {
        get
        {
            if (_sender == null)
                _sender = senderObject.GetComponent<NetworkedHeat>();
            return _sender;
        }
    }
    public float newValue;

    public HeatChangeArgs(NetworkedHeat sender, float newValue)
    {
        _sender = sender;
        senderObject = sender.gameObject;
        this.newValue = newValue;
    }
}

/// <summary>
/// Holds GameObject <see cref="senderObject"/>, NetworkedHeat <see cref="sender"/>, and bool <see cref="isOverheated"/>
/// </summary>
[Serializable]
public struct OverheatedArgs
{
    public GameObject senderObject;
    private NetworkedHeat _sender;
    public NetworkedHeat sender
    {
        get
        {
            if (_sender == null)
                _sender = senderObject.GetComponent<NetworkedHeat>();
            return _sender;
        }
    }
    public bool isOverheated;

    public OverheatedArgs(NetworkedHeat sender, bool isOverheated)
    {
        _sender = sender;
        senderObject = sender.gameObject;
        this.isOverheated = isOverheated;
    }
}
