using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockOnUICamera : MonoBehaviour 
{
	[Tooltip("This should automatically get the Camera Eye on the Camera Rig, but you can set it to make sure.")]
	public Camera cameraEye;

	void Awake()
	{
		if (cameraEye == null)
			cameraEye = Camera.main;
	}

	void OnEnable()
	{
		StartCoroutine (LookAtCamera ());
	}

	void OnDisable()
	{
		StopCoroutine (LookAtCamera ());
	}

	IEnumerator LookAtCamera()
	{
		while (true) 
		{
			yield return new WaitForEndOfFrame ();
			transform.LookAt (cameraEye.transform);
		}
	}
}
