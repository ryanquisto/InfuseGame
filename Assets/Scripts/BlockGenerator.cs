using UnityEngine;
using System.Collections;

public class BlockGenerator : MonoBehaviour {
	public static float InitialDropRate = 1f;
	private static float DropRate;
	private string[] BlockOptions = {"YellowBlock", "RedBlock", "BlueBlock"};
	private int NumDropLocations;
	private bool inactive;
	private GameObject wall;

	// Use this for initialization
	void Start () {
		//Subtract number of children that aren't drop locations
		NumDropLocations = transform.childCount - 1;
		inactive = true;
		wall = transform.Find ("Wall").gameObject;
		DropRate = InitialDropRate;
	}
	
	// Update is called once per frame
	void Update () {
		if (GravityManager.activeGenerator == this && inactive) {
			inactive = false;
			InvokeRepeating ("DropBlock", 0.5f, DropRate);
			wall.SetActive (false);
		} else if (!inactive && GravityManager.activeGenerator != this) {
			CancelInvoke ();
			inactive = true;
			wall.SetActive (true);
		}
			
	
	}

	void DropBlock(){
		int ColorChoice = Random.Range (0, BlockOptions.Length);
		int DropLocation = Random.Range (0, NumDropLocations - 1);
		Instantiate (Resources.Load (BlockOptions [ColorChoice]), transform.Find ("Location" + DropLocation.ToString()).transform.position, Quaternion.identity);
	}

	public static void MultiplyDropRate(float multiplier){
		DropRate = InitialDropRate / multiplier;
		GravityManager.activeGenerator.CancelInvoke ();
		GravityManager.activeGenerator.InvokeRepeating("DropBlock", 0.5f, DropRate);
	}

	public static float GetDropRateMultiple(){
		return DropRate/InitialDropRate;
	}
}
