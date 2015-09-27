using UnityEngine;
using System.Collections;

public class PulseLaserShot : MonoBehaviour {

	[SerializeField]
	private float range = 800f;
	[SerializeField]
	private DamageProfile damageProfile = new DamageProfile ();
	[SerializeField]
	private Transform hitEffect;
	private LineRenderer lr;

	private Ray ray;
	private RaycastHit hitInfo;
	private Vector3 hitPoint;
	
	void Start () {
		lr = GetComponent<LineRenderer> ();
		ray = new Ray (transform.position, transform.forward);

		lr.SetPosition(0, transform.position);
		if (Physics.Raycast (ray, out hitInfo, range)) {
			Debug.Log (hitInfo.transform.name);

			if (hitInfo.transform.GetComponent<HealthManager> ()) {
				hitInfo.transform.GetComponent<HealthManager> ().Damage (damageProfile);
			}
			lr.SetPosition(1, hitInfo.point);
			Instantiate (hitEffect, hitInfo.point, Quaternion.identity);
		} else {
			lr.SetPosition (1, transform.position + transform.forward * range);
		}
	}


}
