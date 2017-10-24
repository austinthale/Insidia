using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Networking;

public class Bullet : NetworkBehaviour {

    public Transform buletStore;


    private void OnEnable()
    {
        
        BulletPool.bullets.Add(gameObject.GetComponent<Rigidbody>());
        
    }

    

    private void OnCollisionEnter(Collision collision)
    {
        BulletPool.shooting = false;
        gameObject.GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 0);
        gameObject.transform.position = buletStore.position;
        BulletPool.bullets.Add(gameObject.GetComponent<Rigidbody>());
    }

    public void TimeOutStarter()
    {
        StartCoroutine(TimeOut());
    }

    IEnumerator TimeOut()
    {
        yield return new WaitForSeconds(3);
        BulletPool.shooting = false;
        gameObject.GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 0);
        gameObject.transform.position = buletStore.position;
        BulletPool.bullets.Add(gameObject.GetComponent<Rigidbody>());
    }
}
