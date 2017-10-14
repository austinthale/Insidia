using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class attackMinion : MonoBehaviour {
    public static Action<bool> Thing;
    private bool EnemyAlone = true;


	// Use this for initialization
	void Start ()
    {
        Thing += canAttackPlayer;
	}

    private void OnTriggerStay(Collider other)
    {
        PlayerSquadManager.alone();
        if (EnemyAlone)
        {
            attackEnemyPlayer();
        }
        else
        {
            attackEnemyMinion();
        }
    }

    void attackEnemyPlayer()
    {
        print("Attacked");

    }
    void attackEnemyMinion()
    {
        print("attack minions");
    }

    void canAttackPlayer(bool t)
    {
        EnemyAlone = t;
    }
}
