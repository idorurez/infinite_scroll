using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputEvents : MonoBehaviour {

	public delegate void KeyUp (float distance);
	public delegate void KeyDown (float distance);

	public static event KeyUp OnKeyUp;
	public static event KeyDown OnKeyDown;

	public float scrollSpeed;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void OnGUI () {
		if (Input.GetKey("up")) {
			OnKeyUp (scrollSpeed);
		}

		if (Input.GetKey ("down")) {
			OnKeyDown (-scrollSpeed);
		}
	}
}
