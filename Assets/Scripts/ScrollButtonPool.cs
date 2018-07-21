using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


/// <summary>
///  I like using enums when I want to properly describe an action instead of using a bool or a number
/// </summary>
public enum Shift { up, down };

/// <summary>
/// Scroll button pool, coupled with methods to manipulate Scroll Button Canvas
/// </summary>
public class ScrollButtonPool : MonoBehaviour {

	public int buttonsTotal = 7;
	public GameObject scrollButtonPrefab;
	public Transform scrollCanvas;
	public float margin = 10f;					// by fraction

	private int firstChoiceIndex;
	private int lastChoiceIndex;
	private Vector3 firstButtonPosition;
	private Vector3 lastButtonPosition;
	private float buttonWidth, buttonHeight, buttonCenterOffset, canvasHeight, canvasWidth;
	private Canvas canvas;
	private RectTransform canvasRT;
	private Vector3 buttonHeightVector;

	public string[] buttonLabels = {"Aprilia", "BMW", "Buell", "Ducati", "Harley Davidson", 
		"Honda", "Husqvarna", "Indian", "Kawasaki", "KTM", "Laverda", "Moto Guzzi", 
		"MV Agusta", "Norton", "Piaggio", "Royal", "Enfield", "Suzuki", "Triumph", "Ural", "Yamaha"
	};

	// initialize data structures
	public List<GameObject> scrollButtonPool = new List<GameObject>();	// strictly for pool, not necessary since we only need to hold two pointers, one to top, one to bottom
	public List<GameObject> scrollButtons = new List<GameObject> ();	// for order

	public static ScrollButtonPool instance;

	void Awake() {
		instance = this;
		firstChoiceIndex = 0;
		lastChoiceIndex = buttonsTotal - 1;
	}


	void Start () {

		// cache our variables

		canvas = scrollCanvas.GetComponent<Canvas> ();			// save canvas
		canvasRT = canvas.GetComponent<RectTransform> ();		// save rect transform

		canvasHeight = canvasRT.rect.height;					// height of canvas 
		canvasWidth = canvasRT.rect.width;						// width of canvas

		buttonWidth = canvasWidth - ((canvasWidth/margin)*2);	// add margin based on factor
		buttonHeight = canvasHeight / buttonsTotal;				// the button height based on total buttons
		buttonCenterOffset = buttonHeight / 2;					// center of button

		buttonHeightVector = new Vector3 (0f, buttonHeight, 0f);

		// initialize the pool with the names
		for (int i = 0; i < buttonsTotal; i++) {
			GameObject go = Instantiate (scrollButtonPrefab, scrollCanvas);
			ScrollButton sButton = go.GetComponent<ScrollButton> ();

			// set scale of buttons
			Vector2 dimensions =  new Vector2 (buttonWidth, buttonHeight);

			// Set intial positions
			float buttonX = canvasWidth / 2; 									// left justified
			float buttonY = canvasHeight - i*buttonHeight - buttonCenterOffset;	// height of button
			Vector3 placement = new Vector3 (buttonX, buttonY, 0f);

			// Initialize buttons
			sButton.Init (buttonLabels [i], placement, dimensions, canvas);

			scrollButtonPool.Add (go);
			scrollButtons.Add (go);

		}
	}
	/// <summary>
	/// Returns the first available pooled object
	/// </summary>
	/// <returns>The pooled game object.</returns>
	public GameObject GetPooledObj() {
		for (int i = 0; i < buttonsTotal; i++) {
			if (!scrollButtonPool [i].activeInHierarchy) {
				return scrollButtonPool [i];
			}
		}
		return null;
	}

	/// <summary>
	/// Shifts the button objects up or down. Uses the top or last button from queue to properly place it
	/// </summary>
	/// <param name="dir">Enum Shift for direction</param>

	public void ShiftSet(Shift dir) {
		// Find the first available (ie inactive) object
		GameObject recycledGO = GetPooledObj ();
		ScrollButton sbRecycledGO = recycledGO.GetComponent<ScrollButton> ();

		switch (dir) {
		case Shift.up:
			firstChoiceIndex = mod ((firstChoiceIndex + 1), buttonLabels.Length);
			lastChoiceIndex = mod ((lastChoiceIndex + 1), buttonLabels.Length);

			// find last item's button Position as starting point, save it
			Vector3 lastButtonPosition = scrollButtons [scrollButtons.Count - 1].transform.position;

			// move game object from top to bottom of list
			GameObject firstObj = scrollButtons [0];
			scrollButtons.RemoveAt (0);
			scrollButtons.Add (firstObj);

			sbRecycledGO.Init (buttonLabels [lastChoiceIndex], lastButtonPosition - buttonHeightVector);
			break;
		case Shift.down:
			firstChoiceIndex = mod((firstChoiceIndex - 1), buttonLabels.Length);
			lastChoiceIndex = mod((lastChoiceIndex - 1), buttonLabels.Length);

			// find first item's button Position as starting point
			Vector3 firstButtonPosition = scrollButtons[0].transform.position;

			// move game object from bottom to top of list
			int lastIndex = scrollButtons.Count-1;
			GameObject lastObj = scrollButtons [lastIndex];
			scrollButtons.RemoveAt (lastIndex);
			scrollButtons.Insert (0, lastObj);

			// Initialized the recyclced object
			sbRecycledGO.Init (buttonLabels [firstChoiceIndex], firstButtonPosition + buttonHeightVector);

			break;
		default:
			break;
		
		}
	}
	/// <summary>
	/// Mod the specified x and m and account for negative values
	/// </summary>
	/// <param name="x">First tuple of a mod</param>
	/// <param name="m">Second tuple of a mod</param>
	public int mod(int x, int m) {
		return (x%m + m)%m;
	}

}
