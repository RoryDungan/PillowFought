using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

namespace ElMoro {
	public interface IAudioSettings {
		/// <summary>
		/// Audio source roll off distances
		/// </summary>
		Vector2 AudioDistanceRange {get;}

		/// <summary>
		/// Array of all audio data
		/// </summary>
		AudioData[] Audio {get;}
	}
	
	[CreateAssetMenu(fileName = "AudioSettings", menuName = "Pillow Fought/AudioSettings")]
	public class AudioSettings : ScriptableObject, IAudioSettings {
		[SerializeField]
		private Vector2 audioDistanceRange = new Vector2(15.0f, 28.0f);

		[SerializeField]
		private AudioData[] audioData = new AudioData[0];

		public Vector2 AudioDistanceRange => audioDistanceRange;

		public AudioData[] Audio => audioData;
	}

	[System.Serializable]
	public class AudioData {
		public string name;
		public AudioClip audioClip;
		public AudioMixerGroup mixerGroup;
		[HideInInspector] public AudioSource source;
		public Vector2 volRange = new Vector2(1, 1);
		public Vector2 pitchRange = new Vector2(1, 1);
		[Range(0, 1)] public float spatialBlend = 0.0f;
		public bool isLooping = false;
	}
}