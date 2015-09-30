using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemyTarget : MonoBehaviour {

	public static EnemyTarget instance;

	private List<Target> targets = new List<Target> ();
	private List<Target> players = new List<Target> ();

	public float maxTargetLifeTime = 30f;
	private System.Random random = new System.Random ();

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
		print ("Number of preset AI Waypoints: " + transform.childCount);
		for (int i = 0; i < transform.childCount; i++) {
			targets.Add (new Target (transform.GetChild (i)));
		}
		StartCoroutine ("UpdateLifeTimes");
		StartCoroutine ("UpdatePositions");
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

	public static void AddTarget (Transform t, bool isPlayer) {
		if (isPlayer) {
			instance.players.Add (new Target (t));
		} else {
			//targets.Add (new Target (t, true));
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
}
