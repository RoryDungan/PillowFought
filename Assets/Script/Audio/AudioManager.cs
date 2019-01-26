using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour {
	public static AudioManager instance;

	[Header("Audio Settings")]
	public Vector2 audioDistanceRange = new Vector2(15.0f, 35.0f);

	[Header("AudioData References")]
	public AudioData[] audioData;

	private Dictionary<string, AudioData> audioLibrary;

	private void Awake() {
		if (instance != null && instance != this) {
			Destroy(instance.gameObject);
		}
		instance = this;
	}

	private void Start() {
		CreateAudioSources();
	}

	private void CreateAudioSources() {
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

[System.Serializable]
public class AudioData {
	public string name;
	public AudioClip audioClip;
	public AudioMixerGroup mixerGroup;
	[HideInInspector] public AudioSource source;
	public Vector2 volRange;
	public Vector2 pitchRange;
	[Range(0.0f, 1.0f)] public float spatialBlend;
	public bool isLooping;
}