using UnityEngine;
using System.Collections;

public class RoateToRoot : MonoBehaviour {

	private Transform rootObject;

	void Start () {
		rootObject = transform.root;
	}

	void Update () {
		transform.rotation = rootObject.rotation;
	}
}
