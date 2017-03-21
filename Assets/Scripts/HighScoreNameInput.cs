using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class HighScoreNameInput : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyUp (KeyCode.Return)) {
			if (GetComponent<InputField> ().text.Length == 0)
				return;
			HighScores.AddNewScore (GetComponent<InputField> ().text, ScoreManager.score ());
			HighScores.ShowLeaderBoard ();
			DisableHighScoreInput ();
		}
	}

	public static void DisableHighScoreInput(){
		if (!GameObject.Find ("HighScoreName"))
			return;
		Transform[] objects = GameObject.Find ("HighScoreName").GetComponentsInChildren<RectTransform> ();
		foreach (Transform o in objects)
			o.gameObject.SetActive (false);
		GameObject.Find ("HighScoreName").SetActive (false);
	}
}
