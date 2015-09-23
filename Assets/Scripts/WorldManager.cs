using UnityEngine;
using System.Collections;

public class WorldManager : MonoBehaviour {

	public static WorldManager instance;

	[SerializeField]
	private Boundary boundary;

	public Boundary WorldBorder {
		get {return boundary;}
	}

	void Awake () {
		if (instance == null) {
			instance = this;
		} else {
			Destroy (this);
		}
	}

	void Start () {
		GameManager.instance.SetGameState (GameManager.GameState.GAME);
	}
}
