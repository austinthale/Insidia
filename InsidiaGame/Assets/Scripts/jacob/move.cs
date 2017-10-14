using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class move : MonoBehaviour {

    public float speed;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        var x = Input.GetAxisRaw("Horizontal") * speed;
        var y = Input.GetAxisRaw("Vertical") * speed;
        transform.Translate(0, 0, y);
        transform.Rotate(0, x, 0);
	}
}