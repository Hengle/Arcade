using UnityEngine;
using System.Collections;

[RequireComponent (typeof(Rigidbody))]
public class MovementController : MonoBehaviour, IMovementController {

	// References
	Rigidbody rb;

	// Movement Values
	public float maxVelocity = 3f;
	public float accelerationForce = 36000f;
	public float rotationSpeed = 0.4f;

	[HideInInspector]
	public Vector3 moveInput = Vector3.zero;

	void Start () {
		rb = GetComponent<Rigidbody> ();
	}

	void FixedUpdate () {

		// Set Rotation
		if (moveInput.x > 0.1 || moveInput.x < -0.1) {
			rb.rotation = Quaternion.Euler (new Vector3 (0, transform.rotation.eulerAngles.y + rotationSpeed * moveInput.x, 0));
		}

		// Set velocity
		rb.velocity = moveInput.z * transform.forward * accelerationForce;

		if (rb.velocity.z > maxVelocity || rb.velocity.z < maxVelocity * -0.7f) {
			rb.velocity = rb.velocity.normalized * maxVelocity;
		}
	}
	
	public void SetMovementVector (Vector3 vector) {
		moveInput = vector;
	}
	
	public void SetRotationVector (Vector3 vector) {
		
	}
}
