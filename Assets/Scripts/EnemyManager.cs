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
	public int enemiesDestroyed;
	private int enemiesCurrently;

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
		enemiesDestroyed = 0;
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
			enemiesLeftText.text = "Enemies Destoyed: " + enemiesDestroyed;
			timeToSpawn -= Time.deltaTime;

			if (enemiesDestroyed > 10000) {
				GameManager.instance.SetLevelObjectAsDone ("Destroy All Enemies");
			}
		}
	}

	void EnemyDestroyed (int score) {
		enemiesDestroyed++;
		enemiesCurrently--;
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

		while (enemiesLeftToSpawn > 0) {

			if (!paused) {
				if (enemiesThisLevel > enemiesCurrently) {
					spawnToUse = EnemySpawn.GetAvailaveSpawn ();;

					if (spawnToUse != null) {
						spawnToUse.Spawn (enemies[Random.Range (0, 2)], holder);
						enemiesCurrently++;
						timeToSpawn = spawnInterval;
					}
				}
			}

			yield return new WaitForSeconds (spawnInterval);
		}

		print ("ENEMY SPAWNING DONE");
	}	
}
