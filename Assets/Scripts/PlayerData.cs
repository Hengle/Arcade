using UnityEngine;
using EnergyBarToolkit;
using System.Collections;

public class PlayerData : MonoBehaviour {
	
	// Variables
	public string name;
	public int lives;

	public bool canRespawn;
	public float respawnTime = 5f;
	private int index;

	public GameObject livesBar;

	public int Index {
		get {return index;}
	}
	public int NumPlayers {
		get {return GameManager.players.Length;}
	}
	public bool CanRespawn {
		get {return canRespawn;}
	}
	
	// Use this for initialization
	void Start () {
		livesBar.GetComponent<EnergyBar> ().valueMax = lives;
		livesBar.GetComponent<RepeatedRendererUGUI> ().repeatCount = lives;
		livesBar.GetComponent<EnergyBar> ().valueCurrent = lives;

		for (int i = 0; i < GameManager.players.Length; i++) {
			if (GameManager.players[i] == null) {
				GameManager.players[i] = this;
				index = i+1;
				break;
			}
		}
	}

	void OnDestroyed () {
		GameManager.players[index] = null;
	}

	public bool Respawn () {
		lives--;
		livesBar.GetComponent<EnergyBar> ().valueCurrent = lives;

		if (lives > 0) {
			GameManager.instance.StartRespawnTimer (gameObject, respawnTime);
			return true;
		}
		return false;
	}
}
