using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class WorldManager : MonoBehaviour {

	public static WorldManager instance;

	public Transform playerSpawnPoint;
	public List<IWorldGen> worldGenerators = new List<IWorldGen> ();


	[SerializeField]
	private Boundary boundary;
	public int seed;

	public int hazardsToGen = 5;

	bool worldGenerated;
	bool levelGenerated = false;

	public Boundary WorldBorder {
		get {return boundary;}
	}

	public bool LevelGenerated {
		get {return levelGenerated;}
	}

	void OnEnabled () {
		if (instance != null) {
			worldGenerated = false;
		}
	}

	void Awake () {
		if (instance == null) {
			instance = this;

			if (seed == null) {
				seed = Time.time.GetHashCode ();
			}
		} else {
			Destroy (this);
		}
	}

	void OnLoadLevel () {
		print ("OnLoadLevel Called");
		worldGenerated = false;
		worldGenerators.AddRange (GetComponents<IWorldGen> ());
		
		StartCoroutine (GenerateTerrain ());
	}

	IEnumerator GenerateTerrain () {
		float timeAtStart = Time.time;

		int level = GameManager.instance.LevelIndex;
		List<int> generators = new List<int> ();

		for (int i = 0; i < worldGenerators.Count; i++) {
			for (int j = 0; j < worldGenerators[i].GetCurrentGenChance (); j++) {
				generators.Add (i);
			}
		}

		for (int i = 0; i < hazardsToGen; i++) {
			print ("[WORLDGEN]: Starting to add hazards");
			int index = UnityEngine.Random.Range (0, worldGenerators.Count -1);
			Vector3 position = new Vector3 (UnityEngine.Random.Range (boundary.xMin, boundary.xMax), UnityEngine.Random.Range (boundary.yMin, boundary.yMax), UnityEngine.Random.Range (boundary.zMin, boundary.zMax));

			IEnumerator e = worldGenerators[index].Generate (position);
			while (e.MoveNext()) yield return e.Current;

			print (worldGenerators[index].ToString () + " DONE!");
		}

		float timeAtEnd = Time.time;

		print (String.Format("LEVEL GENERATION took {0:f}s to generate", (timeAtEnd - timeAtStart)));
		GameManager.instance.SetGameState (GameManager.GameState.GAME);
	}
}
