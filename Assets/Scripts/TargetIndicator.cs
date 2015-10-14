using UnityEngine;
using System.Collections;

public class TargetIndicator : MonoBehaviour {

	Transform player;
	bool show = false;
	public float defaultColor;
	public float damageColor;

	GameObject targetMarker;

	void Start () {
		targetMarker = transform.GetChild (0).gameObject;
		targetMarker.SetActive (false);
	}

	void Update () {
		if (show) {
			if (player != null) {
				transform.LookAt (player.position);
			} else {
				player = GameObject.FindWithTag ("Player").transform;
			}
		}
	}

	void ShowTargeter () {
		targetMarker.SetActive (true);
		show = true;
	}

	void HideTargeter () {
		targetMarker.SetActive (false);
		show = false;
	}
}
