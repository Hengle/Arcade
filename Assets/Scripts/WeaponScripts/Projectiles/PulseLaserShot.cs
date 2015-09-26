using UnityEngine;
using System.Collections;

public class PulseLaserShot : MonoBehaviour {

	[SerializeField]
	private float range = 800f;
	[SerializeField]
	private DamageProfile dm = new DamageProfile ();
	private LineRenderer lr;

	Ray ray;
	RaycastHit hitInfo;
	Vector3 hitPoint;
	
	void Start () {
		lr = GetComponent<LineRenderer> ();
		ray = new Ray (transform.position, transform.forward);

		lr.SetPosition(0, transform.position);
		if (Physics.Raycast (ray, out hitInfo, range)) {
			Debug.Log (hitInfo.transform.name);

			if (hitInfo.transform.GetComponent<HealthManager> ()) {
				hitInfo.transform.GetComponent<HealthManager> ().Damage (dm);
			}
			lr.SetPosition(1, hitInfo.point);
		} else {
			lr.SetPosition (1, transform.position + transform.forward * range);
		}
	}


}
