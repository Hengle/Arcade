	using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CameraFollow : MonoBehaviour {

	public Vector3 cameraOffset;
	public Vector3 defaultPosition = Vector3.zero;
	private Transform player;

	public float lookpointDistance = 1000f;
	private Vector3 lookPoint;

	// Use this for initialization
	void Start () {
		StartCoroutine ("FindFirstPlayer");
	}
	
	// Update is called once per frame
	void LateUpdate () {
		if (player != null) {
			lookPoint = player.transform.position + player.transform.forward * lookpointDistance;
			transform.position = player.transform.position + player.TransformDirection (cameraOffset);
			transform.LookAt (lookPoint);
		} else {
			transform.position = defaultPosition;
		}
	}

	IEnumerator FindFirstPlayer () {
		PlayerData p;
		while (true) {
			p = GameManager.instance.Players[0];
			if (p != null) {
				player = p.gameObject.transform;
				break;
			}
			yield return new WaitForSeconds (0.1f);
		}
		Debug.Log ("First Player Found (" + player.name + ")");
	}
}
