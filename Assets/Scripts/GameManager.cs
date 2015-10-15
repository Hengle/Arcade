using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System;


public class GameManager : MonoBehaviour {

	public static List<LevelEndCondition> levelObjectives = new List<LevelEndCondition> ();

	public enum GameState {MAIN_MENU, GAME, LOAD_LEVEL, LEVEL_COMPLETED, LOAD_NEXT_LEVEL}

	public string gameStateName;
	private static GameState gameState;
	public static GameManager instance;

	// References
	private PlayerData[] players;
	private AudioSource audioSource;

	[Header ("Audio")]
	public AudioClip menuMusic;
	public AudioClip spaceMusic;

	// Variables
	[SerializeField][Header ("Level Info")]
	private int currentLevel = 0;
	[ShowOnlyAttribute]
	public bool levelCompleted = false;

	[Header ("Obejcts")]
	public Text respawnText;
	public Text informationText;
	public GameObject inGameGUI;
	private GameObject inGameMenu;
	public EnemyManager enemyManager;
	public GameObject endGameMenu;

	private bool paused = false;

	public GameState CurrentGameState {
		get {return gameState;}
	}

	public PlayerData[] Players {
		get {return players;}
	}

	public int CurrentLevel  {
		get {return currentLevel;}
	}


	private bool mapEndScreen = false;

	bool levelLoadingDone = false;

	public int LevelIndex {
		get {return currentLevel;}
	}

	void Awake () {
		if (instance == null) {
			instance = this;
			enemyManager = GetComponent<EnemyManager> ();

			if (GameObject.Find ("MenuCanvas") == null) {

				enemyManager.enabled = true;
				informationText.enabled = false;

				UnityEngine.Object[] objects = FindObjectsOfType (typeof (GameObject));

				foreach (GameObject go in objects) {
					go.SendMessage ("OnLoadLevel", SendMessageOptions.DontRequireReceiver);
				}
			}


		} else {
			Destroy (this.gameObject);
		}
		if (players == null) {
			players = new PlayerData[1];
		}
		audioSource = GetComponent<AudioSource> ();
	}

	void Start () {
		/*if (Application.loadedLevel == 1) {
			SetGameState (GameState.GAME);
		}*/
	}

	void Update () {
		if (Input.GetKeyDown (KeyCode.Escape)) {
			if (CurrentGameState ==GameState.GAME) {
				inGameMenu.SetActive (!inGameMenu.activeInHierarchy);

				if (paused) {
					ResumeGame ();
					Cursor.lockState = CursorLockMode.Locked;
					Cursor.visible = false;
				} else {
					PauseGame ();
					Cursor.lockState = CursorLockMode.None;
					Cursor.visible = true;
				}
			}
		}
	}

	void UpdateState () {
		Debug.Log ("Updating Game State (" + gameState + ")");
		gameStateName = CurrentGameState.ToString ();

		switch (gameState) {
		case GameState.LOAD_LEVEL:
			StopCoroutine ("LoadLevel");
			StartCoroutine ("LoadLevel");
			break;
		case GameState.GAME:
			StartGame ();
			break;
		case GameState.LEVEL_COMPLETED:
			LevelComplete ();
			break;
		case GameState.LOAD_NEXT_LEVEL:
			StopCoroutine ("LoadNextLevel");
			StartCoroutine ("LoadNextLevel");
			break;
		}
	}

	public void SetGameState (GameState state) {
		gameState = state;
		UpdateState ();
	}

	public void StartSinglePlayer () {
		instance.players = new PlayerData[1];
		SetGameState (GameState.LOAD_LEVEL);
	}

	public void StartCoop () {
		instance.players = new PlayerData[2];
		SetGameState (GameState.LOAD_LEVEL);
	}

	public void NextLevel () {
		SetGameState (GameState.GAME);
	}

	public void StartRespawnTimer (GameObject go, float time) {
		StartCoroutine (RespawnTimer (go, time));
	}

	public void PauseGame () {
		print ("Sending OnPauseGame");
		UnityEngine.Object[] objects = FindObjectsOfType (typeof (GameObject));
		foreach (GameObject go in objects) {
			go.SendMessage ("OnPauseGame", SendMessageOptions.DontRequireReceiver);
		}
	}

	public void ResumeGame () {
		print ("Sending OnResumeGame");
		UnityEngine.Object[] objects = FindObjectsOfType (typeof (GameObject));
		foreach (GameObject go in objects) {
			go.SendMessage ("OnResumeGame", SendMessageOptions.DontRequireReceiver);
		}
	}

	void StartGame () {
		print ("Sending OnStartGame");
		UnityEngine.Object[] objects = FindObjectsOfType (typeof (GameObject));
		foreach (GameObject go in objects) {
			go.SendMessage ("OnStartGame", SendMessageOptions.DontRequireReceiver);
		}
	}

	void LevelComplete () {
		print ("Sending OnLevelComplete");
		UnityEngine.Object[] objects = FindObjectsOfType (typeof (GameObject));
		foreach (GameObject go in objects) {
			go.SendMessage ("OnLevelComplete", SendMessageOptions.DontRequireReceiver);
		}
	}

	// Called on every GameObject once Level Laoding Begins
	void OnLoadLevel () {
		Cursor.lockState = CursorLockMode.Locked;
		Cursor.visible = false;

		currentLevel++;
		levelCompleted = false;
	}

	// Called when all of current levels goals are met
	void OnLevelComplete () {
		enemyManager.enabled = false;
		NextLevel ();
		//Application.LoadLevel (2);
	}

	// Called on every GameObject once game level has been loaded
	void OnStartGame () {
		inGameGUI = GameObject.Find ("InGameGUI");
		inGameMenu = inGameGUI.transform.FindChild ("InGameMenu").gameObject;
		informationText = GameObject.Find ("InformationLabel").GetComponent<Text> ();
		informationText.text = "";

		StartCoroutine (AddPlayerTargets ());
		StartCoroutine (CheckForLevelEnd ());

		audioSource.clip = spaceMusic;
		audioSource.enabled = true;
		audioSource.Play ();

		enemyManager.enabled = true;


		if (respawnText == null) {
			respawnText = inGameGUI.transform.FindChild ("HUD").FindChild ("RespawnText").GetComponent<Text> ();
		}
		respawnText.gameObject.SetActive (false);

	}

	void OnPauseGame () {
		paused = true;
	}

	void OnResumeGame () {
		paused = false;
	}
	
	public void ExitToWindows () {
		print ("Exiting");
		Application.Quit ();
	}

	public LevelEndCondition AddLevelObjective (string _name) {
		foreach (LevelEndCondition _endCondition in levelObjectives) {
			if (_endCondition.name.Equals (name)) {
				print ("END CONDITION WITH NAME ALEADY EXISTS! (" + _name + ")");
				return null;
			}
		}

		LevelEndCondition endCondition = new LevelEndCondition (_name);
		levelObjectives.Add (endCondition);
		return endCondition;
	}

	public void SetLevelObjectAsDone (string _name) {
		foreach (LevelEndCondition endCondition in levelObjectives) {
			if (endCondition.name.Equals (_name)) {
				endCondition.done = true;
				print (endCondition.name + "is done");
			}
		}
	}

	public void ReturnToMenu () {
		Application.LoadLevel (0);
	}

	void SendGameOver () {
		print ("GAME OVER");
		respawnText.text = "Game Over";
		endGameMenu.SetActive (true);

		
		UnityEngine.Object[] objects = FindObjectsOfType (typeof (GameObject));
		foreach (GameObject go in objects) {
			go.SendMessage ("OnGameOver", SendMessageOptions.DontRequireReceiver);
		}
	}
	
	IEnumerator RespawnTimer (GameObject go, float time) {
		PathfindingManager.RemoveTarget (transform, true);
		respawnText.gameObject.SetActive (true);

		if (time == -1) {

		} else {
			while (time > 0) {
				respawnText.text = String.Format ("Respawning\n{0:f1} s", time);
				time -= 0.1f;
				yield return new WaitForSeconds (0.1f);
			}
			respawnText.gameObject.SetActive (false);
			go.SetActive (true);
			go.SendMessage ("OnRespawn");
			PathfindingManager.AddTarget (go.transform, true);
		}

		yield return null;
	}

	IEnumerator LoadLevel () {
		AsyncOperation async = Application.LoadLevelAsync (1);
		yield return async;

		UnityEngine.Object[] objects = FindObjectsOfType (typeof (GameObject));
		foreach (GameObject go in objects) {
			go.SendMessage ("OnLoadLevel", SendMessageOptions.DontRequireReceiver);
		}
	}

	IEnumerator AddPlayerTargets () {
		yield return new WaitForEndOfFrame ();
		
		foreach (PlayerData pd in instance.players) {
			print (pd.transform.name);
			PathfindingManager.AddTarget (pd.transform, true);
		}
	}

	IEnumerator CheckForLevelEnd () {
		bool isCompleted = false;
		yield return new WaitForSeconds (1f);

		while (!isCompleted) {
			isCompleted = true;
			foreach (LevelEndCondition endCondition in levelObjectives) {
				if (!endCondition.done) {
					isCompleted = false;
				}
			}
			yield return new WaitForSeconds (0.1f);
		}
		levelCompleted = true;
		SetGameState (GameState.LEVEL_COMPLETED);
		yield return null;
	}

	public class LevelEndCondition {
		public bool done = false;
		public string name;

		public LevelEndCondition (string _name) {
			name = _name;
		}

	}
}
