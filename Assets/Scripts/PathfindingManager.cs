using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PathfindingManager : MonoBehaviour {

	public static PathfindingManager instance;

	private List<Target> targets = new List<Target> ();
	private List<Target> players = new List<Target> ();

	public float maxTargetLifeTime = 30f;
	public int waypoints = 20;
	private System.Random random;


	public bool debugNavmesh = false;
	private bool waypointUpdateQueued = false;

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

		for (int i = 0; i < transform.childCount; i++) {
			targets.Add (new Target (transform.GetChild (i)));
		}
		StartCoroutine ("UpdateLifeTimes");
		StartCoroutine ("UpdatePositions");
		StartCoroutine ("UpdateWaypoints");
		waypointUpdateQueued = true;
	}

	void Update () {
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
					foreach (Target t2 in targets) {
						Debug.DrawLine (t.transform.position, t2.transform.position, Color.blue);
					}
					yield return new WaitForSeconds (1f);
					waypointUpdateQueued = false;
				}
			}
			yield return new WaitForSeconds (5f);
		}
	}
}
