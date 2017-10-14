using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI_Type_P : MonoBehaviour {
    public float radius = 2;
    public float speed = 5;
    public Transform destin;
    


    private IEnumerator travelCoroutine;
    private IEnumerator diviate;

    // Use this for initialization
    void Start()
    {
        travelCoroutine = travel(destin);
        StartCoroutine(travelCoroutine);
    }

    private IEnumerator travel(Transform destination)
    {
        while(Vector3.Distance(transform.position, destination.position) != 0.2f)
        {
            transform.position = Vector3.Lerp(transform.position, destination.position, speed * Time.deltaTime);
            yield return null;
        }
        yield return new WaitForSeconds(1f);
    }

    void OnTriggerEnter(Collider thing)
    {
        StopCoroutine(travelCoroutine);
        GameObject x = thing.gameObject;
        StartCoroutine(travel(x.transform));
    }

    private void OnTriggerExit(Collider other)
    {
        StopCoroutine(travelCoroutine);
        travelCoroutine = travel(destin);
        StartCoroutine(travelCoroutine);
    }



}
