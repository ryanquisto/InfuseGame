using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour {
	private static int Score;
	private static Rule[] rules = {
		new Rule("Blocks dropping 50% more quickly!", 25, Rule.RuleType.DropSpeed, 1.5f),
		new Rule("Blocks dropping 100% more quickly!", 50, Rule.RuleType.DropSpeed, 2f),
		new Rule("You must infuse at least 10% primary colors!", 100, Rule.RuleType.PrimaryRule, .1f),
		new Rule("Blocks to Infuse: 5", 120, Rule.RuleType.MergeNum, 5),
		new Rule("Blocks dropping 150% more quickly!", 150, Rule.RuleType.DropSpeed, 2.5f),
		new Rule("Blocks to Infuse: 6", 180, Rule.RuleType.MergeNum, 6),
		new Rule("Complementary merges worth 0 points.", 200, Rule.RuleType.PrimaryRule2, 1),
		new Rule("Blocks dropping 200% more quickly!", 220, Rule.RuleType.DropSpeed, 2.5f),
		new Rule("Blocks to Infuse: 4", 230, Rule.RuleType.MergeNum, 4),
		new Rule("Blocks dropping 250% more quickly!", 350, Rule.RuleType.DropSpeed, 3.5f),
		new Rule("Blocks to Infuse: 5", 400, Rule.RuleType.MergeNum, 5),
		new Rule("You must infuse at least 20% primary colors!", 450, Rule.RuleType.PrimaryRule, .2f),
		new Rule("Blocks to Infuse: 6", 500, Rule.RuleType.MergeNum, 6),
		new Rule("Must score points before timer runs out!", 550, Rule.RuleType.Other, 0),
		new Rule("Complementary merges worth 0 points.", 600, Rule.RuleType.PrimaryRule2, 1),
		new Rule("Blocks to Infuse: 7", 800, Rule.RuleType.MergeNum, 7),
		new Rule("Blocks to Infuse: 8", 1000, Rule.RuleType.MergeNum, 8),
		new Rule("You must infuse at least 30% primary colors!", 1200, Rule.RuleType.PrimaryRule, .3f),
		new Rule("Blocks dropping 300% more quickly!", 1500, Rule.RuleType.DropSpeed, 4f),
		new Rule("Complementary merges worth 0 points.", 1800, Rule.RuleType.PrimaryRule2, 1),
		new Rule("Blocks to Infuse: 6", 1820, Rule.RuleType.MergeNum, 6),
		new Rule("Um.. are you human? How did you make it this far?", 2000, Rule.RuleType.Other, 1),
		new Rule("Time for you to lose. Blocks Dropping 1000% more quickly.", 2000, Rule.RuleType.DropSpeed, 11f),
		new Rule("Time for you to lose. Blocks to infuse: 100", 2000, Rule.RuleType.MergeNum, 100),
		new Rule("Time for you to lose. Must infuse 100% primary colors.", 2000, Rule.RuleType.PrimaryRule, 1f)
	};
	private static int NextRuleIndex;
	public static bool PrimaryRule;
	private static float PrimaryRuleRequirement;
	public static bool PrimaryRule2;
	private static int PrimaryMerges;
	private static int ComplementaryMerges;
	//0 is drop speed, 1 is merge count, 2 is primary rule
	private static string[] RuleDescription = new string[3];
	private static bool TimerActive;
	private static bool ReactivateClockOnResume;

	public enum BlockScoreType {
		Primary,
		Complementary,
	};
	private const int PRIMARY_VALUE = 2;
	private const int COMPLEMENTARY_VALUE = 1;
	private static Text ScoreText;
	private static Text RuleText;
	private static int TimeToDeath;
	private static float TimeLeft;
	private static GameObject Clock;
	private static Text TimeText;
	private static bool TextFlashing;
	public static float dropRate;

	// Use this for initialization
	void Start () {
		Messenger.SendMessage ("Use the arrow keys to change directions");
		Reset ();
	}

	private bool GoingWhite;
	// Update is called once per frame
	void Update () {
		if (TimerActive) {
			TimeLeft -= Time.deltaTime;
			TimeText.text = ((int)TimeLeft).ToString();
			if (TimeLeft < 10 && !TextFlashing)
				TextFlashing = true;
			if (TimeLeft <= 0) {
				Clock.SetActive (false);
				GameOver.DoGameOver ();
			}

			if (TextFlashing) {
				Color textColor = TimeText.color;
				if (!GoingWhite) {
					textColor = new Color (1, textColor.g - Time.deltaTime, textColor.b - Time.deltaTime);
					TimeText.gameObject.transform.localScale += new Vector3 (Time.deltaTime * .7f, Time.deltaTime * .7f, 1);
					if (textColor.b < .01f)
						GoingWhite = true;
				} else {
					textColor = new Color (1, textColor.g + Time.deltaTime, textColor.b + Time.deltaTime);
					TimeText.gameObject.transform.localScale -= new Vector3 (Time.deltaTime * .7f, Time.deltaTime * .7f, 1);
					if (textColor.b > .99f)
						GoingWhite = false;
				}
				TimeText.color = textColor;
			}
		}
		//Cheat - just for fun. Press f12 to get to insanely hard part
		if (Input.GetKeyUp(KeyCode.F12)){
			//NextRuleIndex = 21;
			Clock.SetActive (true);
			TimerActive = true;
			BonusPoints (2000, "being a fucking cheater");
		}
	}

	private static void ResetText(){
		TimeText.color = Color.white;
		TimeText.gameObject.transform.localScale = new Vector3 (1, 1, 1);
	}

	void OnGUI(){
		RuleText.text = "";
		foreach (string r in RuleDescription) {
			if (r != null && r.Length > 0) {
				RuleText.text += "\n" + r + "\n";
			}
		}
		if (PrimaryRule)
			RuleText.text += "\tPercent Primary Merges: " + (PrimaryPercent()*100).ToString("#.##");



	}

	public static void GrantPoints(BlockScoreType b, int stacks){
		if (b == BlockScoreType.Complementary) {
			if (!PrimaryRule2) {
				Score += COMPLEMENTARY_VALUE * stacks;
				TimeLeft = TimeToDeath;
				TextFlashing = false;
				ResetText ();
			}
			if (PrimaryRule)
				ComplementaryMerges++;
		}
		else if (b == BlockScoreType.Primary) {
			//If in primary rule mode, no points granted for primary infusion
			if (ComplementaryCanMerge ()) {
				Score += PRIMARY_VALUE * stacks;
				TimeLeft = TimeToDeath;
				TextFlashing = false;
				ResetText ();
			}
			if (PrimaryRule)
				PrimaryMerges++;
		}
		ScoreText.text = "Score: " + Score.ToString ();
		if (NextRuleIndex < rules.Length)
			CheckForNewRule ();
	}

	private static int bonus;
	private static string BonusReason;
	private static void BonusPoints(int points, string reason){
		Score += points;
		bonus = points;
		BonusReason = reason;
		ScoreText.text = "Score: " + Score.ToString ();
		GameObject.FindObjectOfType<ScoreManager> ().Invoke ("BonusMessage", 1);
	}

	private static void CheckForNewRule(){
		if (NextRuleIndex < rules.Length && Score >= rules [NextRuleIndex].GetPoints ()) {
			rules [NextRuleIndex].Activate ();
			Messenger.SendMessage ("New Rule! " + rules [NextRuleIndex].GetDescription ());
			switch (rules[NextRuleIndex].getType()) {
			case Rule.RuleType.Other:
				if (rules [NextRuleIndex].getValue () == 0) {
					TimerActive = true;
					Clock.SetActive (true);
				} else if (rules [NextRuleIndex].getValue () == 1) {
					BonusPoints (1000, "being way too good at this game");
				}
				break;
			case Rule.RuleType.DropSpeed:
				dropRate = rules [NextRuleIndex].getValue ();
				BlockGenerator.MultiplyDropRate (rules[NextRuleIndex].getValue());
				RuleDescription [0] = rules [NextRuleIndex].GetDescription ();
				break;
			case Rule.RuleType.PrimaryRule:
				PrimaryRuleRequirement = rules [NextRuleIndex].getValue ();
				PrimaryRule = true;
				PrimaryRule2 = false;
				RuleDescription[2] = rules [NextRuleIndex].GetDescription ();
				break;
			case Rule.RuleType.PrimaryRule2:
				if (PrimaryPercent () - PrimaryRuleRequirement > 0.01) {
					bonus = (int)Mathf.Pow ((float)(100*(PrimaryPercent () - PrimaryRuleRequirement)), 1f + 3f * PrimaryRuleRequirement);
					BonusPoints (bonus, "exceeding rule expectations");
				}
				PrimaryRule2 = true;
				PrimaryRule = false;
				RuleDescription[2] = rules [NextRuleIndex].GetDescription ();
				break;
			case Rule.RuleType.MergeNum:
				CheckForMerge.StacksForPoints = (int)rules [NextRuleIndex].getValue ();
				RuleDescription [1] = rules [NextRuleIndex].GetDescription ();
				TimeToDeath = CheckForMerge.StacksForPoints * 10;
				break;
			}

			NextRuleIndex++;
			//If points sufficient for a rule, check to see if sufficient for next rule until it isn't
			CheckForNewRule();
		}
	}
		

	public void BonusMessage(){
		Messenger.SendMessage ("You earned " + bonus.ToString () + " points for " + BonusReason + "!");
		CheckForNewRule ();
	}

	public static bool ComplementaryCanMerge(){
		if (!PrimaryRule)
			return true;
		return ((double)PrimaryMerges / ((double)(PrimaryMerges + ComplementaryMerges + 1)) * 100) >= PrimaryRuleRequirement * 100;
	}

	private static double PrimaryPercent(){
		return ((double)PrimaryMerges / ((double)(PrimaryMerges + ComplementaryMerges)));
	}

	public static void Reset(){
		ScoreText = GameObject.Find ("Score").gameObject.GetComponent<Text> ();
		RuleText = GameObject.Find ("Rules").gameObject.GetComponent<Text> ();
		RuleText.text = "";
		NextRuleIndex = 0;
		PrimaryRule = false;
		PrimaryRule2 = false;
		RuleDescription [0] = "Blocks dropping at normal rate";
		RuleDescription [1] = "Blocks to Infuse: 4";
		RuleDescription [2] = "";
		PrimaryMerges = 1;
		ComplementaryMerges = 1;
		TimeToDeath = 40;
		TimeLeft = TimeToDeath;
		dropRate = BlockGenerator.InitialDropRate;
		ReactivateClockOnResume = false;
		if (Clock == null) {
			Clock = GameObject.Find ("Clock");
			TimeText = Clock.transform.Find ("TimeRemaining").GetComponent<Text> ();
		}
		Clock.SetActive (false);
		Score = 0;
		TimerActive = false;
		if (GameOver.paused)
			GameOver.ResumeGame ();
	}

	public static int score(){
		return Score;
	}

	public static void pauseClock(){
		if (TimerActive) {
			TimerActive = false;
			ReactivateClockOnResume = true;
		}
	}

	public static void resumeClock(){
		if (ReactivateClockOnResume)
			TimerActive = true;
	}

}
