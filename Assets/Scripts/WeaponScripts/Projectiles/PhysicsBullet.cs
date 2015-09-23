using UnityEngine;
using System.Collections;

public class PhysicsBullet : MonoBehaviour, IDamaging {

	[SerializeField]
	private float launchForce;

	[SerializeField]
	private Rigidbody rb;
	public Transform hitEffect;
	public bool hasSecondaryDamage;


	[SerializeField]
	private DamageProfile damageProfile = new DamageProfile ();

	
	// Use this for initialization
	void Start () {
		rb.AddForce (transform.forward * launchForce);
		Debug.Log ("Piercing Damage: " + damageProfile.PiercingDamage);
	}

	void OnTriggerEnter (Collider other) {
		GameObject go;

		Debug.Log (this.name + " hit " + other.name);

		IDamageable canDamage = other.GetComponent<IDamageable> ();
		if (canDamage != null) {
			canDamage.Damage (this.damageProfile);
		} else {
			Debug.Log ("Target appears to be undamageable");
		}

		Instantiate (hitEffect, this.transform.position, Quaternion.identity);

		Destroy (this.gameObject);
	}

	public void SetDamageProfile (DamageProfile dp){

	}
}
