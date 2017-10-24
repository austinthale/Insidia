using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockOnUI : MonoBehaviour 
{
	private Canvas _canvas;
	private LockOnUIColor colorScript;

	[Tooltip("Color of sprites")]
	public Color color = Color.red;

	[Tooltip("Allows color to change depending on layer of target")]
	public bool differentColors = true;

	void Start()
	{
		Controller.AddAsTargetUI += TargetHandler;
		colorScript = GetComponentInChildren<LockOnUIColor> ();
		_canvas = GetComponentInChildren<Canvas> ();
		_canvas.gameObject.SetActive(false);
	}
	
	void NoTarget () 
	{
		_canvas.gameObject.SetActive(false);
		transform.parent = null;
	}

	void SetTarget(GameObject _target)
	{
		if(differentColors)
			ChangeColor (_target);
		else
			colorScript.color = color;
		_canvas.gameObject.SetActive(false);
		_canvas.gameObject.SetActive(true);
		transform.position = _target.transform.position;
		transform.parent = _target.transform.parent;
	}

	void ChangeColor(GameObject _target)
	{
		if (LayerMask.LayerToName (_target.layer) == "Enemy")
			color = Color.red;
		else if (LayerMask.LayerToName (_target.layer) == "Obstacle")
			color = Color.cyan;
		else if (LayerMask.LayerToName (_target.layer) == "Player")
			color = Color.green;

		colorScript.color = color;
	}

	void TargetHandler(GameObject _target)
	{
		if (_target != null)
			SetTarget (_target);
		else
			NoTarget ();
	}
}
