using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace ElMoro
{
    public class PlayerSpawner : MonoBehaviour
    {
        [SerializeField]
        private GameObject playerPrefab;

        [SerializeField]
        [Range(0, 1)]
        private int playerIndex;

        [Inject]
        private Player.Player.Factory playerFactory;

        [Inject]
        private IGameManager gameManager;

        private void Awake()
        {
            if (playerPrefab == null)
            {
                throw new Exception("Player spawner has no player prefab assigned!");
            }
        }

        private void Start()
        {
            gameManager.RegisterSpawner(this);
            SpawnPlayer();
        }

        private void OnDestroy()
        {
            gameManager.UnregisterSpawner(this);
        }

        private void Update()
        {
            // // Testing
            // if (Input.GetKeyDown(KeyCode.P))
            // {
            //     SpawnPlayer();
            // }
        }

        public void SpawnPlayer()
        {
            var newPlayer = playerFactory.Create(playerPrefab);	
            newPlayer.ControllerIndex = playerIndex;
            newPlayer.Position = transform.position;
        }
    }
}
