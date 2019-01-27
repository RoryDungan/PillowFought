using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using ElMoro;

public class PillowSpawner : MonoBehaviour
{
	public float spawnRate = 1.5f;
	public int pillowLimit = 10;
	public GameObject[] pillowPrefabs;

	private List<IPillow> pillows;

	private Vector3[] spawnBounds;

	[Inject]
	private Pillow.Factory pillowFactory;

    void Start()
    {
        spawnBounds = new Vector3[2];
		spawnBounds[0] = GetComponent<Collider>().bounds.max;
		spawnBounds[0] = GetComponent<Collider>().bounds.min;

		pillows = new List<IPillow>();
    }

	public void StartSpawn() {
        StartCoroutine("SpawnPillows");
	}

	public void StopSpawn() {
        StopCoroutine("SpawnPillows");
	}

	private IEnumerator SpawnPillows() {
		while (true) {
			if (pillows.Count < pillowLimit) {
				var newPillow = pillowFactory.Create(pillowPrefabs[Random.Range(0, pillowPrefabs.Length)]);
				pillows.Add(newPillow);
			}
		}
	}
}
