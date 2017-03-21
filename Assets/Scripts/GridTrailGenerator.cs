using UnityEngine;
using System.Collections;

public class GridTrailGenerator : MonoBehaviour {
	public static float MinX;
	public static float MaxX;
	public static float MinY;
	public static float MaxY;
	public static int count;
	// Use this for initialization
	void Start () {
		MinX = GameObject.Find ("LeftGenerator").transform.Find ("Wall").transform.position.x;
		MaxX = GameObject.Find ("RightGenerator").transform.Find ("Wall").transform.position.x;
		MinY = GameObject.Find ("BottomGenerator").transform.Find ("Wall").transform.position.y;
		MaxY = GameObject.Find ("TopGenerator").transform.Find ("Wall").transform.position.y;
		Generate ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void Generate(){
		if (count > 4) {
			Invoke ("Generate", Random.Range (3,5));
			return;
		}
		Instantiate (Resources.Load ("GridTrailParticle"), new Vector3 ((int)Random.Range (MinX, MaxX)+0.5f, (int)Random.Range (MinY, MaxY) + 0.5f, 2.5f), Quaternion.identity);
		Invoke ("Generate", Random.Range (3,5));
		count++;
	}
}
