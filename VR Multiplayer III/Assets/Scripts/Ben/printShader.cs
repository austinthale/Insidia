using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class printShader : MonoBehaviour {

	// Use this for initialization
	void Start () {
        print(gameObject.GetComponent<Renderer>().material.shader);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
