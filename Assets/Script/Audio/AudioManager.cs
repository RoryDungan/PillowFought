using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using Zenject;

namespace ElMoro {
	public interface IAudioManager {
		/// <summary>
		/// Play audio without position input (for 2D audio)
		/// </summary>
		/// <param name="audioName">Name of audio clip to be played</param>
		void Play(string audioName);

		/// <summary>
		/// Play audio with respect to position (for 3D audio)
		/// </summary>
		/// <param name="audioName">Name of audio clip to be played</param>
		/// <param name="playPosition">Position audio to be played from</param>
		void Play(string audioName, Vector3 playPosition);

		/// <summary>
		/// Stop currently playing audio (for music)
		/// </summary>
		/// <param name="audioName">Name of audio clip to be stopped</param>
		void Stop(string audioName);
	}

	public class AudioManager : MonoBehaviour, IAudioManager {
		private Vector2 audioDistanceRange = new Vector2(15.0f, 35.0f);

		private Dictionary<string, AudioData> audioLibrary;

		// public AudioManager(IAudioSettings settings) {
		// 	CreateAudioSources(settings);
		// }
		
		[Inject]
		private IAudioSettings settings;

		public void Start() {
			CreateAudioSources(settings);
		}

		[Inject]
		IAudioManager instance;

		private void Awake() {
			if ((object)instance != null && (object)instance != this) {
				Destroy(this.gameObject);
			}

			DontDestroyOnLoad(gameObject);
		}

		private void CreateAudioSources(IAudioSettings settings) {
			Debug.Log("reate");
			AudioData[] audioData = settings.Audio;
			audioLibrary = new Dictionary<string, AudioData>();

			for (int audioIndex = 0; audioIndex < audioData.Length; audioIndex++) {
				GameObject newObj = new GameObject("AudioSource_" + audioData[audioIndex].name);
				newObj.transform.parent = transform;

				AudioSource newSource = newObj.AddComponent<AudioSource>();
				newSource.outputAudioMixerGroup = audioData[audioIndex].mixerGroup;
				newSource.clip = audioData[audioIndex].audioClip;
				newSource.spatialBlend = audioData[audioIndex].spatialBlend;
				newSource.loop = audioData[audioIndex].isLooping;

				newSource.rolloffMode = AudioRolloffMode.Linear;
				newSource.minDistance = audioDistanceRange.x;
				newSource.maxDistance = audioDistanceRange.y;

				audioData[audioIndex].source = newSource;

				Debug.Log(audioData[audioIndex].name);
				audioLibrary.Add(audioData[audioIndex].name, audioData[audioIndex]);
				Debug.Log(audioLibrary.Count);
			}
		}

		public void Play(string audioName) {
			Play(audioName, Vector3.zero);
		}

		public void Play(string audioName, Vector3 playPosition) {
			if (!audioLibrary.ContainsKey(audioName)) {
				Debug.Log(gameObject.name + ": Requested audio clip does not exist.");
			}

			AudioData curData = audioLibrary[audioName];
			AudioSource curSource = curData.source;
			curSource.volume = Random.Range(curData.volRange.x, curData.volRange.y);
			curSource.pitch = Random.Range(curData.pitchRange.x, curData.pitchRange.y);

			if (curSource.transform.position != playPosition) {
				curSource.transform.position = playPosition;
			}

			if (!curData.isLooping) {
				curSource.PlayOneShot(curSource.clip, curSource.volume);
			} else if (!curSource.isPlaying) {
				curSource.Play();
			}
		}

		public void Stop(string audioName) {
			if (!audioLibrary.ContainsKey(audioName)) {
				Debug.Log(gameObject.name + ": Requested audio clip does not exist.");
			}

			AudioData curData = audioLibrary[audioName];
			AudioSource curSource = curData.source;
			
			curSource.Stop();
		}
	}
}
