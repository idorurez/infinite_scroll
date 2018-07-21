using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputEvents : MonoBehaviour {

	// Could have made a single/generic I guess
	public delegate void KeyUp (float distance);			// up signature
	public delegate void KeyDown (float distance);			// down signature

	public static event KeyUp OnKeyUp;
	public static event KeyDown OnKeyDown;

	public float scrollSpeed;

	void Update () {
		if (Input.GetKey("up") || (Input.GetAxis("Mouse ScrollWheel") < 0f)) {
			OnKeyUp (scrollSpeed);
		}

		if (Input.GetKey ("down")  || (Input.GetAxis("Mouse ScrollWheel") > 0f)) {
			OnKeyDown (-scrollSpeed);
		}
	}
}
