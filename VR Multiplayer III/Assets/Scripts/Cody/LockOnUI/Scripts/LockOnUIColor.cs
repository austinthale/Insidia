using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockOnUIColor : MonoBehaviour 
{
	[HideInInspector]
	public Color color;

	private SpriteRenderer[] _children;

	void Awake()
	{
		_children = gameObject.GetComponentsInChildren<SpriteRenderer> ();
	}

	void OnEnable()
	{
		for (int i = 0; i < _children.Length; i++)
		{
			_children [i].color = color;
		}
	}
}
