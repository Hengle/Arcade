using UnityEngine;
using System.Collections;

public class TargetIndicator : MonoBehaviour {

	public Transform targetMarker;
	public float markerDuration = 10f;
	float markerTimeLeft = -1f;

	Transform mainCamera;

	void Start () {
		targetMarker.gameObject.SetActive (false);
		mainCamera = GameObject.FindWithTag ("MainCamera").transform;
	}

	void ActivateMarker () {
		targetMarker.gameObject.SetActive (true);
		markerDuration = markerDuration;
	}

	void Update () {
		if (markerTimeLeft > 0) {
			markerTimeLeft -= Time.deltaTime;
			targetMarker.transform.LookAt (mainCamera.position);
			targetMarker.transform.rotation = transform.rotation * Quaternion.Euler (0, 180f, 0);
		} else {
			targetMarker.gameObject.SetActive (false);
		}
	}
}
