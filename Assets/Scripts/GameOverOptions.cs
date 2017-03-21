using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;


public class GameOverOptions : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void Replay(){
		ScoreManager.Reset ();
		Time.timeScale = 1;
		SceneManager.LoadScene("scene1");
		CheckForMerge.StacksForPoints = 4;
	}

	public void Quit(){
		Application.Quit ();
	}
}
