using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Created by Christian Clark
/// </summary>
[RequireComponent(typeof(MinionSquad))]
public class PlayerAIConvert : MonoBehaviour, ISensorListener {

    private MinionSquad _squad;
    public TriggerSensor2 minionSensor;
    public Dictionary<Minion, float> convertingMinions = new Dictionary<Minion, float>();

    public float timeToConvert = 3f;

    public static event Action<PlayerAIConvert, Minion, float> MinionConversion;

    private void Awake()
    {
        _squad = GetComponent<MinionSquad>();
    }


    private void Start()
    {
        minionSensor.Setup(this, FilterForMinion);
    }

    private bool FilterForMinion(GameObject other)
    {

        Minion minion = other.GetComponent<Minion>();
        return (minion && IsMinionValidConversionTarget(minion));

    }

    private bool IsMinionValidConversionTarget(Minion minion)
    {
        return ((_squad.minions.Count < _squad.capacity) && (minion.tag == "AI Neutral") && (minion.state != Minion.State.Attack));
    }

    public void OnSensorEnter(TriggerSensor2 sensor, GameObject other)
    {
        Minion minion = other.GetComponent<Minion>();

        if (!convertingMinions.ContainsKey(minion))
        {
            convertingMinions.Add(minion, 0f);
            StartCoroutine(ConvertNeutralAI(minion));
        }
    }

    public void OnSensorExit(TriggerSensor2 sensor, GameObject other) { }

    private IEnumerator ConvertNeutralAI(Minion minion)
    {
        float lastUpdateTime = Time.time;
        float deltaTime = 0f;

        while (true)
        {
            yield return new WaitForSeconds(0.25f);
            deltaTime = Time.time - lastUpdateTime;
            lastUpdateTime = Time.time;

            //If the minion is no longer valid, stop the coroutine as it is definately not getting converted.
            if (!IsMinionValidConversionTarget(minion))
            {
                convertingMinions.Remove(minion);
                ShowConversionProgress(minion, 0f);
                yield break;
            }

            //Update the progressed based on whether or not it's in the sensor range.
            float conversionProgress = convertingMinions[minion];
            conversionProgress = Mathf.Clamp(conversionProgress + ((minionSensor.sensedObjects.Contains(minion.gameObject)) ? deltaTime : -deltaTime), 0, timeToConvert);
            convertingMinions[minion] = conversionProgress;
            if (MinionConversion != null)
                MinionConversion(this, minion, conversionProgress / timeToConvert);

            //ShowConversionProgress(minion, conversionProgress / timeToConvert);

            //Conversion success!
            if (conversionProgress >= timeToConvert)
            {
                //Convert the guy
                Convert(minion);
                //Remove him from the set of minions being currently converted.
                convertingMinions.Remove(minion);
                //And stop the coroutine.
                yield break;
            }
            //Conversion failure...
            else if (conversionProgress <= 0f)
            {
                convertingMinions.Remove(minion);
                yield break;
            }
        }
    }

    private void ShowConversionProgress(Minion minion, float percent)
    {
        minion.GetComponent<Renderer>().material.color = Color.Lerp(Color.yellow, Color.green, percent);
    }

    private void Convert(Minion minion)
    {
        minion.tag = "AI Friendly";
        //minion.GetComponent<Renderer>().material.color = Color.green;

        minion.ChangeSquad(_squad);
    }
}