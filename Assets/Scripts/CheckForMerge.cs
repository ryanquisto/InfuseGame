using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CheckForMerge : MonoBehaviour {
	public static int StacksForPoints = 4;
	private int stacks;
	private bool falling;
	private int CheckCount;
	private enum Color {
		Red,
		Yellow,
		Blue,
		Purple,
		Green,
		Orange,
	};
	private Color color;
	private bool canTriggerGameOver;

	// Use this for initialization
	void Start () {
		if (stacks == 0) {
			stacks = 1;
			falling = true;
		}
		if (name.Contains ("Blue")) {
			color = Color.Blue;
		} else if (name.Contains ("Yellow")) {
			color = Color.Yellow;
		} else if (name.Contains ("Red")) {
			color = Color.Red;
		} else if (name.Contains ("Purple")) {
			color = Color.Purple;
			canTriggerGameOver = true;
		} else if (name.Contains ("Green")) {
			color = Color.Green;
			canTriggerGameOver = true;
		} else if (name.Contains ("Orange")) {
			color = Color.Orange;
			canTriggerGameOver = true;
		}

	}
	
	// Update is called once per frame
	void Update () {
		if (GetComponent<Rigidbody> ().velocity.magnitude > Physics.gravity.magnitude){
			GetComponent<Rigidbody> ().velocity = Physics.gravity;
		}

	}

	void OnCollisionEnter(Collision col){
		if (col.gameObject.GetComponent<CheckForMerge> () && Vector3.Magnitude (transform.position - col.transform.position - Physics.gravity / Vector3.Magnitude (Physics.gravity)) < 0.1) {//(AbsPositionDiff.x < GravityDirection.x) && (AbsPositionDiff.y < GravityDirection.y) && (AbsPositionDiff.z < GravityDirection.z)) {
			GetComponent<Rigidbody> ().velocity = Vector3.zero;
			merge (col.gameObject.GetComponent<CheckForMerge> ().gameObject);
			if (!col.gameObject.GetComponent<CheckForMerge> ().isFalling ()) {
				falling = false;
				canTriggerGameOver = true;
			}
		} else if (col.gameObject.name.Contains ("Wall") && GetComponent<Rigidbody> ().velocity.magnitude < 0.2) {
			falling = false;
			canTriggerGameOver = true;
		}

	} 

	private int checkForBlockOverlapCount;
	void OnCollisionStay(Collision col){
		if (col.gameObject.GetComponent<CheckForMerge> () && Vector3.Magnitude(transform.position - col.transform.position - Physics.gravity/Vector3.Magnitude(Physics.gravity)) < 0.1){//(AbsPositionDiff.x < GravityDirection.x) && (AbsPositionDiff.y < GravityDirection.y) && (AbsPositionDiff.z < GravityDirection.z)) {

			merge (col.gameObject.GetComponent<CheckForMerge> ().gameObject);
			if (!col.gameObject.GetComponent<CheckForMerge>().isFalling())
				falling = false;

		}

		if (checkForBlockOverlapCount < 20) {
			if (Vector3.Magnitude(col.gameObject.transform.position - transform.position) < 0.5f && this.stacks < col.gameObject.GetComponent<CheckForMerge> ().stacks)
				Destroy (this.gameObject);
			checkForBlockOverlapCount++;
		}
	}

	void OnCollisionExit(Collision col){
		if (GetComponent<Rigidbody>().velocity.magnitude > 0)
			falling = true;
	}
		

	void OnGUI(){
		//GUI.TextArea (new Rect (transform.position, new Vector2 (2, 2)), stacks.ToString ());
		if (stacks > 1 && !GameOver.paused) {
			GUIStyle textStyle = new GUIStyle ();
			textStyle.alignment = TextAnchor.UpperLeft;
			textStyle.fontSize = 26;

			GUIStyle borderStyle = new GUIStyle ();
			borderStyle.alignment = TextAnchor.UpperLeft;
			borderStyle.fontSize = 26;
			borderStyle.normal.textColor = new UnityEngine.Color (1f, 1f, 1f, 1f);

			Vector2 position = Camera.main.WorldToScreenPoint (transform.position);
			position = new Vector2 (position.x, Screen.height - position.y);

			GUI.TextField(new Rect (position + new Vector2(1,1), new Vector2 (100, 100)), stacks.ToString(), 1, borderStyle);
			GUI.TextField(new Rect (position + new Vector2(-1,-1), new Vector2 (100, 100)), stacks.ToString(), 1, borderStyle);
			GUI.TextField(new Rect (position + new Vector2(-1,1), new Vector2 (100, 100)), stacks.ToString(), 1, borderStyle);
			GUI.TextField(new Rect (position + new Vector2(1,-1), new Vector2 (100, 100)), stacks.ToString(), 1, borderStyle);
			GUI.TextField(new Rect (position, new Vector2 (100, 100)), stacks.ToString(), 1, textStyle);


		}
	}

	public void merge(GameObject other){
		if (((this.color == Color.Orange || this.color == Color.Green || this.color == Color.Purple) && !ScoreManager.ComplementaryCanMerge()))
			return;
		if (other.name.Equals(name)){
			stacks+=other.gameObject.GetComponent<CheckForMerge>().stacks;
			Destroy (other);
			if (stacks >= StacksForPoints) {
				if (this.color == Color.Blue || this.color == Color.Red || this.color == Color.Yellow)
					ScoreManager.GrantPoints (ScoreManager.BlockScoreType.Primary, StacksForPoints);
				else
					ScoreManager.GrantPoints (ScoreManager.BlockScoreType.Complementary, StacksForPoints);
				ParticleSystem p = ((GameObject)Instantiate (Resources.Load ("Sparkles"), transform.position + new Vector3(0f,0f,-1f), Quaternion.identity)).GetComponent<ParticleSystem> ();
				p.startColor = GetColor (color);
				Destroy (this.gameObject);
				Destroy (other.gameObject);
			} else
			return;
		}
		//At this point, only complementary merges possible, want to make sure we can do that
		if (!ScoreManager.ComplementaryCanMerge ())
			return;
		GameObject NewBlock;
		switch (name) {
		case "BlueBlock(Clone)":
			if (other.gameObject.name.Contains ("Yellow")) {
				NewBlock = (GameObject) Instantiate (Resources.Load ("GreenBlock"), transform.position, Quaternion.identity);
				((GameObject)Instantiate (Resources.Load ("Cloud"), transform.position + new Vector3(0f,0f,-1f), Quaternion.identity)).GetComponent<ParticleSystem> ().startColor = GetColor(Color.Green);
			} else if (other.gameObject.name.Contains ("Red")) {
				NewBlock = (GameObject) Instantiate (Resources.Load ("PurpleBlock"), transform.position, Quaternion.identity);
				((GameObject)Instantiate (Resources.Load ("Cloud"), transform.position + new Vector3(0f,0f,-1f), Quaternion.identity)).GetComponent<ParticleSystem> ().startColor = GetColor(Color.Purple);
			} else {
				return;
			}
			break;
		case "YellowBlock(Clone)":
			if (other.gameObject.name.Contains ("Blue")) {
				NewBlock = (GameObject) Instantiate (Resources.Load ("GreenBlock"), transform.position, Quaternion.identity);
				((GameObject)Instantiate (Resources.Load ("Cloud"), transform.position + new Vector3(0f,0f,-1f), Quaternion.identity)).GetComponent<ParticleSystem> ().startColor = GetColor(Color.Green);
			} else if (other.gameObject.name.Contains ("Red")) {
				NewBlock = (GameObject) Instantiate (Resources.Load ("OrangeBlock"), transform.position, Quaternion.identity);
				((GameObject)Instantiate (Resources.Load ("Cloud"), transform.position + new Vector3(0f,0f,-1f), Quaternion.identity)).GetComponent<ParticleSystem> ().startColor = GetColor(Color.Orange);

			} else {
				return;
			}
			break;
		case "RedBlock(Clone)":
			if (other.gameObject.name.Contains ("Blue")) {
				NewBlock = (GameObject) Instantiate (Resources.Load ("PurpleBlock"), transform.position, Quaternion.identity);
				((GameObject)Instantiate (Resources.Load ("Cloud"), transform.position + new Vector3(0f,0f,-1f), Quaternion.identity)).GetComponent<ParticleSystem> ().startColor = GetColor(Color.Purple);
			} else if (other.gameObject.name.Contains ("Yellow")) {
				NewBlock = (GameObject) Instantiate (Resources.Load ("OrangeBlock"), transform.position, Quaternion.identity);
				((GameObject)Instantiate (Resources.Load ("Cloud"), transform.position + new Vector3(0f,0f,-1f), Quaternion.identity)).GetComponent<ParticleSystem> ().startColor = GetColor(Color.Orange);
			} else {
				return;
			}

			break;
		default:
			return;
		}
		if (this.stacks < other.gameObject.GetComponent<CheckForMerge> ().stacks)
			NewBlock.GetComponent<CheckForMerge> ().setStacks(this.stacks);
		else
			NewBlock.GetComponent<CheckForMerge> ().setStacks(other.gameObject.GetComponent<CheckForMerge> ().stacks);
		Destroy (this.gameObject);
		Destroy (other.gameObject);

	}

	public void AdjustConstraints(char axis){
		if (axis == 'x') {
			GetComponent<Rigidbody> ().constraints = RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePositionZ;
			//transform.position = new Vector3 (Mathf.Round (transform.position.x), transform.position.y, transform.position.z);
		} else if (axis == 'y') {
			GetComponent<Rigidbody> ().constraints = RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePositionZ;
			//transform.position = new Vector3 (transform.position.x, Mathf.Round(transform.position.y), transform.position.z);
		}
		transform.position = new Vector3 (Mathf.Round(transform.position.x), Mathf.Round(transform.position.y), transform.position.z);
		checkForBlockOverlapCount = 0;
	}

	public void setStacks(int stacks){
		this.stacks = stacks;
	}
		

	UnityEngine.Color GetColor(Color thisColor){
		if (thisColor == Color.Red)
			return new UnityEngine.Color (0.8f, 0.1f, 0.1f);
		else if (thisColor == Color.Yellow)
			return new UnityEngine.Color (0.8f, 0.8f, 0.1f);
		else if (thisColor == Color.Blue)
			return new UnityEngine.Color (0.2f, 0.2f, 0.7f);
		else if (thisColor == Color.Orange)
			return new UnityEngine.Color (0.8f, 0.5f, 0.1f);
		else if (thisColor == Color.Green)
			return new UnityEngine.Color (0.2f, 0.8f, 0.2f);
		else
			return new UnityEngine.Color (0.8f, 0.1f, 0.8f);
	}

	public bool isFalling(){
		return falling;
	}

	public void setGameOver(){
		canTriggerGameOver = true;
	}

	public bool blockTriggersGameOver(){
		return canTriggerGameOver;
	}

}
