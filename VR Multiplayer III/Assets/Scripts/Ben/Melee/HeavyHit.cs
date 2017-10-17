using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Networking;

public class HeavyHit : NetworkBehaviour {

    public static Action Heavy;

    public float heavyAttack;
    public GameObject heavyA;

    private MeshRenderer _thisRenderer;
    private Collider _thisCollider;
    

	// Use this for initialization
	void Start () {
        if (!isLocalPlayer)
        {
            Destroy(this);
            return;
        }
        //heavyA = gameObject;
        Heavy += HeavyHandler;
        _thisRenderer = heavyA.GetComponent<MeshRenderer>();
        _thisCollider = heavyA.GetComponent<Collider>();
}

    private void HeavyHandler()
    {
        StartCoroutine(StartHeavyAttack());
    }

    IEnumerator StartHeavyAttack()
    {
        _thisRenderer.enabled = true;
        _thisCollider.enabled = true;
        yield return new WaitForSeconds(.8f);
        _thisRenderer.enabled = false;
        _thisCollider.enabled = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.GetComponent<Rigidbody>())
        {
            print("Hit a thingy!");
            //will be used when we have health scripts
            /*if(other.GetComponent<Health>())
            {
                other.GetComponent<Health>() - heavyAttack;
            }
            */
        }
    }
}
