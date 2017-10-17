using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class MenuButton_KH : MonoBehaviour {

	public static Action openMenu;
	public GameObject pauseMenu;
	public bool _menuIsOn = false;

	void Start (){
		_menuIsOn = false;
		pauseMenu.SetActive (false);
		openMenu += OpenMenuHandler;
	}

	private void OpenMenuHandler(){
		if (_menuIsOn) {
			pauseMenu.SetActive (false);
			_menuIsOn = false;
		} else if (!_menuIsOn) {
			ActivateMenu ();
		}
	}

	public void ActivateMenu (){
		if (_menuIsOn == false) {
			pauseMenu.SetActive (true);
			_menuIsOn = true;

			MenuPointer.startMenuRaycast ();
		}
	}

}
