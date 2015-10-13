using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class EnemyManager : MonoBehaviour {

	public static EnemyManager instance;

	[Header ("Spawn Information")]
	public int enemiesToSpawn = 20;
	public float multiplierPerLevel = 1.1f;
	public float spawnInterval = 10f;
	private float timeToSpawn;

	public Queue<Transform> enemySpawnQueue;

	[Header ("Informational")][ShowOnlyAttribute]
	public int enemiesThisLevel;
	[ShowOnlyAttribute]
	public int enemiesLeftToSpawn;
	[ShowOnlyAttribute]
	public int enemiesLeft;

	[Header ("References")]
	public Transform[] enemies;
	private Transform holder;

	private int currentLevel;
	[Header ("HUD")]
	public Text enemiesLeftText;

	private bool paused = false;

	void Awake () {
		if (instance != null) {
			Destroy (this);
		} else {
			instance = this;
		}
	}
	
	void OnStartGame () {
		currentLevel = GameManager.instance.CurrentLevel;
		enemiesThisLevel = (int) (enemiesToSpawn + (enemiesToSpawn * (multiplierPerLevel - 1) * (currentLevel - 1)));
		enemiesLeft = enemiesThisLevel;
		enemiesLeftToSpawn = enemiesThisLevel;
		enemiesLeftText = GameObject.FindWithTag ("RespawnText").GetComponent<Text> ();

		holder = GameObject.Find ("EnemiesHolder").transform;
		GameManager.instance.AddLevelObjective ("Destroy All Enemies");
		StartCoroutine ("SpawnEnemies");
	}

	void OnDisable () {
		StopCoroutine ("SpawnEnemies");
	}

	void Update () {
		if (!paused) {
			enemiesLeftText.text = "Enemies Left: " + enemiesLeft;
			timeToSpawn -= Time.deltaTime;

			if (enemiesLeft <= 0) {
				GameManager.instance.SetLevelObjectAsDone ("Destroy All Enemies");
			}
		}
	}

	void EnemyDestroyed (int score) {
		enemiesLeft--;
	}

	void OnPauseGame () {
		paused = true;
	}

	void OnResumeGame () {
		paused = false;
	}
	
	IEnumerator SpawnEnemies () {
		EnemySpawn spawnToUse;
		enemySpawnQueue = new Queue<Transform> ();

		for (int i = 0; i < enemiesThisLevel; i++) {
			enemySpawnQueue.Enqueue (enemies[Random.Range (0, enemies.Length -1)]);
		}

		while (enemiesLeftToSpawn > 0) {

			if (!paused) {
				if (timeToSpawn <= 0) {
					spawnToUse = EnemySpawn.GetAvailaveSpawn ();;

					if (spawnToUse != null) {
						spawnToUse.Spawn (enemySpawnQueue.Dequeue (), holder);
						enemiesLeftToSpawn--;
						timeToSpawn = spawnInterval;
					}
				}
			}

			yield return new WaitForSeconds (1f);
		}

		print ("ENEMY SPAWNING DONE");
	}	
}
