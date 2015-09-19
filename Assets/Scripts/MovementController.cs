using UnityEngine;
using System.Collections;

[RequireComponent (typeof(Rigidbody))]
public class MovementController : MonoBehaviour {

	// References
	Rigidbody rb;

	// Movement Values
	public float maxVelocity = 3f;
	public float accelerationForce = 360000f;
	public float rotationSpeed = 0.8f;

	[HideInInspector]
	public Vector3 moveInput = Vector3.zero;

	// Use this for initialization
	void Start () {
		rb = GetComponent<Rigidbody> ();
	}

	// Update is called once per frame
	void FixedUpdate () {
		rb.AddForce (moveInput.z * transform.forward * accelerationForce);

		if (moveInput.x > 0.1 || moveInput.x < -0.1) {
			rb.rotation = Quaternion.Euler (new Vector3 (0, transform.rotation.eulerAngles.y + rotationSpeed * moveInput.x, 0));
		}

		if (rb.velocity.z > maxVelocity || rb.velocity.z < maxVelocity * -0.7f) {
			rb.velocity = rb.velocity.normalized * maxVelocity;
		}
		Debug.Log (rb.velocity);
	}
}
