using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace ElMoro {
	public class TEST_AudioPlayer : MonoBehaviour {
		public string testString = "Hit";

		[Inject]
		IAudioManager audioManager;

		private void Update() {
			if (Input.GetKeyDown(KeyCode.J)) {
				audioManager.Play(testString, transform.position);
			}
			if (Input.GetKeyDown(KeyCode.M)) {
				audioManager.Play("BGM");
			}
			if (Input.GetKeyDown(KeyCode.N)) {
				audioManager.Stop("BGM");
			}
		}
	}
}