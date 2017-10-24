using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Networking;

public class BaseMelee : NetworkBehaviour {

    public static Action StartMeleeCount;
    public static Action ReleaseMeleeAttack;
    private bool _attacking;
    private bool _AttackCharged;
    private float _HeavyWait = 1.3f;
    private float _LightWait = 1f;
    //private Coroutine _heavy;


	// Use this for initialization
	void Start () {
        if(!isLocalPlayer)
        {
            Destroy(this);
            return;
        }
        _attacking = false;
        _AttackCharged = false;
        StartMeleeCount += StartMeleeCountHandler;
        ReleaseMeleeAttack += ReleaseMeleeAttackHandler;
	}

    private void ReleaseMeleeAttackHandler()
    {
        if(!_attacking && !_AttackCharged)
        {
            _attacking = true;
            LightHit.Light();
            StartCoroutine(ResetAttack(_LightWait));
        }
        if(!_attacking && _AttackCharged)
        {
            _attacking = true;
            HeavyHit.Heavy();
            StartCoroutine(ResetAttack(_HeavyWait));
        }
    }

    private void StartMeleeCountHandler()
    {
        if (!_attacking)
        {
            _AttackCharged = false;
            StartCoroutine(WaitForHeavy());
        }
    }

    IEnumerator WaitForHeavy ()
    {
        
        yield return new WaitForSeconds(2f);
        if (!_attacking)
        {
            _AttackCharged = true;
        }
    }

    IEnumerator ResetAttack(float _Time)
    {
        yield return new WaitForSeconds(_Time);
        _attacking = false;
        _AttackCharged = false;
    }
}
