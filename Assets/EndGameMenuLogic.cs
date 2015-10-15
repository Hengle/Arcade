using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class EndGameMenuLogic : MonoBehaviour {

	public Button[] buttons;

	void Awake () {
		foreach (Button b in buttons) {
			b.gameObject.SetActive (false);
		}
	}

	void OnGameOver () {
		foreach (Button b in buttons) {
			b.gameObject.SetActive (true);
		}
	}

	public void Retry () {
		GameManager.instance.StartSinglePlayer ();
	}

	public void ReturnToMainMenu () {
		GameManager.instance.ReturnToMenu ();
	}
}
