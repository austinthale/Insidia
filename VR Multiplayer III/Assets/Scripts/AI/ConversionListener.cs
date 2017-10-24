using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

[RequireComponent(typeof(Minion))]
public class ConversionListener : NetworkBehaviour {

    private Color _startColor;
    private Renderer _renderer;
    private bool _subscribed = false;
    private Minion _minion;

	// Use this for initialization
	void Awake () {
        _renderer = GetComponent<Renderer>();
        _startColor = _renderer.material.color;
        _minion = GetComponent<Minion>();
	}

    private void OnEnable()
    {
        if (!_subscribed && isServer)
        {
            _subscribed = true;
            PlayerAIConvert.MinionConversion += OnMinionConversion;
        }
    }

    private void Start()
    {
        if (!_subscribed && isServer)
        {
            _subscribed = true;
            PlayerAIConvert.MinionConversion += OnMinionConversion;
        }
    }

    private void OnMinionConversion(PlayerAIConvert playerConvert, Minion minion, float progress)
    {
        //If the minion being converted is this minion...
        if (minion == _minion)
        {
            //...then get the NetworkIdentity of the player that is converting us...
            NetworkIdentity netId = playerConvert.GetComponent<NetworkIdentity>();
            if (netId)
            {
                //...and tell them to color us green based on the conversion progress!
                TargetShowConversionProgress(netId.clientAuthorityOwner, Color.green, progress);
            }
        }
    }

    [TargetRpc]
	private void TargetShowConversionProgress(NetworkConnection target, Color color, float amount)
    {
        _renderer.material.color = Color.Lerp(_startColor, color, amount);
    }
}
