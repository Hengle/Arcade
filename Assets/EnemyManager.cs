using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class EnemyManager : MonoBehaviour {

	public static EnemyManager instance;

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
		enemiesThisLevel = (int) (enemiesToSpawn * multiplierPerLevel * currentLevel);
		enemiesLeft = enemiesThisLevel;
		enemiesLeftToSpawn = enemiesThisLevel;
		enemiesLeftText = GameObject.FindWithTag ("RespawnText").GetComponent<Text> ();

		holder = GameObject.Find ("EnemiesHolder").transform;
		GameManager.instance.AddLevelObjective ("DestroAll");
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
				GameManager.instance.SetLevelObjectAsDone ("DestroAll");
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

		while (enemiesLeftToSpawn > 0) {

			if (!paused) {
				if (timeToSpawn <= 0) {
					spawnToUse = EnemySpawn.GetAvailaveSpawn ();;

					if (spawnToUse != null) {
						spawnToUse.Spawn (enemies[0], holder);//[Random.Range (0, enemies.Length-1)]);
						enemiesLeftToSpawn--;
						timeToSpawn = spawnInterval;
					}
				}
			}

			yield return new WaitForSeconds (1f);
		}

		print ("ENEMY SPAWNING DOME");
	}
}
