using UnityEngine;
using System;
using System.Collections;

public interface ISensorListener
{
    void OnSensorEnter(TriggerSensor2 sensor, GameObject other);

    void OnSensorExit(TriggerSensor2 sensor, GameObject other);
}
