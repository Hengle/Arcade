using UnityEngine;
using System.Collections;

public class HomingRocket : MonoBehaviour {

	Rigidbody rb;
	AITargetFinder ai;

	public float accelerationTime = 2f;
	public float flightSpeed = 200f;
	public float turnDamping = 1f;
	public float maxTurnAngle = 70f;

	[ShowOnlyAttribute]
	public float velocityZ;

	[ShowOnlyAttribute]
	public bool disabled = false;

	void Awake () {
		rb = GetComponent<Rigidbody> ();
		ai = GetComponent<AITargetFinder> ();
		StartCoroutine (AngleChecker ());
	}

	// Update is called once per frame
	void FixedUpdate () {
		if (!disabled) {
			if (ai.HasPriorityTarget) {
				Quaternion targetRotation = Quaternion.LookRotation (ai.priorityTarget.position - transform.position);
				transform.rotation = Quaternion.Slerp (transform.rotation, targetRotation, Time.fixedDeltaTime * turnDamping);
				
				GameManager.instance.respawnText.gameObject.SetActive (true);
				GameManager.instance.respawnText.text = "MISSILE\nLOCK ON!";
			}
		}


	
		rb.velocity += transform.forward * flightSpeed * Time.fixedDeltaTime / accelerationTime;

		if (rb.velocity.magnitude > flightSpeed) {
			rb.velocity = transform.forward * flightSpeed;
		}

		velocityZ = rb.velocity.magnitude;
	}

	void OnDestory () {
		GameManager.instance.respawnText.gameObject.SetActive (false);
	}

	IEnumerator AngleChecker () {
		while (true) {
			if (ai.HasPriorityTarget) {
				if (Vector3.Angle (transform.forward, ai.priorityTarget.transform.position - transform.position) > maxTurnAngle) {
					disabled = true;
					GameManager.instance.respawnText.gameObject.SetActive (false);

				}
			}
			yield return new WaitForSeconds (0.1f);
		}
		yield return null;
	}
}
