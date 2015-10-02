using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;


public class GameManager : MonoBehaviour {

	public enum GameState {MAIN_MENU, GAME, LOAD_LEVEL}
	private static GameState gameState;
	public static GameManager instance;

	// References
	private PlayerData[] players;
	private AudioSource audioSource;
	public AudioClip menuMusic;
	public AudioClip spaceMusic;

	public Text respawnText;
	public GameObject InGameMenu;

	public GameState CurrentGameState {
		get {return gameState;}
	}

	public PlayerData[] Players {
		get {return players;}
	}

	// Variables
	[SerializeField]
	private int currentLevel = 0;
	private bool mapEndScreen = false;

	bool levelLoadingDone = false;

	public int LevelIndex {
		get {return currentLevel;}
	}

	void Awake () {
		if (instance == null) {
			instance = this;

			if (GameObject.Find ("MenuCanvas") == null) {
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
		if (Application.loadedLevel == 1) {
			SetGameState (GameState.GAME);
		}
	}

	void Update () {
		if (Input.GetKeyDown (KeyCode.Escape)) {
			if (CurrentGameState ==GameState.GAME) {
				InGameMenu.SetActive (!InGameMenu.activeInHierarchy);
			}
		}
	}

	void UpdateState () {
		Debug.Log ("Updating Game State (" + gameState + ")");
		switch (gameState) {
		case GameState.LOAD_LEVEL:
			StopCoroutine ("LoadLevel");
			StartCoroutine ("LoadLevel");
			break;
		case GameState.GAME:
			StartGame ();
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

	public void StartRespawnTimer (GameObject go, float time) {
		StartCoroutine (RespawnTimer (go, time));
	}

	public void PauseGame () {
		print ("Sending OnPauseGame");
		UnityEngine.Object[] objects = FindObjectsOfType (typeof (IPausable));
		foreach (GameObject go in objects) {
			go.SendMessage ("OnPauseGame", SendMessageOptions.DontRequireReceiver);
		}
	}

	public void ResumeGame () {
		print ("Sending OnResumeGame");
		UnityEngine.Object[] objects = FindObjectsOfType (typeof (IPausable));
		foreach (GameObject go in objects) {
			go.SendMessage ("OnResumeGame", SendMessageOptions.DontRequireReceiver);
		}
	}

	void StartGame () {
		print ("Sending OnGameStart");
		UnityEngine.Object[] objects = FindObjectsOfType (typeof (GameObject));
		foreach (GameObject go in objects) {
			go.SendMessage ("OnStartGame", SendMessageOptions.DontRequireReceiver);
		}
	}

	// Called on every GameObject once game level has been loaded
	void OnStartGame () {
		StartCoroutine (AddPlayerTargets ());
		audioSource.clip = spaceMusic;
		audioSource.enabled = true;
		if (respawnText != null) {
			respawnText.gameObject.SetActive (false);
		}
	}

	public void ExitToWindows () {
		print ("Exiting");
		Application.Quit ();
	}
	
	IEnumerator RespawnTimer (GameObject go, float time) {
		respawnText.gameObject.SetActive (true);
		if (time == -1) {
			respawnText.text = "Game Over";
		} else {
			while (time > 0) {
				respawnText.text = String.Format ("Respawning\n{0:f1} s", time);
				time -= 0.1f;
				yield return new WaitForSeconds (0.1f);
			}
			respawnText.gameObject.SetActive (false);
			go.SetActive (true);
		}

		yield return null;
	}

	IEnumerator LoadLevel () {
		UnityEngine.Object[] objects = FindObjectsOfType (typeof (GameObject));
		foreach (GameObject go in objects) {
			go.SendMessage ("OnLoadLevel", SendMessageOptions.DontRequireReceiver);
		}

		
		AsyncOperation async = Application.LoadLevelAsync (1);
		yield return async;
		levelLoadingDone = true;
	}

	IEnumerator AddPlayerTargets () {
		yield return new WaitForEndOfFrame ();
		
		foreach (PlayerData pd in instance.players) {
			print (pd.transform.name);
			PathfindingManager.AddTarget (pd.transform, true);
		}

	}
}
