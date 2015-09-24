using UnityEngine;
using System.Collections;

public class ProjectileMultiSpawn : MonoBehaviour {

	[SerializeField]
	private Transform[] spawnPoints;

	public int NumberOfSpawns {
		get {return spawnPoints.Length;}
	}

	public Transform[] SpawnPoint {
		get {return spawnPoints;}
	}
}
