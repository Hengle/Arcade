 using UnityEngine;
using System.Collections;

public class AIBehaviourAttack : MonoBehaviour, IBehaviour {

	AICore ai;
	IMovementController movementController;
	
	public float turnDamping = 1f;
	public float maxTurnAngle = 120f;

	bool active;

	[SerializeField] [ShowOnlyAttribute]
	private bool passed = false;
	public float attackBreakDistance = 120f;
	public float reengageDistance = 200f;

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
		if (passed) {
			if (ai.GetTargetFinder.distanceToPrimary > reengageDistance) {
				passed = false;
			}
		} else {
			if (ai.GetTargetFinder.distanceToPrimary < attackBreakDistance) {
				passed = true;
			}
		}

		if (!passed) {
			Quaternion targetRotation = Quaternion.LookRotation (ai.GetTargetFinder.priorityTarget.position - transform.position);
			transform.rotation = Quaternion.Slerp (transform.rotation, targetRotation, Time.fixedDeltaTime * turnDamping);


		} else {

		}
	}


	IEnumerator AngleChecker () {
		while (true) {
			if (ai.GetTargetFinder.HasPriorityTarget) {
				if (Vector3.Angle (transform.forward, ai.GetTargetFinder.priorityTarget.position - transform.position) > maxTurnAngle) {
					passed = true;
					GameManager.instance.informationText.gameObject.SetActive (false);
					
				}
			}
			yield return new WaitForSeconds (0.1f);
		}
		yield return null;
	}
}
