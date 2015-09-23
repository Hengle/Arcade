using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {

	public enum GameState {MAIN_MENU, GAME}

	public static GameManager instance;
	public static PlayerData[] players = new PlayerData[2];
	private static GameState gameState;

	public GameState CurrentGameState {
		get {return gameState;}
	}

	// Variables
	[SerializeField]
	private int currentLevel = 0;
	private bool mapEndScreen = false;

	void Start () {
		if (instance == null) {
			instance = this;
		} else {
			Destroy (this.gameObject);
		}
	}

	void UpdateState () {
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

	IEnumerator LoadLevel () {
		yield return new WaitForSeconds (0.2f);
		AsyncOperation async = Application.LoadLevelAsync (1);
		yield return async;
		Debug.Log ("Loading Complete");
	}

	public void QuitGame () {
		Application.Quit ();
	}
}
