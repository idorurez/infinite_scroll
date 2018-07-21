using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum ScrollOutPosition { top, bottom, none };

public class ScrollButton : MonoBehaviour {

	public bool visible = true;
	public Canvas canvas;						
	private RectTransform canvasRT;						// We could use the instance provided by the button pool, but let's make my life more complicated by passing the canvas
	private RectTransform rectT;						
	private Text buttonText;

	// Wake up! Give me my variables!
	void Awake() {
		rectT = GetComponent<RectTransform> ();
		buttonText = GetComponentInChildren<Text> ();
	}

	// Initial Init
	public void Init(string label, Vector3 position, Vector2 dimensions, Canvas parentCanvas) {
		buttonText.text = label;
		gameObject.transform.position = position;
		SetDimensions (dimensions);
		canvas = parentCanvas;
		canvasRT = canvas.GetComponent<RectTransform> ();

	}

	// Recycled Init
	public void Init(string label, Vector3 position) {
		buttonText.text = label;
		gameObject.transform.position = position;
		gameObject.SetActive (true);
	}

	public void Start() {

	}

	/// <summary>
	/// During the GUI event, check to see if the button is in or out of the canvas
	/// If it is, tur off the button, and then call the necessary methods to perform 
	/// the shift in the button pool.
	/// </summary>
	void OnGUI () {
		switch(CheckOnScreen()) {
		case ScrollOutPosition.top:
			gameObject.SetActive (false);
			ScrollButtonPool.instance.ShiftSet (Shift.up);
			break;
		case ScrollOutPosition.bottom:
			gameObject.SetActive (false);
			ScrollButtonPool.instance.ShiftSet (Shift.down);
			break;
		default:
			break;
		}
	}
		
	void OnEnable() {
		InputEvents.OnKeyUp += Move;
		InputEvents.OnKeyDown += Move;
	}

	void OnDisable() {
		InputEvents.OnKeyUp -= Move;
		InputEvents.OnKeyDown -= Move;
	}

	/// <summary>
	/// Callback to shift the button whatever distance
	/// </summary>
	/// <param name="distance">Distance.</param>
	public void Move(float distance) {
		transform.position += new Vector3 (0f, distance, 0f);
	}

	/// <summary>
	/// Sets the dimensions of the button
	/// </summary>
	/// <param name="dim">Dim.</param>
	public void SetDimensions(Vector2 dim) {
		rectT.sizeDelta = dim;
	}

	/// <summary>
	/// Check to see if the button is on the Screen
	/// </summary>
	/// <returns>enum ScrollOutPosition where the button exited the canvas area.</returns>
	public ScrollOutPosition CheckOnScreen() {
		// check if it's off the top or bottom, based on center of button
		if (gameObject.transform.position.y > canvasRT.rect.height) {
			return ScrollOutPosition.top;
		} else if (gameObject.transform.position.y < 0) {
			return ScrollOutPosition.bottom;
		}
		return ScrollOutPosition.none;
	}

}