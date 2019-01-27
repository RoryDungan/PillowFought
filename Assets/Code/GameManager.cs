using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Zenject;

namespace ElMoro {
    public interface IGameManager {
        /// <summary>
        /// Reset round parameters (reset players, remove instantiated pillows and walls)
        /// </summary>
        void PlayerDefeated(int defeatedPlayerIndex);

        void ResetRound();

        void RegisterSpawner(PlayerSpawner spawner);
        void UnregisterSpawner(PlayerSpawner spawner);
    }

    public class GameManager : IGameManager {
        private int[] playerLives;
        private IList<PlayerSpawner> playerSpawners = new List<PlayerSpawner>();

        public void RegisterSpawner(PlayerSpawner spawner)
        {
            playerSpawners.Add(spawner);
        }

        public void UnregisterSpawner(PlayerSpawner spawner)
        {
            playerSpawners.Remove(spawner);
        }

        [Inject]
        public void Setup(IGameManagerSettings gmSettings) {
            playerLives = new int[] {gmSettings.MaxLives, gmSettings.MaxLives};
        }

        public void PlayerDefeated(int defeatedPlayerIndex) {
            if (defeatedPlayerIndex < 0 || defeatedPlayerIndex > 1) {
                throw new ArgumentOutOfRangeException(nameof(defeatedPlayerIndex));
            }
            //	Input Manager Hook: Freeze controls

            playerLives[defeatedPlayerIndex]--;

            if (playerLives.Any(l => l <= 0)) {
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
            var players = GameObject.FindGameObjectsWithTag(Player.Player.PlayerTag);
            for (int playerIndex = 0; playerIndex < players.Length; playerIndex++) {
                if (players[playerIndex]) {
                    UnityEngine.Object.Destroy(players[playerIndex].gameObject);
                }
                playerSpawners[playerIndex].SpawnPlayer();
            }

            // Destroy all pillows
            var pillows = GameObject.FindGameObjectsWithTag(Pillow.PillowTag);
            for (int pillowIndex = 0; pillowIndex < pillows.Length; pillowIndex++) {
                UnityEngine.Object.Destroy(pillows[pillowIndex]);
            }
        }

        private void EndGame() {
            // UI Hook: Bring up end screen
            // Can either reset current scene or go to main menu
        }
    }
}
