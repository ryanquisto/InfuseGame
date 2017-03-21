using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Messenger : MonoBehaviour {
    private static bool ShowMessage;
    private static string Message;
    private static float TextRiseAmount;
	private static Queue<string> WaitingMessages;
	// Use this for initialization
	void Start () {
		WaitingMessages = new Queue<string> ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public static new void SendMessage(string Text)
    {
		if (Message == null) {
			TextRiseAmount = 0;
			ShowMessage = true;
			Message = Text;
		} else {
			WaitingMessages.Enqueue (Text);
		}
    }

    void OnGUI()
    {
        if (ShowMessage)
        {
            TextRiseAmount += Time.deltaTime * 10;
            GUIStyle style = new GUIStyle();
            style.fontSize = 20;
            style.wordWrap = true;
            style.alignment = TextAnchor.UpperCenter;
            style.normal.textColor = new Color(0, 0, 0, 1 - TextRiseAmount / 70);
            //Sloppy way to make a text border
            GUI.Label(new Rect(Screen.width / 2 - 300 + 1, Screen.height / 2.5f - TextRiseAmount, 600, 200), Message, style);
            GUI.Label(new Rect(Screen.width / 2 - 300 - 1, Screen.height / 2.5f - TextRiseAmount, 600, 200), Message, style);
            GUI.Label(new Rect(Screen.width / 2 - 300, Screen.height / 2.5f + 1 - TextRiseAmount, 600, 200), Message, style);
            GUI.Label(new Rect(Screen.width / 2 - 300, Screen.height / 2.5f - 1 - TextRiseAmount, 600, 200), Message, style);
            style.normal.textColor = new Color(1, 1, 1, 1 - TextRiseAmount / 70);
            GUI.Label(new Rect(Screen.width / 2 - 300, Screen.height / 2.5f - TextRiseAmount, 600, 200), Message, style);
            if (TextRiseAmount > 70)
            {
                TextRiseAmount = 0;
				if (WaitingMessages.Count == 0) {
					ShowMessage = false;
					Message = null;
				} else {
					Message = WaitingMessages.Dequeue ();
				}
            }
        }
    }
}
