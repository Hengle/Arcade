using UnityEngine;
using System.Collections;

public class TargetIndicator : MonoBehaviour {

	public float markerDuration = 10f;
	float markerTimeLeft = -1f;

	Transform mainCamera;
	Transform targetMarker;
	bool recentlyMarked = false;
	AITargetFinder targeter;

	public bool AlreadyMarked {
		get {return recentlyMarked;}
	}

	void Start () {
		targeter = GetComponent<AITargetFinder> ();
		targetMarker = transform.FindChild ("TargetMarker");
		targetMarker.gameObject.SetActive (false);
		mainCamera = GameObject.FindWithTag ("MainCamera").transform;

		StartCoroutine (CheckForMarking ());
	}

	void ActivateMarker () {
		if (!recentlyMarked) {
			print ("Activating marker");
			targetMarker.gameObject.SetActive (true);
			markerTimeLeft = markerDuration;
			recentlyMarked = true;
		}
	}

	void Update () {
		if (markerTimeLeft > 0) {
			markerTimeLeft -= Time.deltaTime;
			targetMarker.transform.LookAt (transform.position - mainCamera.transform.forward * 100f);
		} else {
			targetMarker.gameObject.SetActive (false);
			recentlyMarked = false;
		}
	}

	IEnumerator CheckForMarking () {
		while (true) {
			if (targeter.HasPriorityTarget && !recentlyMarked) {
				ActivateMarker ();
			}
			yield return new WaitForSeconds (0.1f);
		}
		yield return null;
	}
}
