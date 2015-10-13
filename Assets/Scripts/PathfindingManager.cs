using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PathfindingManager : MonoBehaviour {

	public static PathfindingManager instance;

	private List<Target> targets = new List<Target> ();
	private List<Target> players = new List<Target> ();

	public float maxTargetLifeTime = 30f;
	public int extraWaypoints = 10;
	public Boundary waypointDistanceFromBase = new Boundary (500, 3000, 500, 3000, 500, 3000);
	
	public bool debugNavmesh = false;
	private bool waypointUpdateQueued = false;
	public Transform waypoint;
	private System.Random random;

	public Target[] Waypoints {
		get {return targets.ToArray ();}
	}
	public Target[] PlayerTargets {
		get {return players.ToArray ();}
	}

	void Awake () {
		if (instance == null) {
			instance = this;
		} else {
			Destroy (this.gameObject);
		}
	}

	void Start () {
		random = new System.Random ();
	}

	void OnLoadLevel () {
		CreateBaseNavGrid ();
	}

	void OnStartGame () {
		for (int i = 0; i < transform.childCount; i++) {
			targets.Add (new Target (transform.GetChild (i)));
		}
		StartCoroutine ("UpdateLifeTimes");
		StartCoroutine ("UpdatePositions");
		StartCoroutine ("UpdateWaypoints");
		waypointUpdateQueued = true;
	}

	void CreateBaseNavGrid () {
		Transform t;
		Vector3 pos;

		for (int a = 0; a < extraWaypoints; a++) {
			int i, j, k;
			float x = (Random.value < 0.5) ? Random.Range (waypointDistanceFromBase.xMin, waypointDistanceFromBase.xMin) : -Random.Range (waypointDistanceFromBase.xMin, waypointDistanceFromBase.xMin);
			float y = (Random.value < 0.5) ? Random.Range (waypointDistanceFromBase.yMin, waypointDistanceFromBase.yMax) : -Random.Range (waypointDistanceFromBase.yMin, waypointDistanceFromBase.yMax);
			float z = (Random.value < 0.5) ? Random.Range (waypointDistanceFromBase.zMin, waypointDistanceFromBase.zMax) : -Random.Range (waypointDistanceFromBase.zMin, waypointDistanceFromBase.zMax);
			pos = new Vector3 (x, y, z);

			t = (Transform) Instantiate (waypoint, pos, Quaternion.identity);
			t.name = ("Waypoint (" + t.position + ")");
			t.SetParent (transform);
		}
	}

	void OnDrawGizmos () {
		if (debugNavmesh) {
			foreach (Target t in targets) {
				foreach (Target t2 in targets) {
					Debug.DrawLine (t.transform.position, t2.transform.position, Color.blue);
				}
			}
		}
	}

	public Target GetNewRandomTarget () {
		int i = random.Next (0, instance.targets.Count);
		return instance.targets[i];
	}

	public Target GetNewRandomTargetExcluding (Target excluded) {
		targets.Remove (excluded);
		Target t = GetNewRandomTarget ();
		targets.Add (excluded);
		return t;
	}

	public static int AddTarget (Transform t, bool isPlayer) {
		if (isPlayer) {
			instance.players.Add (new Target (t));
			return instance.players.Count;
		} else {
			instance.targets.Add (new Target (t));
			return instance.targets.Count;
		}
	}

	IEnumerator UpdateLifeTimes () {
		while (true) {
			foreach (Target t in instance.targets) {
				if (!t.persistent) {
					t.lifeTime -= 0.5f;
					if (t.lifeTime <= 0) {
						instance.targets.Remove (t);
					}
				}
			}
			yield return new WaitForSeconds (0.5f);
		}
	}

	IEnumerator UpdatePositions () {
		while (true) {
			foreach (Target t in instance.players) {
				if (t.position != null) {
					t.position = t.transform.position;
				} else {
					print ("A PLAYER SHOULD NEVER GET DESTROYED!");
				}
			}
			yield return new WaitForSeconds (0.1f);
		}
	}

	IEnumerator UpdateWaypoints () {
		while (true) {
			if (waypointUpdateQueued) {
				foreach (Target t in targets) {
					// ADD Update script here
					yield return new WaitForSeconds (1f);
					waypointUpdateQueued = false;
				}
			}
			yield return new WaitForSeconds (5f);
		}
	}
}
