using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour {
	public static bool lost;
	private static GameObject HighScoreNameInput;
	private static GameObject EndGame;
	public static bool paused;
	private static float dropRate;
	// Use this for initialization
	void Start () {
		HighScoreNameInput = GameObject.Find ("HighScoreName");
		HighScoreNameInput.SetActive (false);
		EndGame = GameObject.Find ("EndGamePanel");
		//RectTransform[] toDisable = EndGame.GetComponentsInChildren<RectTransform> ();
		//foreach (RectTransform r in toDisable)
		//	r.gameObject.SetActive (false);
		EndGame.SetActive (false);
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyUp (KeyCode.Escape)) {
			EndGame.SetActive (true);
			PauseGame ();
		}
			
	}
		

	public static void DoGameOver(){
		if (ScoreManager.score () > HighScores.LowestScoreOnLeaderboard () && ScoreManager.score() < 2500)
			HighScoreNameInput.SetActive (true);
		else
			HighScores.ShowLeaderBoard ();
		PauseGame ();
	}

	public static void PauseGame(){
		BlockGenerator.MultiplyDropRate (0);
		ScoreManager.pauseClock ();
		GravityManager.KeyboardInput = false;
		paused = true;
	}

	public static void ResumeGame(){
		BlockGenerator.MultiplyDropRate (ScoreManager.dropRate);
		ScoreManager.resumeClock ();
		GravityManager.KeyboardInput = true;
		paused = false;
	}
}
