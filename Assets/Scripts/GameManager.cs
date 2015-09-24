using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;

public class GameManager : MonoBehaviour {

	public enum GameState {MAIN_MENU, GAME}

	public static GameManager instance;

	public static PlayerData[] players = new PlayerData[2];
	private static GameState gameState;

	public Text respawnText;

	public GameState CurrentGameState {
		get {return gameState;}
	}

	// Variables
	[SerializeField]
	private int currentLevel = 0;
	private bool mapEndScreen = false;

	void Start () {
		respawnText.gameObject.SetActive (false);

		if (instance == null) {
			instance = this;
		} else {
			Destroy (this.gameObject);
		}
	}

	void UpdateState () {
		Debug.Log ("Updating Game State (" + gameState + ")");
		switch (gameState) {
		case GameState.GAME:
			GetComponent<AudioSource> ().enabled = true;
			break;
		}
	}

	public void SetGameState (GameState state) {
		gameState = state;
		UpdateState ();
	}

	public void StartSinglePlayer () {
		StartCoroutine ("LoadLevel");
	}

	public void StartCoop () {
		StartCoroutine ("LoadLevel");
	}

	public void StartRespawnTimer (GameObject go, float time) {
		StartCoroutine (RespawnTimer (go, time));
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

	public void QuitGame () {
		Application.Quit ();
	}
}
