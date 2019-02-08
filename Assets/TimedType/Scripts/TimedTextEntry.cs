using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimedTextEntry : MonoBehaviour {
	//Drags
	public Text targetText;

	//Editor Config
	[TextArea] public string[] printStrings;
	public float letterDelay = 0.1f;
	public float maxRandomDelay = 0.05f;
	public bool clearBetweenEntries = false;
	public bool printOnStart = true;
	
	[TextArea] public string continueMessage = "Press Space or Click to Continue...";
	public UnityEngine.KeyCode[] continueKeyCodes;

	//Bookkeeping
	Coroutine typingCoroutine;
	int entryIdx;
	string nextStr;
	bool noDelay;
	bool finishedPrinting;

	//Debug
	[SerializeField] private bool debugTimedText;

	// Use this for initialization
	void Start () {
		targetText.text = "";

		if (printOnStart)
			NextSequence ();
	}

	//Use this to set as a UI element action, such as button press.
	public void BtnPressNextSequence () {
		NextSequence ();
	}

	//Check for accelerated output via user input.
	void Update () {
		if (finishedPrinting)
			return;

		//If no existing continue inputs, keep moving.
		if (typingCoroutine == null &&
			(continueKeyCodes.Length == 0 || continueKeyCodes[0] == KeyCode.None)) {
			NextSequence ();
			return;
		}

		//Check for continue key code input.
		for (int i = 0; i < continueKeyCodes.Length; i++) {
			if (Input.GetKeyDown (continueKeyCodes[i])) {
				NextSequence ();
				return;
			}
		}
	}

	//Move to the next sequence depending on state.
	void NextSequence () {
		if (typingCoroutine == null) {
			NextEntry ();
		} else {
			noDelay = true;
		}
	}

	//If valid, start the coroutine to type a new entry.
	void NextEntry () {
		if (entryIdx >= printStrings.Length) {
			if (debugTimedText)
				print ("No more strings to display. Do something else now.");

			finishedPrinting = true;

			//DoSomethingElseEntirelyHere();

			return;
		}

		if (clearBetweenEntries) {
			targetText.text = "";
			nextStr = "";
		}

		typingCoroutine = StartCoroutine (TypeNextEntry ());
	}

	//Type out a timed entry and push it to the text field.
	IEnumerator TypeNextEntry () {
		char[] charArray = printStrings[entryIdx].ToCharArray ();

		if (entryIdx > 0)
			nextStr += "\r\n\r\n";

		//Append to text field.
		for (int i = 0; i < charArray.Length; i++) {
			float nextDelay = GetDelay ();

			//Check for user acceleration.
			if (noDelay) {
				for (int j = i; j < charArray.Length; j++)
					nextStr += charArray[j];

				targetText.text = nextStr;

				EndEntry ();

				yield break;
			}

			//Continue with normal timed routine.
			nextStr += charArray[i];
			targetText.text = nextStr;

			yield return new WaitForSeconds (nextDelay);
		}

		EndEntry ();
	}

	//Indicate an entry is finished.
	void EndEntry () {
		entryIdx++;

		if (!string.IsNullOrEmpty (continueMessage)) {
			targetText.text += "\r\n\r\n";
			targetText.text += continueMessage;
		}

		noDelay = false;
		typingCoroutine = null;
	}

	//Calculate the delay before entry of next character.
	//Clear Code > Clever Code, lol.
	float GetDelay () {
		float fullDelay = letterDelay;
		if (maxRandomDelay != 0f)
			fullDelay += UnityEngine.Random.Range (-maxRandomDelay, maxRandomDelay);

		if (fullDelay < 0f)
			fullDelay = 0f;

		return fullDelay;
	}
}