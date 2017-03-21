using UnityEngine;
using System.Collections;

public class GameOverTrigger : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerStay(Collider col){
		if (col.gameObject.GetComponent<Rigidbody> () && col.gameObject.GetComponent<Rigidbody> ().velocity.magnitude < 0.05f && !col.gameObject.GetComponent<CheckForMerge> ().isFalling () && col.gameObject.GetComponent<CheckForMerge>().blockTriggersGameOver()) {
			GameOver.DoGameOver ();
			Debug.Log ("Game over triggered by " + gameObject.name);
		}
	}
}
