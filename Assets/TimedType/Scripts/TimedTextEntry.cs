using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimedTextEntry : MonoBehaviour {
	public Text targetText;
	[TextArea] public string[] printStrings;
	public float letterDelay = 0.1f;
	public float maxRandomDelay = 0.05f;
	public bool clearBetweenEntries = false;
	public bool printOnStart = true;
	public UnityEngine.KeyCode[] continueKeyCodes;

	int entryIdx;
	string nextStr;
	bool noDelay;
	Coroutine typingCoroutine;

	[SerializeField] private bool debugTimedText;
	private bool finishedPrinting;

	// Use this for initialization
	void Start () {
		targetText.text = "";

		if (printOnStart)
			typingCoroutine = StartCoroutine (EnterNextString ());
	}

	//Use this to set as a UI element action.
	public void BtnPressNextSequence () {
		PrintNextSequence ();
	}

	//Use this if you want accelerated output via keyboard or mouse or something.
	void Update () {
		if (finishedPrinting)
			return;

		if (typingCoroutine == null &&
			(continueKeyCodes.Length == 0 || continueKeyCodes[0] == KeyCode.None)) {

			PrintNextSequence ();

			return;
		}

		for (int i = 0; i < continueKeyCodes.Length; i++) {
			if (Input.GetKeyDown (continueKeyCodes[i]))
				PrintNextSequence ();
		}
	}

	void PrintNextSequence () {
		if (typingCoroutine == null) {
			typingCoroutine = StartCoroutine (EnterNextString ());
		} else {
			noDelay = true;
		}
	}

	IEnumerator EnterNextString () {
		if (entryIdx >= printStrings.Length) {
			if (debugTimedText) {
				targetText.text = "No more stuff to display. Exit the sequence here.";
				print ("No more strings to display. Do something else now.");
			}

			finishedPrinting = true;

			yield break;
		}

		if (clearBetweenEntries) {
			targetText.text = "";
			nextStr = "";
		}

		char[] charArray = printStrings[entryIdx].ToCharArray ();

		for (int i = 0; i < charArray.Length; i++) {
			float nextDelay = GetDelay ();

			if (nextDelay == -1f) {
				//Finish printing to text immediately.
				for (int j = i; j < charArray.Length; j++)
					nextStr += charArray[j];

				targetText.text = nextStr;
				EndStringEntry ();

				yield break;
			}
			nextStr += charArray[i];
			targetText.text = nextStr;

			yield return new WaitForSeconds (nextDelay);
		}
		EndStringEntry ();
	}

	private void EndStringEntry () {
		entryIdx++;
		noDelay = false;
		typingCoroutine = null;
	}

	private float GetDelay () {
		if (noDelay)
			return -1f;

		float fullDelay = letterDelay;
		if (maxRandomDelay != 0f)
			fullDelay += UnityEngine.Random.Range (-maxRandomDelay, maxRandomDelay);

		if (fullDelay < 0f)
			fullDelay = 0f;

		return fullDelay;
	}
}