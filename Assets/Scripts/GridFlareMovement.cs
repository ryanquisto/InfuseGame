using UnityEngine;
using System.Collections;

public class GridFlareMovement : MonoBehaviour {
	private Vector3[] directions = {
		new Vector3(1,0,0),
		new Vector3(0,1,0),
		new Vector3(-1,0,0),
		new Vector3(0,-1,0)
	};
	private Vector3 direction;
	private int directionIndex;
	private float speed;
	private int badIndex1;
	private int badIndex2;
	private System.Random random;
	private ParticleSystem p;
	private bool moving;
	private float lifeSpan;

	// Use this for initialization
	void Start () {
		moving = true;
		random = new System.Random ();
		p = GetComponent<ParticleSystem> ();
		directionIndex = random.Next (0, 4);
		direction = directions [directionIndex];
		speed = random.Next (2, 5);
		p.emissionRate *= speed;
		InvokeRepeating ("ChangeDirection", 1 / speed, 1 / speed);
		lifeSpan = random.Next (3, 6);
		Invoke ("DestroyTrail", lifeSpan);
		PickColorParticle ();
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		if (moving)
			transform.position += Time.deltaTime * speed * direction;
	}

	void ChangeDirection(){
		badIndex2 = -1;
		if (directionIndex == 0)
			badIndex1 = 2;
		else if (directionIndex == 2)
			badIndex1 = 0;
		else if (directionIndex == 1)
			badIndex1 = 3;
		else
			badIndex1 = 1;
		if (transform.position.y >= GridTrailGenerator.MaxY)
			badIndex2 = 1;
		if (transform.position.y <= GridTrailGenerator.MinY)
			badIndex2 = 3;
		if (transform.position.x >= GridTrailGenerator.MaxX)
			badIndex2 = 0;
		if (transform.position.x <= GridTrailGenerator.MinX)
			badIndex2 = 2;
		do {
			directionIndex = random.Next (0, 4);
		} while(directionIndex == badIndex1 || directionIndex == badIndex2);
		direction = directions [directionIndex];
	}

	void DestroyTrail(){
		moving = false;
		p.Stop ();
		Invoke ("FinishDestroy", p.duration);

	}

	void FinishDestroy(){
		GridTrailGenerator.count--;
		Destroy (this.gameObject);
	}

	void PickColorParticle(){
		int color = random.Next (0, 3);
		ParticleSystem p = GetComponent<ParticleSystem> ();
		if (color == 0)
			p.startColor = new Color (1, 0.1f, 0.1f, 0.3f);
		else if (color == 1)
			p.startColor = new Color (0.1f, 0.1f, 1f, 0.3f);		
		else
			p.startColor = new Color (0.8f, 0.8f, 0.1f, 0.3f);
	}
	/**void PickColor(){
		int color = random.Next (0, 3);
		if (color == 0)
			r.material = Resources.Load ("blue_trail", typeof (Material)) as Material;
		else if (color == 1)
			r.material = Resources.Load ("red_trail", typeof (Material)) as Material;
		else
			r.material = Resources.Load ("yellow_trail", typeof (Material)) as Material;
	}*/

}
