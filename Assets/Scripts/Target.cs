using UnityEngine;
using System.Collections;

[System.Serializable]
public class Target : ITarget {
	
	public Transform transform;
	public float lifeTime;
	public bool persistent;
	public bool dynamic;
	public Vector3 position;
	
	public Target (Transform t) {
		transform = t;
		persistent = true;
		dynamic = false;
		position = t.position;
	}

	public Target (Transform t, bool b, float l) {
		transform = t;
		persistent = b;
		lifeTime = l;
		dynamic = false;
		position = t.position;
	}

}
