using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class Letter : MonoBehaviour {
	private Rigidbody2D rb;
	private float speed, posX, posY;
	private char letter;
	private Text text, butt;
	private bool deleteFlag;
	Vector3 dist;
	#region Unity scene setting
	[SerializeField] private float[] probability;
	#endregion
	private char[] letters;
	private Main main;

	int Choose(float[] probability)	{
		float RandomPoint = UnityEngine.Random.value;
		for (int i = 0; i < probability.Length; i++)
			if (RandomPoint < probability[i])
				return i;
			else
				RandomPoint -= probability[i];
		return probability.Length - 1;
	}

	void Start () {
		main = GameObject.Find ("Main Camera").GetComponent<Main> ();
		letters = new char[] {'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z'};
		deleteFlag = false;
		rb = GetComponent<Rigidbody2D> ();
		speed = UnityEngine.Random.Range (-1.5f, -4f);
		rb.velocity = new Vector2 (0, speed);
		text = GetComponentInChildren<Text> ();
		letter = letters [Choose (probability)];
		text.text = char.ToString (letter);
		butt = GameObject.Find ("Text").GetComponent<Text> ();
	}

	void Update () {
		if (transform.position.y <= -7)
			Destroy (gameObject);
		if (Input.GetKeyDown(KeyCode.Mouse0) && deleteFlag) {
			main.currentWord += letter;
			main.lastLetterIndex = main.currentWord.Length - 1;
			int index = main.nounList.BinarySearch (main.currentWord);
			Debug.Log (index);
			if (main.currentWord.Length > 1 && index >= 0) {
				main.valid = true;
				butt.text = "v";
			} else {
				main.valid = false;
				butt.text = "x";
			}
			Destroy (gameObject);
		}
	}

	void OnMouseEnter() {
		deleteFlag = true;
	}

	void OnMouseExit() {
		deleteFlag = false;
	}
}
