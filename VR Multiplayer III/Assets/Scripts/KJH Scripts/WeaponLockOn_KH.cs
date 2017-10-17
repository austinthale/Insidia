using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class WeaponLockOn_KH : MonoBehaviour {

	public static Action <Controller> setFocus;

	private Controller _controller;

	LineRenderer line;
	RaycastHit rayHit;

	public bool canRaycast;
	public float rayTimeLimit = 2f;
	public float lineEndWidth = .5f;

	public void Start(){
		setFocus += SetFocusHandler;
		line = gameObject.GetComponent<LineRenderer> ();
		line.enabled = false;
		line.startWidth = 0;
		line.endWidth = lineEndWidth;
	}

	private void SetFocusHandler(Controller _getFocus){
		canRaycast = true;
		_controller = _getFocus;
		_controller.allFocus.Clear ();
		StartCoroutine (RaycastWeapon ());
		StartCoroutine (RaycastCounter ());
	}

	//
	IEnumerator RaycastWeapon(){

		if (Physics.Raycast (transform.position, transform.up * -1, out rayHit)) {
			line.SetPosition (0, transform.position);
			line.SetPosition (1, rayHit.point);
			line.enabled = true;

			if (rayHit.collider.GetComponent <Renderer>().material.shader == _controller.Outlineable) {
				if (_controller.focus != rayHit.collider.GetComponent<Transform> ()) {
					if (_controller.focus != null){
						_controller.focus.GetComponent<Renderer>().material.SetFloat ("_OutlineTransparency", 0);
					}
					_controller.focus = rayHit.collider.GetComponent<Transform> ();
					_controller.focus.GetComponent<Renderer>().material.SetFloat ("_OutlineTransparency", 1);

					if(Controller.AddAsTargetUI != null && _controller.focus.gameObject != null)
						Controller.AddAsTargetUI(_controller.focus.gameObject);
					
					if (_controller.allFocus.Count != 0) {
						if (!_controller.allFocus.Contains (_controller.focus)) {
							_controller.allFocus.Add (_controller.focus);
						}
					} else {
						_controller.allFocus.Add (_controller.focus);
					}
				}
			}
		}
		yield return new WaitForFixedUpdate ();
		if (canRaycast)
			StartCoroutine(RaycastWeapon());
	}

	IEnumerator RaycastCounter (){
		yield return new WaitForSeconds (rayTimeLimit);
		canRaycast = false;
		line.enabled = false;

	}
}
