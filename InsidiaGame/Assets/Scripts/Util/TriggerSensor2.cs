using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
[RequireComponent(typeof(Collider))]
public class TriggerSensor2 : MonoBehaviour {

    private Collider _collider;

    private ISensorListener _listener;
    private MonoBehaviour _listenerBehaviour;
    public MonoBehaviour Listener { get { return _listenerBehaviour; } }

    public List<GameObject> sensedObjects = new List<GameObject>();
    public Predicate<GameObject> Filter = (gameObject => true);
    public bool notifyIfInactive = false;
    
    
    public void Setup<T>(T listener) where T : MonoBehaviour, ISensorListener
    {
        _listener = listener;
        _listenerBehaviour = listener;
    }

    public void Setup<T>(T listener, Predicate<GameObject> filter) where T : MonoBehaviour, ISensorListener
    {
        _listener = listener;
        _listenerBehaviour = listener;
        Filter = filter;
    }

    private void Awake()
    {
        _collider = GetComponent<Collider>();
    }

    private void OnEnable()
    {
        _collider.enabled = true;
    }

    private void OnDisable()
    {
        _collider.enabled = false;
    }

    public void OnTriggerEnter(Collider target)
    {
        if (Filter(target.gameObject))
        {
            sensedObjects.Add(target.gameObject);
            if (_listener != null && (notifyIfInactive || _listenerBehaviour.enabled))
                _listener.OnSensorEnter(this, target.gameObject);
        }
    }


    public void OnTriggerExit(Collider target)
    {
        if (sensedObjects.Remove(target.gameObject))
            if (_listener != null && (notifyIfInactive || _listenerBehaviour.enabled))
                _listener.OnSensorExit(this, target.gameObject);
    }

}























    

