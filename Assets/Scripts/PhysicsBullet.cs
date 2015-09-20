using UnityEngine;
using System.Collections;

public class PhysicsBullet : MonoBehaviour {

	[SerializeField]
	private float launchForce;

	[SerializeField]
	private Rigidbody rb;

	// Use this for initialization
	void Start () {
		rb.AddForce (transform.forward * launchForce);
	}
}
