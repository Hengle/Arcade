using UnityEngine;
using System.Collections;

public class PlayerData : MonoBehaviour {
	
	// Variables
	public string name;
	private int index;

	public int Index {
		get {return index;}
	}
	public int NumPlayers {
		get {return GameManager.players.Length;}
	}
	
	// Use this for initialization
	void Start () {
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
}
