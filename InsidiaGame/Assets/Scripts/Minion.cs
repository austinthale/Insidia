using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

[RequireComponent(typeof(AICharMovement))]
public class Minion : MonoBehaviour {

    public enum State { Follow, Attack, Regroup }
    public State state = State.Follow;
    private State prevState = State.Follow;
    public GameObject attackTarget = null;
    public float meleeAttackDistanceBuffer = 1.5f;

    public MinionSquad Squad { get; private set; }
    private AICharMovement _charMovement;

    //public event Action<Minion, GameCharacter> Attacked;

    //private void OnTakeDamage()
    //{
    //    if (Attacked != null)
    //        Attacked(this, other)
    //}

    public void ChangeSquad(MinionSquad newSquad)
    {
        if (newSquad.minions.Count < newSquad.capacity)
        {
            //Leave current squad code
            if (Squad != null)
            {
                Squad.minions.Remove(this);
                //unsub delegates

                //join new squad
                Squad = newSquad;
                Squad.minions.Add(this);
                //subscr to delegates
            }
            else
            {
                //Intialize self into squad
                Squad = newSquad;
                if (!Squad.minions.Contains(this))
                    Squad.minions.Add(this);

                //subscr to delegates
            }
        }
    }

    public void Start()
    {
        _charMovement = GetComponent<AICharMovement>();
    }

    public void OnEnable()
    {
        StartCoroutine(this.UpdateCoroutine(20f, MinionUpdate));
    }

    public void OnDisable()
    {
        StopAllCoroutines();
    }

    public void MinionUpdate()
    {
        if (state != prevState)
        {
            StateChanged(prevState, state);
            prevState = state;
        }

        switch (state)
        {
            case State.Attack:
                AttackUpdate();
                break;
            case State.Follow:
                FollowUpdate();
                break;
            case State.Regroup:
            default:
                RegroupUpdate();
                break;
        }
    }

    public void StateChanged(State prevState, State newState)
    {
        if (newState == State.Follow)
        {
            _charMovement.Agent.stoppingDistance = 0.1f;
        }
        else if (newState == State.Attack)
        {
            //Adjust stopping distance
            _charMovement.Agent.stoppingDistance = meleeAttackDistanceBuffer;
        }
    }

    public void AttackUpdate()
    {
        //Transition to Regroup state
        if (Squad && Vector3.Distance(transform.position, Squad.transform.position) > Squad.range)
        {
            state = State.Regroup;
            attackTarget = null;
            return;
        }

        //See if there's anything to attack
        attackTarget = FindAttackTarget();

        //Transition to follow state
        if (!attackTarget)
        {
            state = State.Follow;
            return;
        }

        //Move to the target
        _charMovement.Goal = attackTarget.transform.position;


        //Find edge-to-edge distance between our colliders
        //Vector3 targetClosestPoint = attackTarget.GetComponent<Collider>().ClosestPoint(transform.position);
        //float pointDistance = Vector3.Distance(transform.position, targetClosestPoint);

        if (Vector3.Distance(transform.position, attackTarget.transform.position) <= meleeAttackDistanceBuffer)
        {
            print("ATTACCCC!! " + attackTarget.name);
        }
    }

    private GameObject FindAttackTarget()
    {
        if (Squad.targetSensor.sensedObjects.Count > 0)
        {
            return GetClosestToPositionFromList(transform.position, Squad.targetSensor.sensedObjects);
        }

        return null;
    }

    private GameObject GetClosestToPositionFromList(Vector3 minionPosition, List<GameObject> objects)
    {
        float dist = float.PositiveInfinity;
        GameObject closestObj = null;

        foreach (var obj in objects)
        {
            float newDist = (minionPosition - obj.transform.position).sqrMagnitude;
            if (newDist <= dist)
            {
                dist = newDist;
                closestObj = obj;
            }
        }

        return closestObj;
    }

    public void FollowUpdate()
    {
        //Check if out of squad range, if so change to Regroup state and return;
        //Transition to Regroup state
        if (Squad && Vector3.Distance(transform.position, Squad.transform.position) > Squad.range)
        {
            state = State.Regroup;
            return;
        }

        //Don't set any movement goals here, it's being managed by the Squad.
    }



    public void RegroupUpdate()
    {
        if (Vector3.Distance(transform.position, Squad.transform.position) < Squad.range)
        {
            state = State.Follow;
        }
        else
        {
            _charMovement.Goal = Squad.transform.position;
        }
    }
}
