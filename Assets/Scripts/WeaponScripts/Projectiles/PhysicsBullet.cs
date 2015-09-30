using UnityEngine;
using System.Collections;

public class PhysicsBullet : MonoBehaviour, IDamaging {

	[SerializeField]
	private float launchForce;

	private Rigidbody rb;
	private CapsuleCollider col;
	private LineRenderer lr;
	[SerializeField]
	private DamageProfile damageProfile = new DamageProfile ();

	public bool hasHitEffect = false;
	public Transform hitEffect;
	public bool hasSecondaryDamage;

	private Collider ignored;
	private bool hitScanned = false;
	private RaycastHit hit;


	void Awake () {
		rb = GetComponent<Rigidbody> ();
		col = GetComponent<CapsuleCollider> ();
		lr = GetComponent<LineRenderer> ();
	}

	void Start () {
		rb.AddForce (transform.forward * launchForce);
		Debug.Log ("Piercing Damage: " + damageProfile.PiercingDamage);
	}

	void Update () {
		lr.SetPosition (0, transform.position);
		lr.SetPosition (1, transform.position - transform.forward * (rb.velocity.z < 0 ? -rb.velocity.z /10 : rb.velocity.z /10));
	}

	void FixedUpdate () {
		if (!hitScanned) {
			Ray ray = new Ray (transform.position, transform.forward);
			Physics.Raycast (ray, out hit, rb.velocity.z /2);

		} else {
			transform.position = hit.point;
			OnTriggerEnter (hit.collider);
		}
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
				Instantiate (hitEffect, transform.position - transform.forward /2, Quaternion.identity);
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
