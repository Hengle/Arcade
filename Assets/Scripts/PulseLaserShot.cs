using UnityEngine;
using System.Collections;

public class PulseLaserShot : MonoBehaviour {

	[SerializeField]
	private float range = 200f;

	public LineRenderer lr;
	RaycastHit hitInfo;
	Vector3 hitPoint;

	void Start () {
		if (Physics.Raycast (transform.position, transform.forward, out hitInfo, range)) {
			hitPoint = hitInfo.point;
		} else {
			hitPoint = transform.forward * range;
		}

		lr.SetPosition (0, transform.position);
		lr.SetPosition (1, hitPoint);
	}
}
