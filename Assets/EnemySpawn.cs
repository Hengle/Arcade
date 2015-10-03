using UnityEngine;
using System.Collections.Generic;

public class EnemySpawn : MonoBehaviour {

	private static List<EnemySpawn> enemySpawns = new List<EnemySpawn> ();

	public float spawnDelay = 10f;
	public Transform spawnPoint;
	private float minTimeToSpawn;
	private bool canSpawn = true;

	public static int NumberOfSpawns {
		get {return enemySpawns.Count;}
	}


	public bool CanSpawn {
		get {return canSpawn;}
	}

	void Start () {
		enemySpawns.Add (this);
		minTimeToSpawn = spawnDelay;
	}

	void Update () {
		if (minTimeToSpawn < 0) {
			canSpawn = true;
		}
		minTimeToSpawn -= Time.deltaTime;
	}

	public static EnemySpawn GetAvailaveSpawn () {
		List<EnemySpawn> availablePoint = new List<EnemySpawn> ();
		int index;

		foreach (EnemySpawn spawn in enemySpawns) {
			if (spawn.CanSpawn) {
				availablePoint.Add (spawn);
			}
		}

		if (availablePoint.Count > 0) {
			return availablePoint[Random.Range (0, availablePoint.Count -1)];		}
		return null;
	}

	public void Spawn (Transform enemy, Transform holder) {
		canSpawn = false;
		minTimeToSpawn = spawnDelay;

		Transform t = (Transform) Instantiate (enemy, spawnPoint.position, spawnPoint.rotation);
		t.SetParent (holder);
	}
}
