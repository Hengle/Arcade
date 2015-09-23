using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemyTarget : MonoBehaviour{

	public static EnemyTarget instance;

	public List<Target> targets = new List<Target> ();

	public float maxTargetLifeTime = 30f;


	void Awake () {
		if (instance == null) {
			instance = this;
		} else {
			Destroy (this.gameObject);
		}
	}

	void Start () {
		StartCoroutine ("UpdateLifeTimes");
	}

	public static Transform GetNewRandomTarget () {
		int i = (int) Random.value * instance.targets.Count;
		return instance.targets[i].tagetObject;
	}

	IEnumerator UpdateLifeTimes () {
		while (true) {
			foreach (Target t in instance.targets) {
				if (!t.persistent) {
					t.lifeTime -= Time.deltaTime;
					if (t.lifeTime <= 0) {
						instance.targets.Remove (t);
					}
				}
			}
			yield return new WaitForSeconds (0.5f);
		}
	}

	public class Target {
		public Transform tagetObject;
		public float lifeTime;
		public bool persistent;
	}
}
