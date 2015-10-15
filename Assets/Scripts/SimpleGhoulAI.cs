using UnityEngine;
using System.Collections.Generic;
using System.Collections;

[RequireComponent (typeof (IMovementController))]
public class SimpleGhoulAI : MonoBehaviour {

	// References
	IMovementController movement;
	Rigidbody rb;
	AIObstacleAvoidance avoider;
	AITargetFinder targetFinder;
	NPCWeaponManager weapons;

	// Other stuff
	[SerializeField] [ShowOnlyAttribute]
	bool paused = false;

	// Movement
	[Header ("Movement")] 
	public float turnDamping = 3f;
	[SerializeField] [ShowOnlyAttribute]
	Vector3 movementVector = Vector3.zero;
	[SerializeField] [ShowOnlyAttribute]
	Vector3 velocity = Vector3.zero;
	[SerializeField] [ShowOnlyAttribute]
	float velocityMagnitude = 0f;

	[Header ("Patroling")]
	public bool showPatrolRoute = false;
	public Range numTargetsToPatrol = new Range (3, 5);
	
	private List<Target> targetsToPatrol;
	private IMovementController movementController;
	[SerializeField]
	private Target currentTarget = null;

	private static int maxAIUpdatesPerFrame = 5;
	private static int updatesUsed = 0;

	bool doneCurretBehaviour = false;

	void Awake () {
		movement = GetComponent<IMovementController> ();
		avoider = GetComponent<AIObstacleAvoidance> ();
		rb = GetComponent<Rigidbody> ();
		targetFinder = GetComponent<AITargetFinder> ();
		targetsToPatrol = new List<Target> ();
		weapons = GetComponent<NPCWeaponManager> ();
	}

	void Start () {
		movementVector = new Vector3 (0, 0, 1);
		StartCoroutine (AIUpdate ());
	}

	void UpdateMovement () {
		movementVector = new Vector3 (0, 0, 0);
		movementVector += transform.InverseTransformVector (avoider.movementVector);

		float magnitude = movementVector.magnitude;
		if (magnitude > 1f) {
			movementVector.Normalize ();
		} else if (magnitude < 0.1f) {
			movementVector = new Vector3 (0, 0, 1);
		}
		movement.SetMovement (movementVector);
	}

	void Patrol () {
		if (!doneCurretBehaviour) {
			if (targetsToPatrol.Count > 0) {
				
			} else {
				for (int i = 0; i < 5; i++) {
					targetsToPatrol.Add (PathfindingManager.instance.GetNewRandomTarget ());
				}
				
				currentTarget = targetsToPatrol[0];
			}
		}
	}

	void OnTriggerEnter (Collider other) {
		if (other.transform.Equals (currentTarget.transform)) {
			targetsToPatrol.RemoveAt (0);
			
			if (targetsToPatrol.Count > 0) {
				currentTarget = targetsToPatrol[0];
			} else {
				doneCurretBehaviour = true;
			}
		}
	}

	void OnDrawGizmos () {
		if (showPatrolRoute && currentTarget != null) {
			if (targetsToPatrol.Count > 1) {
				for (int i = -1; i < targetsToPatrol.Count -1; i++) {
					Debug.DrawLine ((i == -1) ? transform.position : targetsToPatrol[i].position, targetsToPatrol[i+1].position, Color.green);
				}
			}
		}
	}

	void OnPauseGame () {
		paused = true;
	}

	void OnResumeGame () {
		paused = false;
	}

	[System.Serializable]
	public class Range {
		public int min, max;
		
		public Range (int _min, int _max) {
			min = _min;
			max = _max;
		}
	}

	void LateUpdate () {
		updatesUsed = 0;
	}

	IEnumerator AIUpdate () {
		while (true) {
			if (paused || updatesUsed >= maxAIUpdatesPerFrame) {
				yield return new WaitForSeconds (0.1f);
			} else {
				updatesUsed++;
				velocityMagnitude = rb.velocity.magnitude;
				
				if (targetFinder.HasPriorityTarget) {
					float angle = Vector3.Angle (transform.forward, targetFinder.priorityTarget.position - transform.position);

					if (angle > 15f) {
						Quaternion targetRotation = Quaternion.LookRotation (targetFinder.priorityTarget.position - transform.position);
						transform.rotation = Quaternion.Slerp (transform.rotation, targetRotation, Time.fixedDeltaTime * turnDamping);
					} else {
						transform.LookAt (targetFinder.priorityTarget.position);
						weapons.Fire ();
					}
				} else {
					Patrol ();
					
					Quaternion targetRotation = Quaternion.LookRotation (currentTarget.position - transform.position);
					transform.rotation = Quaternion.Slerp (transform.rotation, targetRotation, Time.fixedDeltaTime * turnDamping);
				}
				
				UpdateMovement ();
			}
			yield return new WaitForSeconds (0.1f);
		}
	}
}
