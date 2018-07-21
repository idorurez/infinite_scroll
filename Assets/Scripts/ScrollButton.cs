using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum ScrollOutPosition { top, bottom, none };

public class ScrollButton : MonoBehaviour {

	public bool visible = true;
	public Canvas canvas;						
	private RectTransform canvasRT;
	private RectTransform rectT;
	private Text buttonText;

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
		Debug.Log ("Init @ " + position);
		buttonText.text = label;
		gameObject.transform.position = position;
		gameObject.SetActive (true);
	}

	public void Start() {

	}

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

	public void Move(float distance) {
		transform.position += new Vector3 (0f, distance, 0f);
	}

	public void SetDimensions(Vector2 dim) {
		rectT.sizeDelta = dim;
	}

	public ScrollOutPosition CheckOnScreen() {
		// check if it's off the top or bottom
		if (gameObject.transform.position.y > canvasRT.rect.height) {
			return ScrollOutPosition.top;
		} else if (gameObject.transform.position.y < 0) {
			return ScrollOutPosition.bottom;
		}
		return ScrollOutPosition.none;
	}

	public void PrintLabel() {
		Debug.Log (buttonText.text);
	}
}