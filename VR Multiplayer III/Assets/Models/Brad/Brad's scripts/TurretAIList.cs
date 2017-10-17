using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class TurretAIList : MonoBehaviour {

    //Use this Codes for Turrets

    /// <summary>
    /// Brad Beachell
    /// 
    /// The goal of this script is to detect an intruder. when there is an intruder the turret will look at and shoot 
    /// at the intruder. 
    /// 
    /// Must haves to make this work:
    /// You must have an object like a sphere or cylinder object to put this script on. Scale it to the size of the
    /// trigger area. Once you have it to the desired size you then turn off the mesh renderer and put the
    /// turret head as a child of that cylinder.
    /// </summary>

    List<GameObject> targetDinosaurs = new List<GameObject>();
    public float smoothValue = 5;
    
    public void OnTriggerEnter(Collider Target)
    {
        targetDinosaurs.Add(Target.gameObject);
        if(targetDinosaurs.Count == 1)
            StartCoroutine(TargetingSystem());


        
    }


    public void OnTriggerExit(Collider Target)
    {
        targetDinosaurs.Remove(Target.gameObject);
        
        if(targetDinosaurs.Count == 0)
            StopAllCoroutines();
    }

    IEnumerator TargetingSystem()
    {
        while (true)
        {

            
            Vector3 direction = targetDinosaurs[0].transform.position - transform.position;
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction, Vector3.up), smoothValue * Time.deltaTime);
            yield return null;
            //enter the calling for the shooting here

        }


    }

}



/// to use a remove from an object from a list use 

///attack targetDinosaurs[0] always;

///lookat.targetDinosaurs[0]























    

