	using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CameraFollow : MonoBehaviour {

	public bool followPlayer = false;
	public Vector3 cameraOffset = new Vector3 (0, 2, -8);
	public Vector3 dynamicOffset = Vector3.zero;
	public float cameraResponsiveness = 3f;
	public Vector3 defaultPosition = Vector3.zero;
	private Transform player;

	public float lookpointDistance = 1000f;
	private Vector3 lookPoint;
	Quaternion targetRotation;

	// Star animation stugg
	public float timeToWait = 0.2f;
	float transformTime = 2f;
	float transformTimeLeft;
	float waitLeft;

	private float startTime;
	private float journeyLength;
	bool spawnAnimation = false;


	InputController playerInput;
	
	// Use this for initialization
	void Start () {
		StartCoroutine ("FindFirstPlayer");
	}
	
	// Update is called once per frame
	void LateUpdate () {
		if (player != null && followPlayer) {
			dynamicOffset = new Vector3 (0, 0, cameraOffset.z * playerInput.InputVerical);
			lookPoint = player.transform.position + player.transform.forward * lookpointDistance;
			transform.position = player.transform.position + player.TransformDirection (cameraOffset + dynamicOffset);
			transform.LookAt (lookPoint);

			transform.rotation = player.rotation;
		} else {
			transform.position = defaultPosition;

			if (player != null) {

				if (spawnAnimation) {
					if  (waitLeft <= 0) {
						LerpCamera ();
					}
					waitLeft -= Time.deltaTime;
				} else {
					transform.LookAt (player.position);
				}
			}
		}
	}

	void OnLevelLoad () {
		followPlayer = false;
	}

	void OnStartGame () {
		lookPoint = player.position + player.transform.forward * lookpointDistance;
		waitLeft = timeToWait;
		transformTimeLeft = transformTime;
		spawnAnimation = true;
	}

	void LerpCamera () {
		transformTimeLeft -= Time.deltaTime;

		if (transformTimeLeft < 0) {
			transformTimeLeft = 0;
		}

		
		Vector3 targetPosition = player.transform.position + player.TransformDirection (cameraOffset);
		Vector3 joyrneyPoint = targetPosition - (targetPosition * transformTimeLeft /transformTime) /500;
		
		transform.position = Vector3.Lerp(transform.position, joyrneyPoint , 1 - Mathf.Pow (transformTimeLeft /transformTime, 6));
		transform.LookAt (Vector3.Lerp (player.position + player.transform.forward * lookpointDistance * (1 - (transformTimeLeft /transformTime) /4), 
		                                lookPoint,
		                                1 - transformTimeLeft /transformTime * 3));

		if (transformTimeLeft == 0) {
			followPlayer = true;
			spawnAnimation = false;
		}
	}


	IEnumerator FindFirstPlayer () {
		PlayerData p;
		while (true) {
			p = GameManager.instance.Players[0];
			if (p != null) {
				player = p.gameObject.transform;
				playerInput = player.GetComponent<InputController> ();
				break;
			}
			yield return new WaitForSeconds (0.1f);
		}
		Debug.Log ("First Player Found (" + player.name + ")");
	}
}
