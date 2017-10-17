using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeactivateMenu_KH : MonoBehaviour {

	public GameObject pauseMenu;
	public MenuPointer menuScript;
	public GameObject circle;

	 void OnTriggerEnter (){
		if (menuScript._menuIsOn == true) {
			circle.GetComponent<Collider> ().enabled = false;

			menuScript._menuIsOn = false;
			LController.menuOpen = false;
			print (LController.menuOpen);
			pauseMenu.SetActive (false);
		}
	}
}
