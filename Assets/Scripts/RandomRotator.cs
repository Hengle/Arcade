using UnityEngine;
using System.Collections;

public class RandomRotator : MonoBehaviour 
{
	public float tumble;
	bool paused;
	Vector3 savedRotationVelocity;
	Vector3 savedVelocity;
	Rigidbody rb;

	void Awake () {
		rb = GetComponent<Rigidbody> ();
	}

	void Start () {
		rb.angularVelocity = Random.insideUnitSphere * tumble;
	}

	void OnPauseGame () {
		savedRotationVelocity = rb.angularVelocity;
		savedVelocity = rb.velocity;
		rb.angularVelocity = Vector3.zero;
		rb.velocity = Vector3.zero;

		paused = true;
	}

	void OnResumeGame () {
		rb.velocity = savedVelocity;
		rb.angularVelocity = savedRotationVelocity;
		paused = false;
	}
}