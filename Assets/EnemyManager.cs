using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class EnemyManager : MonoBehaviour {

	[Header ("Spawn Information")]
	public int enemiesToSpawn = 20;
	public float multiplierPerLevel = 1.1f;
	public float spawnInterval = 10f;
	private float timeToSpawn;

	[Header ("Informational")][ShowOnlyAttribute]
	public int enemiesThisLevel;
	[ShowOnlyAttribute]
	public int enemiesLeftToSpawn;
	[ShowOnlyAttribute]
	public int enemiesLeft;

	[Header ("References")]
	public Transform[] enemies;
	public Transform holder;

	private int currentLevel;
	[Header ("HUD")]
	public Text enemiesLeftText;
	
	void OnStartGame () {
		currentLevel = GameManager.instance.CurrentLevel;
		enemiesThisLevel = (int) (enemiesToSpawn * multiplierPerLevel * currentLevel);
		enemiesLeft = enemiesThisLevel;
		enemiesLeftToSpawn = enemiesThisLevel;

		StartCoroutine ("SpawnEnemies");
	}

	void OnDisable () {
		StopCoroutine ("SpawnEnemies");
	}

	void Update () {
		enemiesLeftText.text = "Enemies Left: " + enemiesLeft;
		timeToSpawn -= Time.deltaTime;
	}

	EnemySpawn GetAvailabeSpawnPoint () {
		int r = Random.Range (0, EnemySpawn.NumberOfSpawns-1);

		return EnemySpawn.GetAvailaveSpawn ();
	}

	IEnumerator SpawnEnemies () {
		EnemySpawn spawnToUse;

		while (enemiesLeftToSpawn > 0) {

			if (timeToSpawn < 0) {
				spawnToUse = GetAvailabeSpawnPoint ();
				print ("ATTEMPTING TO SPAWN ENEMY" + spawnToUse);

				if (spawnToUse != null) {
					spawnToUse.Spawn (enemies[0], holder);//[Random.Range (0, enemies.Length-1)]);
					enemiesLeftToSpawn--;
					timeToSpawn = spawnInterval;
				}
			}

			yield return new WaitForSeconds (1f);
		}

		print ("ENEMY SPAWNING DOME");
	}
}
