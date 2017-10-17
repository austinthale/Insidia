using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Networking;

public class LightHit : NetworkBehaviour {

    public static Action Light;

    public float lightAttack;
    public GameObject lightA;

    private MeshRenderer _thisRenderer;
    private Collider _thisCollider;

    // Use this for initialization
    void Start () {
        if (!isLocalPlayer)
        {
            Destroy(this);
            return;
        }
        //lightA = gameObject;
        Light += LightAttackHandler;
        _thisRenderer = lightA.GetComponent<MeshRenderer>();
        _thisCollider = lightA.GetComponent<Collider>();
	}

    private void LightAttackHandler()
    {
        StartCoroutine(StartLightAttack());
    }

    IEnumerator StartLightAttack()
    {
        _thisRenderer.enabled = true;
        _thisCollider.enabled = true;
        yield return new WaitForSeconds(.5f);
        _thisCollider.enabled = false;
        _thisRenderer.enabled = false; 
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Rigidbody>())
        {
            print("Hit a thingy!");
            //will be used when we have health scripts
            /*if(other.GetComponent<Health>())
            {
                other.GetComponent<Health>() - lightAttack;
            }
            */
        }
    }

}
