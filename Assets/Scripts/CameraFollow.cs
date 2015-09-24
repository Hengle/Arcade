using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CameraFollow : MonoBehaviour {

	public Vector3 cameraOffset;
	private Transform player;

	// Use this for initialization
	void Start () {
		StartCoroutine ("FindFirstPlayer");
	}
	
	// Update is called once per frame
	void LateUpdate () {
		if (player != null) {
			transform.position = player.transform.InverseTransformVector (player.transform.position) + cameraOffset;
			transform.LookAt (player.transform.position);
		}
	}

	IEnumerator FindFirstPlayer () {
		PlayerData p;
		while (true) {
			p = GameManager.players[0];
			if (p != null) {
				player = p.gameObject.transform;
				break;
			}
			yield return new WaitForSeconds (0.1f);
		}
		Debug.Log ("First Player Found (" + player.name + ")");
	}
}
