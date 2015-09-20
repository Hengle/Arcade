using UnityEngine;
using System.Collections;

public class ExplosiveDevice : MonoBehaviour, IDamaging {

	[SerializeField]
	private DamageProfile damageProfile;
	private bool hit = false;
	public float fallOff = 0.4f;

	void OnTriggerEnter (Collider other) {
		IDamageable canDamage = other.GetComponent<IDamageable> ();
	
		if (canDamage != null) {
			canDamage.DamageWithFallOff (this.damageProfile, Vector3.Distance (transform.position, other.transform.position) * fallOff);
		}
	}

	void FixedUpdate () {
		if (!hit) {
			hit = true;
		} else {
			GetComponent<SphereCollider> ().enabled = false;
		}
	}

	public void SetDamageProfile (DamageProfile dp) {
		this.damageProfile = dp;
	}
}
