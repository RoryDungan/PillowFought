using UnityEngine;
using Zenject;

namespace ElMoro {
	public interface IGameManager {
		/// <summary>
		/// Reset round parameters (reset players, remove instantiated pillows and walls)
		// /// </summary>
		void NextRound(string deadPlayerTag);
		
		void ResetRound();
	}

	public class GameManager : IGameManager {
		private int[] playerLives;
		private PlayerSpawner[] playerSpawners;

		public GameManager(IGameManagerSettings gmSettings) {
			playerLives = new int[] {gmSettings.MaxLives, gmSettings.MaxLives};

			playerSpawners = new PlayerSpawner[] {
				GameObject.FindGameObjectWithTag("Spawn1").GetComponent<PlayerSpawner>(),
				GameObject.FindGameObjectWithTag("Spawn2").GetComponent<PlayerSpawner>()};

			foreach (PlayerSpawner spawner in playerSpawners) {
				spawner.SpawnPlayer();
			}
		}

		public void NextRound(string deadPlayerTag) {
			//	Input Manager Hook: Freeze controls

			playerLives[deadPlayerTag.Equals("Player1") ? 0 : 1]--;

			if (playerLives[0] == 0 || playerLives[1] == 0) {
				EndGame();
			} else {
				EndRound();
			}
		}

		private void EndRound() {
			// UI Hook: bring up round end UI
			// Needs to trigger ResetRound() 
		}

		public void ResetRound() {
			// Input Manager Hook: Re-enable controls

			// Reset player parameters
			GameObject[] players = new GameObject[] {
				GameObject.FindGameObjectWithTag("Player1"), GameObject.FindGameObjectWithTag("Player2")};
			for (int playerIndex = 0; playerIndex < players.Length; playerIndex++) {
				if (players[playerIndex]) {
					Object.Destroy(players[playerIndex].gameObject);
				}
				playerSpawners[playerIndex].SpawnPlayer();
			}

			// Destroy all pillows
			GameObject[] pillows = GameObject.FindGameObjectsWithTag("Pillow");
			for (int pillowIndex = 0; pillowIndex < pillows.Length; pillowIndex++) {
				Object.Destroy(pillows[pillowIndex]);
			}

			// Destroy all walls
			GameObject[] walls = GameObject.FindGameObjectsWithTag("Wall");
			for (int wallIndex = 0; wallIndex < walls.Length; wallIndex++) {
				Object.Destroy(walls[wallIndex]);
			}
		}

		private void EndGame() {
			// UI Hook: Bring up end screen
			// Can either reset current scene or go to main menu
		}
	}
}