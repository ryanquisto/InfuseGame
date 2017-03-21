using UnityEngine;
using System.Collections;
using System;
using UnityEngine.UI;

public class HighScores : MonoBehaviour {
	private static string[] names = new string[5];
	private static int[] scores = new int[5];
	private static GameObject panel;
	// Use this for initialization
	void Start () {
		ReadHighScores ();
		panel = GameObject.Find ("HighScoresContainer");
		panel.SetActive (false);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public static void ReadHighScores(){
		string namesString = PlayerPrefs.GetString ("HighScoreNames", "");
		int place = 0;
		while (namesString.Length > 0) {
			namesString = namesString.Trim ();
			names [place] = namesString.Substring (0, namesString.IndexOf (','));
			namesString = namesString.Remove (0, namesString.IndexOf (',') + 1);
			place++;
		}
		while (place < 5) {
			names [place] = "None";
			place++;
		}

		string scoresString = PlayerPrefs.GetString ("HighScores", "");
		place = 0;
		while (scoresString.Length > 0) {
			scoresString = scoresString.Trim ();
			scores [place] = Convert.ToInt32(scoresString.Substring (0, scoresString.IndexOf (',')));
			scoresString = scoresString.Remove (0, scoresString.IndexOf (',') + 1);
			place++;
		}
		while (place < 5) {
			scores [place] = 0;
			place++;
		}
	}

	public static void WriteHighScores(){
		string namesString = "";
		string scoresString = "";
		for (int i = 0; i < 5; i++) {
			if (scores [i] == 0)
				break;
			scoresString += scores [i].ToString () + ",";
			namesString += names [i].ToString () + ",";			
		}
		PlayerPrefs.SetString ("HighScoreNames", namesString);
		PlayerPrefs.SetString ("HighScores", scoresString);

	}


	public static void AddNewScore(string name, int score){
		if (score <= LowestScoreOnLeaderboard ())
			return;
		for (int i = 4; i >= 0; i--){
			if (i == 0 || score < scores [i - 1]) {
				scores [i] = score;
				names [i] = name;
				WriteHighScores ();
				break;
			}
			scores [i] = scores [i - 1];
			names [i] = names [i - 1];
		}
	}

	public static int LowestScoreOnLeaderboard(){
		return scores [4];
	}

	public static void ShowLeaderBoard(){
		ReadHighScores ();
		panel.SetActive (true);
		if (GameObject.Find ("HighScoreName"))
			HighScoreNameInput.DisableHighScoreInput ();
		Transform ScoresPanel = panel.transform.Find ("PlacesPanel");
		for (int i = 0; i < 5; i++) {
			ScoresPanel.Find ("Place" + (i + 1).ToString ()).GetComponent<Text> ().text = names [i];
			ScoresPanel.Find ("Points" + (i + 1).ToString ()).GetComponent<Text> ().text = (scores [i]).ToString();

		}
		
		
	}
}
