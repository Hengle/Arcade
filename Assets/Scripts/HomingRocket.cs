using UnityEngine;
using System.Collections;

public class HomingRocket : MonoBehaviour {

	Rigidbody rb;

	public float accelerationTime = 2f;
	public float flightSpeed = 200f;
	public float armTime = 1f;

	public DamageProfile damageProfile;
	public Transform hitEffect;

	[ShowOnlyAttribute]
	public float velocityZ;
	[SerializeField][ShowOnlyAttribute]
	bool armed = false;

	void Awake () {
		rb = GetComponent<Rigidbody> ();
	}

	// Update is called once per frame
	void FixedUpdate () {
		rb.velocity += transform.forward * flightSpeed * Time.fixedDeltaTime / accelerationTime;

		if (rb.velocity.magnitude > flightSpeed) {
			rb.velocity = transform.forward * flightSpeed;
		}

		velocityZ = rb.velocity.magnitude;
	}

	void Update () {
		if (armTime > 0) {
			armTime -= Time.deltaTime;
		} else {
			armed = true;
		}
	}

	void OnTriggerEnter (Collider other) {
		if (armed) {
			IDamageable canDamage = other.GetComponent<IDamageable> ();
			
			if (canDamage != null) {
				canDamage.Damage (damageProfile);
			}
			
			if (hitEffect != null) {
				Instantiate (hitEffect, transform.position, Quaternion.identity);
			}
			
			Destroy (this.gameObject);
		}
	}
}
