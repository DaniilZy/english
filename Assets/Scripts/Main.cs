﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class Main : MonoBehaviour {
	public GameObject letter;
	private Vector3 position;
	private float spawnTime, timer, delay;
	private Text text, butt, score;
	public string currentWord;
	public List<string> nounList;
	public bool valid;
	public int lastLetterIndex;
	private bool isKeyUp;


	private string CharSwap (string source, int index, int direction) {
		if (index + direction < 0 || index + direction >= source.Length)
			return source;
		lastLetterIndex += direction;
		char[] chars = source.ToCharArray ();

		char tmp = chars [index + direction];
		chars [index + direction] = chars [index];
		chars [index] = tmp;

		return new string (chars);
	} 

	void Start () {
		butt = GameObject.Find ("Text").GetComponent<Text> ();
		score = GameObject.Find ("Score").GetComponent<Text> ();
		string[] nouns = File.ReadAllLines("Assets/Data/nounlist.txt");
		nounList = nouns.ToList ();
		nounList.Sort ();
		currentWord = "";
		score.text = "15";
		delay = 0;
		spawnTime = 0;
		timer = 0;
		valid = false;
		butt.text = "f";
		lastLetterIndex = -1;
		isKeyUp = true;
	}

	void Update () {
		if (Input.GetKeyDown (KeyCode.Return)) {
			if (valid) {
				score.text = (Convert.ToInt32(score.text) + currentWord.Length).ToString ();
				butt.text = "x";
				currentWord = "";
			}
		}
		if (Input.GetKeyDown (KeyCode.RightControl)) {
			if (currentWord.Length > 0)
				score.text = (Convert.ToInt32(score.text) - 2).ToString ();
			currentWord = "";
		}
		if (Input.GetKeyDown (KeyCode.LeftArrow) && isKeyUp) {
			currentWord = CharSwap (currentWord, lastLetterIndex, -1);
			int index = nounList.BinarySearch (currentWord);
			Debug.Log (index);
			if (currentWord.Length > 1 && index >= 0) {
				valid = true;
				butt.text = "v";
			} else {
				valid = false;
				butt.text = "x";
			}
			isKeyUp = false;
		} else if (Input.GetKeyDown (KeyCode.RightArrow) && isKeyUp) {
			currentWord = CharSwap (currentWord, lastLetterIndex, 1);
			int index = nounList.BinarySearch (currentWord);
			Debug.Log (index);
			if (currentWord.Length > 1 && index >= 0) {
				valid = true;
				butt.text = "v";
			} else {
				valid = false;
				butt.text = "x";
			}
			isKeyUp = false;
		}
		if (Input.GetKeyUp (KeyCode.LeftArrow) || Input.GetKeyUp (KeyCode.RightArrow))
			isKeyUp = true;
		text = GetComponentInChildren<Text> ();
		if (text.text != currentWord) {
			text.text = currentWord;

		}
		spawnTime += Time.deltaTime;
		timer += Time.deltaTime;
		position = new Vector3 (UnityEngine.Random.Range(-2.4f, 2.4f), 7, 0);
		if (spawnTime >= delay) {
			Instantiate (letter, position, Quaternion.identity);
			spawnTime = 0;
			delay = UnityEngine.Random.Range (0f, 1f);
		}
		if (timer > 8f) {
			timer = 0;
			score.text = (Convert.ToInt32(score.text) - 1).ToString ();
		}
	}

	public void SubmitButtonClick () {
		if (valid) {
			score.text = (Convert.ToInt32(score.text) + currentWord.Length).ToString ();
			butt.text = "f";
			currentWord = "";
		}
	}

}
