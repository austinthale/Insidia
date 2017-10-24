using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class QuitGame_KH : MonoBehaviour {

	public GameObject pauseMenu;
	public MenuPointer menuScript;
	public GameObject circle;

	public Scene menuScene;

	void OnTriggerEnter(){
		
		circle.GetComponent<Collider> ().enabled = false;
		pauseMenu.SetActive (false);
		menuScript._menuIsOn = false;
		SceneManager.LoadScene (0);
	}
}
