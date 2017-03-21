using UnityEngine;
using System.Collections;

public class EndGameOptions : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void Confirm(){
		GameOver.DoGameOver ();
		gameObject.SetActive (false);
	}

	public void Cancel(){
		GameOver.ResumeGame ();
		gameObject.SetActive (false);
	}
}
