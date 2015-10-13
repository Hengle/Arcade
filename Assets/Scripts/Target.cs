using UnityEngine;
using System.Collections;

[System.Serializable]
public class Target : ITarget {
	
	public Transform transform;
	[HideInInspector]
	public float lifeTime;
	[HideInInspector]
	public bool persistent;
	[HideInInspector]
	public bool dynamic;
	public Vector3 position;

	public Target (Transform t) {
		transform = t;
		persistent = true;
		dynamic = false;
		position = t.position;
	}

	public Target (Transform t, Vector3 pos, bool b, float l) {
		transform = t;
		persistent = b;
		lifeTime = l;
		dynamic = false;
		position = pos;
	}

	public Target (Transform t, bool b, float l) {
		transform = t;
		persistent = b;
		lifeTime = l;
		dynamic = false;
		position = t.position;
	}

}
