using UnityEngine;
using System.Collections;

public class ExplosiveDevice : MonoBehaviour, IDamaging {

	[SerializeField]
	private DamageProfile damageProfile;
	private bool hit = false;
	public float fallOff = 0.4f;

	public Transform hitEffect;

	void OnTriggerEnter (Collider other) {
		IDamageable canDamage = other.GetComponent<IDamageable> ();
	
		if (canDamage != null) {
			canDamage.DamageWithFallOff (this.damageProfile, Vector3.Distance (transform.position, other.transform.position) * fallOff);
		}
		if (hitEffect != null) {
			Instantiate (hitEffect, transform.position, Quaternion.identity);
		}
		Destroy (this.gameObject);
	}

	public void SetDamageProfile (DamageProfile dp) {
		this.damageProfile = dp;
	}
}
