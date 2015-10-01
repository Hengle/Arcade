using UnityEngine;
using System.Collections;

public class MovementControllerSpace : MonoBehaviour, IMovementController, IPausable {

	// References
	private Rigidbody rb;

	public bool is3D = false;

	// Movement Values
	[SerializeField]
	private float maxVelocity = 3f;
	[SerializeField]
	private float accelerationForce = 2f;
	[SerializeField]
	private Vector3 rotationForce = new Vector3 (0.0f, 100f, 0.0f);
	[SerializeField]
	private float tilt = 4f;
	
	private Vector3 moveInput = Vector3.zero;
	private Vector3 rotVector = Vector3.zero;
	Quaternion deltaRot;

	private Boundary boundary;
	
	// Use this for initialization
	void Start () {
		rb = GetComponent<Rigidbody> ();
		boundary = WorldManager.instance.WorldBorder;
	}

	// Update is called once per frame
	void FixedUpdate () {
		float velocityMagnitude = rb.velocity.magnitude;
		float angle;

		rb.AddForce (transform.forward * moveInput.z * accelerationForce);
		rb.AddForce (transform.up * moveInput.y * accelerationForce * 0.7f);
		rb.AddForce (transform.right * moveInput.x * accelerationForce * 0.7f);

		rb.AddTorque (transform.forward * moveInput.x * -rotationForce.z);

		rb.AddTorque (transform.up * rotVector.x * rotationForce.x);
		rb.AddTorque (transform.right * rotVector.y * -rotationForce.y);
	
		if (velocityMagnitude > maxVelocity) {
			rb.velocity = rb.velocity.normalized * maxVelocity;
			
		} else if (velocityMagnitude < maxVelocity * -0.7f) {
			rb.velocity = rb.velocity.normalized * maxVelocity * 0.7f;
		}
		
		//transform.rotation = Quaternion.Euler (transform.rotation.x, transform.rotation.y, rb.velocity.x * -tilt);

		rb.position.Set
		(
			Mathf.Clamp (rb.position.x, boundary.xMin, boundary.xMax), 
			0.0f, 
			Mathf.Clamp (rb.position.z, boundary.zMin, boundary.zMax)
		);
	}

	void OnPauseGame () {

	}

	public void SetMovementVector (Vector3 vector) {
		moveInput = vector;
	}

	public void SetRotationVector (Vector3 vector) {
		rotVector = vector;
	}
}
