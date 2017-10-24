using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CharacterSelect_VR : MonoBehaviour 
{
	private SteamVR_TrackedController _controller;
	private SteamVR_TrackedObject trackedObject;
	private SteamVR_Controller.Device device;

	public Vector2 touchSpot;
	public bool touching;
	public bool triggerClicked;
	public bool pulledTrigger;
	public bool padClicked;

	public Animator anim;
	public List<GameObject> characters;
	int currentCharacter = 0;

	private void OnEnable()
	{
		_controller = GetComponent<SteamVR_TrackedController> ();
		trackedObject = GetComponent<SteamVR_TrackedObject> ();
		_controller.TriggerClicked += HandleTriggerClicked;
		_controller.TriggerUnclicked += HandleTriggerUnclicked;
		_controller.PadTouched += HandlePadTouched;
		_controller.PadUntouched += HandlePadUntouched;
		_controller.PadClicked += HandlePadClicked;
		_controller.PadUnclicked += HandlePadUnclicked;
		touching = false;
		pulledTrigger = false;
		triggerClicked = false;
		padClicked = false;
	}

	private void HandleTriggerClicked( object sender, ClickedEventArgs e)
	{
		triggerClicked = true;
		int scene = SceneManager.GetActiveScene().buildIndex;
		SceneManager.LoadScene(scene + 1, LoadSceneMode.Single);
	}

	private void HandleTriggerUnclicked( object sender, ClickedEventArgs e)
	{
		triggerClicked = false;
	}

	private void HandlePadTouched( object sender, ClickedEventArgs e)
	{
		touching = true;
	}

	private void HandlePadUntouched( object sender, ClickedEventArgs e)
	{
		touching = false;
	}

	private void HandlePadClicked( object sender, ClickedEventArgs e)
	{
		padClicked = true;
        StartCoroutine(ChangeCharacter());
    }

    private void HandlePadUnclicked( object sender, ClickedEventArgs e)
	{
		padClicked = false;
	}

	void Start () 
	{
		HologramAppear();
	}

	IEnumerator ChangeCharacter()
	{
		yield return new WaitForFixedUpdate ();
		device = SteamVR_Controller.Input ((int)trackedObject.index);
		touchSpot = new Vector2 (device.GetAxis ().x, device.GetAxis ().y);

		if (touchSpot.x > .1f && padClicked == true) 
		{
			characters [currentCharacter].SetActive (false);
			if (currentCharacter == 0)
				currentCharacter = characters.Count - 1;
			else
				currentCharacter -= 1;
			
			HologramAppear ();
		} 
		else if (touchSpot.x < -.1f && padClicked == true) 
		{
			characters [currentCharacter].SetActive (false);
			
			if (currentCharacter == characters.Count - 1)
				currentCharacter = 0;
			else
				currentCharacter += 1;
			
			HologramAppear ();
		}
    }

	void ChangeCharacter(string _s)
	{
		string tempDino = _s.Replace ("Hologram", "");
		StaticVars.characterP1 = (StaticVars.dinoList)System.Enum.Parse (typeof(StaticVars.dinoList), tempDino);
	}

	void HologramAppear()
	{
		characters [currentCharacter].SetActive (true);
		ChangeCharacter (characters [currentCharacter].name);
		anim.PlayInFixedTime ("in");
		print (StaticVars.characterP1);
	}

    private void OnDisable()
    {
        _controller.TriggerClicked -= HandleTriggerClicked;
        _controller.TriggerUnclicked -= HandleTriggerUnclicked;
        _controller.PadTouched -= HandlePadTouched;
        _controller.PadUntouched -= HandlePadUntouched;
        _controller.PadClicked -= HandlePadClicked;
        _controller.PadUnclicked -= HandlePadUnclicked;
    }
}