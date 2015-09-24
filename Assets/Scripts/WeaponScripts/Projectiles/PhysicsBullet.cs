using UnityEngine;
using System.Collections;

public class PhysicsBullet : MonoBehaviour, IDamaging {

	[SerializeField]
	private float launchForce;

	[SerializeField]
	private Rigidbody rb;
	[SerializeField]
	private CapsuleCollider col;
	[SerializeField]
	private DamageProfile damageProfile = new DamageProfile ();

	public bool hasHitEffect = false;
	public Transform hitEffect;
	public bool hasSecondaryDamage;

	private Collider ignored;

	void Start () {
		rb.AddForce (transform.forward * launchForce);
		Debug.Log ("Piercing Damage: " + damageProfile.PiercingDamage);
	}

	void OnTriggerEnter (Collider other) {
		if (ignored == null) {
			ignored = other;
		} else {
			GameObject go;
			
			Debug.Log (this.name + " hit " + other.name);
			
			IDamageable canDamage = other.GetComponent<IDamageable> ();
			if (canDamage != null) {
				canDamage.Damage (this.damageProfile);
			} else {
				Debug.Log ("Target appears to be undamageable");
			}
			
			if (hasHitEffect) {
				Instantiate (hitEffect, this.transform.position, Quaternion.identity);
			}
			
			Destroy (this.gameObject);
		}
	}

	void OnTriggerExit (Collider other) {
		if (other == ignored) {
			col.enabled = true;
		}
	}

	public void SetDamageProfile (DamageProfile dp){

	}
}
