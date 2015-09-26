using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;


public class GameManager : MonoBehaviour {

	public enum GameState {MAIN_MENU, GAME}

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
			instance = this;
		} else {
			Destroy (this.gameObject);
		}
		if (players == null) {
			players = new PlayerData[1];
		}
	}
	void Start () {
		Debug.Log ("Number of players: " + players.Length);
		StartCoroutine (AddPlayerTargets ());
	}

	void UpdateState () {
		Debug.Log ("Updating Game State (" + gameState + ")");
		switch (gameState) {
		case GameState.GAME:
			GetComponent<AudioSource> ().enabled = true;
			respawnText.gameObject.SetActive (false);
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
		UnityEngine.Object[] objects = FindObjectsOfType (typeof (IPausable));
		foreach (GameObject go in objects) {
			go.SendMessage ("OnPauseGame", SendMessageOptions.DontRequireReceiver);
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
		yield return new WaitForSeconds (0.1f);
		AsyncOperation async = Application.LoadLevelAsync (1);
		yield return async;
		Debug.Log ("Loading Complete");
	}

	IEnumerator AddPlayerTargets () {
		yield return new WaitForEndOfFrame ();

		foreach (PlayerData pd in players) {
			print (pd.transform.name);
			EnemyTarget.AddTarget (pd.transform, true);
		}
	}
}
