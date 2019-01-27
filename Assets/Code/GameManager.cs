using System;
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
    }

    public class GameManager : IGameManager {
        private int[] playerLives;
        private PlayerSpawner[] playerSpawners;

        [Inject]
        public void Setup(IGameManagerSettings gmSettings) {
            playerLives = new int[] {gmSettings.MaxLives, gmSettings.MaxLives};

            var spawn1Obj = GameObject.FindGameObjectWithTag("Spawn1");
            if (spawn1Obj == null)
            {
                throw new Exception("Could not find object with tag Spawn1 in the scene.");
            }
            var spawn1 = spawn1Obj.GetComponent<PlayerSpawner>();
            if (spawn1 == null)
            {
                throw new Exception("Found object with Spawn1 tag but it was missing PlayerSpawner component.");
            }

            var spawn2Obj = GameObject.FindGameObjectWithTag("Spawn2");
            if (spawn2Obj == null)
            {
                throw new Exception("Could not find object with tag Spawn2 in the scene.");
            }
            var spawn2 = spawn2Obj.GetComponent<PlayerSpawner>();
            if (spawn2 == null)
            {
                throw new Exception("Found object with Spawn2 tag but it was missing PlayerSpawner component.");
            }

            playerSpawners = new [] { spawn1, spawn2 };

            foreach (var spawner in playerSpawners) {
                spawner.SpawnPlayer();
            }
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
