using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AimAssist : MonoBehaviour {

	Vector3 lookPoint;
	public float maxHitAngle = 5f;

	void Start () {
		lookPoint = transform.root.position + transform.root.forward * 2000f;
	}

	void FixedUpdate () {
		Ray ray = new Ray (transform.position + transform.forward *10f, transform.root.forward);
		RaycastHit [] hits = Physics.SphereCastAll (ray, 20f, 3000f);

		RaycastHit closestTarget;
		float closestAngle = maxHitAngle;
		lookPoint = transform.root.position + transform.root.forward * 2000f;

		foreach (RaycastHit hit in hits) {
			if (hit.transform.tag.Equals ("Enemy")) {
				float angle = Vector3.Angle (transform.root.forward, hit.point - transform.position);

				if (angle < maxHitAngle && angle < closestAngle) {
					closestTarget = hit;
					closestAngle = angle;
					lookPoint = hit.point;
				}
			}
		}
		transform.LookAt (lookPoint);
	}

	void OnDrawGizmos () {
		Debug.DrawRay (transform.position, transform.root.forward * 200f, Color.green);
		Debug.DrawLine (transform.root.position, lookPoint, Color.cyan);
	}
}
