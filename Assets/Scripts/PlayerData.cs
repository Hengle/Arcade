using UnityEngine;
using EnergyBarToolkit;
using System.Collections;

public class PlayerData : MonoBehaviour {
	
	// Variables
	public string playerName;
	public int lives;

	public bool canRespawn;
	public float respawnTime = 5f;
	private int index;

	public GameObject livesBar;

	public int Index {
		get {return index+1;}
	}
	public int NumPlayers {
		get {return GameManager.instance.Players.Length;}
	}
	public bool CanRespawn {
		get {return canRespawn;}
	}
	
	// Use this for initialization
	void Start () {
		livesBar.GetComponent<EnergyBar> ().valueMax = lives;
		livesBar.GetComponent<RepeatedRendererUGUI> ().repeatCount = lives;
		livesBar.GetComponent<EnergyBar> ().valueCurrent = lives;

		for (int i = 0; i < GameManager.instance.Players.Length; i++) {
			if (GameManager.instance.Players[i] == null) {
				GameManager.instance.Players[i] = this;
				index = i;
				break;
			}
		}
	}

	void OnDestroyed () {
		GameManager.instance.Players[index] = null;
	}

	public bool Respawn () {
		lives--;
		livesBar.GetComponent<EnergyBar> ().valueCurrent = lives;

		if (lives > 0) {
			GameManager.instance.StartRespawnTimer (gameObject, respawnTime);
			return true;
		} else {
			GameManager.instance.StartRespawnTimer (gameObject, -1);
		}
		return false;
	}
}
