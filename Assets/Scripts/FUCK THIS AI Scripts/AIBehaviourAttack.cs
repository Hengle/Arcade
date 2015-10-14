 using UnityEngine;
using System.Collections;

public class AIBehaviourAttack : MonoBehaviour, IBehaviour {

	AICore ai;
	IMovementController movementController;
	public bool drawWeaponTargeting = false;

	[Header ("Manouvering")]
	public float turnDamping = 1f;
	public float maxTurnAngle = 120f;

	bool active;
	bool paused = false;
	bool withinAttackAngle = false;

	[SerializeField] [ShowOnlyAttribute]
	private bool passed = false;
	[Header ("Combat Profile")]
	public float attackBreakDistance = 120f;
	public float reengageDistance = 200f;
	public float attackAngle = 20f;

	private static string _name = "Attack";
	public string Name {
		get {return _name;}
	}
	
	private bool done = false;
	public bool IsDone {
		get {return done;}
	}

	void Awake () {
		ai = GetComponent<AICore> ();
		movementController = GetComponent<IMovementController> ();
	}

	public void StartBehaviour () {
		enabled = true;
		print ("STARTING ATTACK BEHAVIOUR FOR " + transform.name);
	}

	public void EndBehaviour () {
		enabled = false;
	}

	void FixedUpdate () {
		if (paused) {
			return;
		}

		if (passed) {
			if (ai.GetTargetFinder.distanceToPrimary > reengageDistance /2) {
				passed = false;
			}
		} else {
			if (ai.GetTargetFinder.distanceToPrimary < attackBreakDistance) {
				passed = true;
			}
		}
		
		if (!passed) {
			if (withinAttackAngle) {
				transform.LookAt (ai.GetTargetFinder.priorityTarget.position);
			} else {
				Quaternion targetRotation = Quaternion.LookRotation (ai.GetTargetFinder.priorityTarget.position - transform.position);
				transform.rotation = Quaternion.Slerp (transform.rotation, targetRotation, Time.fixedDeltaTime * turnDamping);
			}
		} else {
			
		}
	}

	void Update () {
		if (withinAttackAngle) {
			ai.GetWeaponController.FirePrimary ();

			if (ai.GetWeaponController != null) {
				print ("[" + transform.name + "] SHOULD BE NOW FIRING");
				ai.GetWeaponController.FirePrimary ();
			}
		}
	}

	void OnDrawGizmos () {
		if (drawWeaponTargeting) {
			Debug.DrawRay (transform.position, transform.forward * 100, Color.red);
		}
	}

	void OnPauseGame () {
		paused = true;
	}

	void OnResumeGame () {
		paused = false;
	}

	IEnumerator AngleChecker () {
		while (true) {
			if (ai.GetTargetFinder.HasPriorityTarget) {
				float angle = Vector3.Angle (transform.forward, ai.GetTargetFinder.priorityTarget.position - transform.position);
				if (angle > maxTurnAngle) {
					passed = true;
					GameManager.instance.informationText.gameObject.SetActive (false);	
				}
				withinAttackAngle = false;
				if (!passed) {
					if (angle < attackAngle) {
						withinAttackAngle = true;
					}
				}
			}
			yield return new WaitForSeconds (0.1f);
		}
		yield return null;
	}
}
