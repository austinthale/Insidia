using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class MenuPointer : MonoBehaviour {

	public static Action openMenu;
	public GameObject pauseMenu;
	public bool _menuIsOn = false;

	RaycastHit rayHit;
	public GameObject circle;
	LineRenderer line;

	public static Action startMenuRaycast;
	public static Action clickMenuButton;

	void Start(){
		startMenuRaycast += startMenuRaycastHandler;
		circle.GetComponent<Collider> ().enabled = false;

		_menuIsOn = false;
		pauseMenu.SetActive (false);
		openMenu += OpenMenuHandler;
		clickMenuButton += clickMenuButtonHandler;
	}

	private void startMenuRaycastHandler(){
		StartCoroutine(RaycastPointer());
	}

	IEnumerator RaycastPointer(){

		line = gameObject.GetComponent<LineRenderer> ();

		if (Physics.Raycast (transform.position, transform.up * -1, out rayHit)) {
			circle.transform.position = rayHit.point;
		}
		//line.SetPosition (0, transform.position);
		//line.SetPosition (1, rayHit.point);
		//line.enabled = true;

		yield return new WaitForFixedUpdate ();
		if (_menuIsOn) {
			
			StartCoroutine (RaycastPointer ());
		}
	}

	private void OpenMenuHandler(){
		if (_menuIsOn) {
			pauseMenu.SetActive (false);
			_menuIsOn = false;
		} else if (!_menuIsOn) {
			ActivateMenu ();
		}
	}

	private void clickMenuButtonHandler(){
		StartCoroutine (PointerColliderOn ());
	}

	IEnumerator PointerColliderOn (){
		circle.GetComponent<Collider> ().enabled = true;
		yield return new WaitForSeconds (0.1f);
		circle.GetComponent<Collider> ().enabled = false;
	}

	public void ActivateMenu (){
		if (_menuIsOn == false) {
			pauseMenu.SetActive (true);
			_menuIsOn = true;

			MenuPointer.startMenuRaycast ();
		}
	}
}
