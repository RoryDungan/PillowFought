using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TEST_AudioPlayer : MonoBehaviour {
	public string testString = "Hit";

	private void Update() {
		if (Input.GetKeyDown(KeyCode.J)) {
			GetComponent<AudioManager>().Play(testString, transform.position);
		}
		if (Input.GetKeyDown(KeyCode.M)) {
			GetComponent<AudioManager>().Play("BGM");
		}
		if (Input.GetKeyDown(KeyCode.N)) {
			GetComponent<AudioManager>().Stop("BGM");
		}
	}
}
