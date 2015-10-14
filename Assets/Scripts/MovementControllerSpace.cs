using UnityEngine;
using System;
using System.Collections;

public class MovementControllerSpace : MonoBehaviour, IMovementController, IPausable {

	// References
	private Rigidbody rb;
	
	// Movement Values
	[SerializeField]
	private float maxVelocity = 3f;
	[SerializeField]
	private float accelerationForce = 2f;
	[SerializeField]
	private Vector3 rotationForce = new Vector3 (0.0f, 100f, 0.0f);
	
	private Vector3 moveInput = Vector3.zero;
	private Vector3 rotVector = Vector3.zero;
	Quaternion deltaRot;

	private Boundary boundary;
	private bool paused = false;

	Vector3 savedVelocity = Vector3.zero;
	Vector3 savedRotationVelocity = Vector3.zero;

	
	// Use this for initialization
	void Start () {
		rb = GetComponent<Rigidbody> ();
		boundary = WorldManager.instance.WorldBorder;
	}

	// Update is called once per frame
	void FixedUpdate () {
		if (!paused) {
			rb.AddForce (transform.forward * moveInput.z * accelerationForce);
			rb.AddForce (transform.up * moveInput.y * accelerationForce * 0.7f);
			rb.AddForce (transform.right * moveInput.x * accelerationForce * 0.7f);
			
			rb.AddTorque (transform.forward * moveInput.x * -rotationForce.z);
			
			rb.AddTorque (transform.up * rotVector.x * rotationForce.x);
			rb.AddTorque (transform.right * rotVector.y * -rotationForce.y);
			
			float velocityMagnitude = rb.velocity.magnitude;
			
			if (velocityMagnitude > maxVelocity) {
				rb.velocity = rb.velocity.normalized * maxVelocity;
				
			} else if (velocityMagnitude < maxVelocity * -0.7f) {
				rb.velocity = rb.velocity.normalized * maxVelocity * -0.7f;
			}
		}
	}

	void OnPauseGame () {
		savedVelocity = rb.velocity;
		savedRotationVelocity = rb.angularVelocity;
		rb.velocity = Vector3.zero;
		rb.angularVelocity = Vector3.zero;
		paused = true;
	}

	void OnResumeGame () {
		rb.velocity = savedVelocity;
		rb.angularVelocity = savedRotationVelocity;
		paused = false;
	}

	public void SetMovement (Vector3 vector) {
		moveInput = vector;
	}

	public void SetRotation (Vector3 vector) {
		rotVector = vector;
	}

	public void SetMovementAdditive (Vector3 vector) {
		moveInput.x += vector.x;
		moveInput.y += vector.y;
		moveInput.z += vector.z;
	}

	public void SetRotationAdditive (Vector3 vector) {
		rotVector += vector;
	}
}
