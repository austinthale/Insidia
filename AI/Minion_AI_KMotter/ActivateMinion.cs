using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class is designed to be attached to an object with a trigger collider that will
///     only react to a PC (generally done though physics layers).
///     
/// It requires an associated 'minion' prefab (stored as Transform) that it activates
///     (using Activate() within the MinionNavigation class) upon being triggered.
///     
/// This script disables its object upon being triggered.
/// </summary>
public class ActivateMinion : MonoBehaviour {

    public Transform minion;
    MinionNavigation ai;

	// Use this for initialization
	void Start () {
        ai = minion.GetComponent<MinionNavigation>();
	}

    private void OnTriggerEnter(Collider other)
    {
        ai.owningPlayer = other.transform;
        ai.Activate();
        gameObject.SetActive(false);
    }
}
