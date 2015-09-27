using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;


public class GameManager : MonoBehaviour {

	public enum GameState {MAIN_MENU, GAME, LOAD_LEVEL}

	public static GameManager instance;

	private PlayerData[] players;
	private GameState gameState;

	public Text respawnText;

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

	void Awake () {
		if (instance == null) {
			if (Application.loadedLevel == 1) {
				StartCoroutine (AddPlayerTargets ());
			}
			instance = this;
		} else {
			print ("GAME MANGER ALREDY EXISTS");
			Destroy (this.gameObject);
		}
		if (players == null) {
			players = new PlayerData[1];
		}
	}
	void Start () {
		Debug.Log ("Number of players: " + players.Length);
	}

	void UpdateState () {
		Debug.Log ("Updating Game State (" + gameState + ")");
		switch (gameState) {
		case GameState.LOAD_LEVEL:
			break;
		case GameState.GAME:
			StartGame ();
			GetComponent<AudioSource> ().enabled = true; // Implement these in there relative calsses with the OnGameStart ()
			respawnText.gameObject.SetActive (false); // Implement these in there relative calsses with the OnGameStart ()
			break;
		}
	}

	public void SetGameState (GameState state) {
		gameState = state;
		UpdateState ();
	}

	public void StartSinglePlayer () {
		players = new PlayerData[1];
		StartCoroutine ("LoadLevel");
	}

	public void StartCoop () {
		players = new PlayerData[2];
		StartCoroutine ("LoadLevel");
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

	public void StartGame () {
		print ("Sending OnGameStart");
		UnityEngine.Object[] objects = FindObjectsOfType (typeof (GameObject));
		foreach (GameObject go in objects) {
			go.SendMessage ("OnStartGame", SendMessageOptions.DontRequireReceiver);
		}
	}

	public void ExitToWindows () {
		print ("Exiting");
		Application.Quit ();
	}

	IEnumerator RespawnTimer (GameObject go, float time) {
		respawnText.gameObject.SetActive (true);
		while (time > 0) {
			respawnText.text = String.Format ("Respawning\n{0:f1} s", time);
			time -= 0.1f;
			yield return new WaitForSeconds (0.1f);
		}
		go.SetActive (true);
		go.transform.position = WorldManager.instance.playerSpawnPoint.position;
		respawnText.gameObject.SetActive (false);

		yield return null;
	}

	IEnumerator LoadLevel () {
		SetGameState (GameState.LOAD_LEVEL);
		yield return new WaitForSeconds (0.1f);
		AsyncOperation async = Application.LoadLevelAsync (1);
		yield return async;
		Debug.Log ("Loading Complete");
		SetGameState (GameState.GAME);
	}

	IEnumerator AddPlayerTargets () {
		yield return new WaitForEndOfFrame ();
		
		foreach (PlayerData pd in instance.players) {
			print (pd.transform.name);
			EnemyTarget.AddTarget (pd.transform, true);
		}

	}
}
