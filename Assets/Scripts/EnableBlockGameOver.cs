using UnityEngine;
using System.Collections;

public class EnableBlockGameOver : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter(Collider col){
		if (col.gameObject.GetComponent<CheckForMerge> ())
			col.gameObject.GetComponent<CheckForMerge> ().setGameOver ();
	}
}
