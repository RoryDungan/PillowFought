using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ElMoro {
	public interface IGameManagerSettings {
		int MaxLives {get;}
	}

	[CreateAssetMenu(fileName = "GameManagerSettings", menuName = "Pillow Fought/GameManagerSettings")]
	public class GameManagerSettings : ScriptableObject, IGameManagerSettings {
		[SerializeField]
		private int maxLives = 5;

		public int MaxLives => maxLives;
	}
}