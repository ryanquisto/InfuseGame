using UnityEngine;
using System.Collections;

public class GravityManager : MonoBehaviour {
	public static BlockGenerator activeGenerator;
	public float gravity;
	private RectTransform UIPanel;
	private float CanSwitch;
	public static bool KeyboardInput;
	// Use this for initialization
	void Start () {
		activeGenerator = transform.Find ("TopGenerator").GetComponent<BlockGenerator> ();
		Physics.gravity = new Vector3(0, gravity * -1, 0);
		UIPanel = GameObject.Find ("UIContainer").GetComponent<RectTransform> ();
		//Gravity can only be switched every 0.5 seconds, doing immediately back to back often had weird results
		CanSwitch = 0.5f;
		KeyboardInput = true;
	}
	
	// Update is called once per frame
	void Update () {
		if (CanSwitch > 0)
			CanSwitch -= Time.deltaTime;
		else if (!KeyboardInput)
			return;
		else {
			if (Input.GetKeyUp (KeyCode.UpArrow) && Physics.gravity.y != gravity) {
				Physics.gravity = new Vector3 (0, gravity, 0);
				//UIPanel.anchorMin = new Vector2 (1, 0);
				//UIPanel.anchorMax = new Vector2 (1, 0);
				UIPanel.anchoredPosition = new Vector2 (UIPanel.anchoredPosition.x, UIPanel.rect.height - GameObject.Find ("Canvas").GetComponent<RectTransform> ().rect.height + GameObject.Find ("Rules").GetComponent<RectTransform> ().rect.height);

				activeGenerator = transform.Find ("BottomGenerator").GetComponent<BlockGenerator> ();
				CheckForMerge[] cubes = GameObject.FindObjectsOfType<CheckForMerge> ();
				for (int i = 0; i < cubes.Length; i++)
					cubes [i].AdjustConstraints ('x');
			} else if (Input.GetKeyUp (KeyCode.DownArrow) && Physics.gravity.y != gravity*-1) {
				Physics.gravity = new Vector3 (0, gravity * -1, 0);
				UIPanel.anchoredPosition = new Vector2 (UIPanel.anchoredPosition.x, 0);
				activeGenerator = transform.Find ("TopGenerator").GetComponent<BlockGenerator> ();
				CheckForMerge[] cubes = GameObject.FindObjectsOfType<CheckForMerge> ();
				for (int i = 0; i < cubes.Length; i++)
					cubes [i].AdjustConstraints ('x');
			} else if (Input.GetKeyUp (KeyCode.RightArrow) && Physics.gravity.x != gravity) {
				Physics.gravity = new Vector3 (gravity, 0, 0);
				UIPanel.anchoredPosition = new Vector2 (UIPanel.anchoredPosition.x, 0);

				activeGenerator = transform.Find ("LeftGenerator").GetComponent<BlockGenerator> ();
				CheckForMerge[] cubes = GameObject.FindObjectsOfType<CheckForMerge> ();
				for (int i = 0; i < cubes.Length; i++)
					cubes [i].AdjustConstraints ('y');
			} else if (Input.GetKeyUp (KeyCode.LeftArrow) && Physics.gravity.x != gravity*-1) {
				Physics.gravity = new Vector3 (gravity * -1, 0, 0);
				UIPanel.anchoredPosition = new Vector2 (UIPanel.anchoredPosition.x, 0);

				activeGenerator = transform.Find ("RightGenerator").GetComponent<BlockGenerator> ();
				CheckForMerge[] cubes = GameObject.FindObjectsOfType<CheckForMerge> ();
				for (int i = 0; i < cubes.Length; i++)
					cubes [i].AdjustConstraints ('y');
			} else
				return;
			CanSwitch = 0.5f;
		}
	}
}
