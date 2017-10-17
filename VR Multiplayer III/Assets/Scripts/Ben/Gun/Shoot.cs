using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Networking;

public class Shoot : NetworkBehaviour {
    public static Action shooter;

    public Rigidbody currentBullet;
    public float bulletSpeed;
    

    //public GameObject gun;
    private void Start()
    {
        if (!isLocalPlayer)
        {
            Destroy(this);
            return;
        }

        shooter += ShooterHandler;
        bulletSpeed = 100;
    }

    private void ShooterHandler()
    {
        //if (!BulletPool.shooting)
        //{
            currentBullet = BulletPool.bullets[0];
            currentBullet.GetComponent<Bullet>().TimeOutStarter();
            BulletPool.bullets.Remove(currentBullet);
            currentBullet.transform.position = transform.position + (transform.forward * 5);
            currentBullet.transform.rotation = transform.rotation;
            currentBullet.transform.localEulerAngles = transform.localEulerAngles;
            currentBullet.GetComponent<Rigidbody>().AddRelativeForce(Vector3.forward * bulletSpeed * 10);
            if (BulletPool.bullets != null)
            {
                currentBullet = BulletPool.bullets[0];
            }
            else
                print("Not Enough Bullets");
        //}
    }
}
