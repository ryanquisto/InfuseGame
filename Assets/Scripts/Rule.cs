public class Rule {
	private string description;
	private int points;
	private bool active;
	public enum RuleType{
		DropSpeed,
		MergeNum,
		PrimaryRule,
		PrimaryRule2,
		Other
	};
	private RuleType type;
	private float number;
	public Rule(string description, int points, RuleType type, float number){
		this.description = description;
		this.points = points;
		this.active = false;
		this.type = type;
		this.number = number;
	}
	public string GetDescription(){
		return description;
	}
	public int GetPoints(){
		return points;
	}

	public void Activate(){
		active = true;
	}

	public bool IsActive(){
		return active;
	}

	public float getValue (){
		return number;
	}

	public RuleType getType(){
		return type;
	}
}
