using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AICore : MonoBehaviour {


	public enum BehaviourType {PASSIVE, DEFENSIVE, OFFENSIVE}

	// References
	private Rigidbody rb;
	private IMovementController mc;

	// avoidance
	[Header ("Obstacle Avoidance")]
	public bool avoidObstacles;
	[Range (0.01f, 1f)]
	public float avoidanceCheckInterval = 0.1f;
	public float avoidanceCheckDistance = 80f;
	public float minumumAvoidanceDistance = 10f;
	public float proximityCheckDistance = 30f;
	public float frontCheckRadius = 10f;
	public float avoidanceForce = 10000f;

	private Vector3 avoidanceVector = Vector3.zero;

	[Header ("Behaviour")]
	private List<IBehaviour> availableBehaviours = new List<IBehaviour> ();
	private AITargetFinder targetFinder;
	private IBehaviour currentBehaviour;
	[ShowOnlyAttribute]
	public string activeBehaviourName;
	[HideInInspector]
	private WeaponController weaponController;
	public WeaponController GetWeaponController {
		get {return weaponController;}
	}

	private bool inSquad = false;
	public bool updateBehaviour = false;

	public AITargetFinder GetTargetFinder {
		get {return targetFinder;}
	}
	
	[SerializeField][Range (1, 1000)]
	private float aggressionRange = 500f;
	bool priorityOverride = false;
	bool paused = false;

	Vector3 velocity = Vector3.zero;
	Vector3 angVelocity = Vector3.zero;

	public float AggressionRange {
		get {return aggressionRange;}
	}

	void Awake () {
		rb = GetComponent <Rigidbody> ();
		targetFinder = GetComponent<AITargetFinder> ();
		weaponController = GetComponent<WeaponController> ();

		availableBehaviours.AddRange (GetComponents<IBehaviour> ());
	}

	void Start () {
		if (currentBehaviour == null) {
			updateBehaviour = true;
		}
		StartCoroutine (ObstacleAvoidance ());
	}
	
	void Update () {
		if (paused) {
			return;
		}

		if (updateBehaviour) {
			if (inSquad) {
				SquadBehaviour ();
			} else {
				SoloBehaviour ();
			}
			updateBehaviour = false;
		}
	}


	void FixedUpate () {
		if (paused) {
			return;
		}
		mc.SetMovementAdditive (-avoidanceVector.normalized * 1000f);
	}

	void SoloBehaviour () {
		if (targetFinder.HasPriorityTarget) {
			if (!priorityOverride) {
				foreach (IBehaviour behaviour in availableBehaviours) {
					if (behaviour.Name.Equals ("Attack")) {
						if (currentBehaviour != null) {
							currentBehaviour.EndBehaviour ();
						}

						currentBehaviour = behaviour;
						activeBehaviourName = behaviour.Name;
						priorityOverride = true;
						currentBehaviour.StartBehaviour ();
						break;
					}
				}
			}


		} else {
			if (currentBehaviour != null) {
				if (currentBehaviour.IsDone) {
					print ("GETTING NEW BEHAVIOUR FOR: " + transform.name);
					NewBehaviour ();
				}
			} else {
				NewBehaviour ();
			}

			priorityOverride = false;
		}
	}

	void SquadBehaviour () {

	}

	void NewBehaviour () {
		if (availableBehaviours.Count > 0) {
			currentBehaviour = availableBehaviours[0];
			currentBehaviour.StartBehaviour ();
			activeBehaviourName = currentBehaviour.Name;
		} else {
			print ("REVERTING TO IDLE AS THERE ARE NO AVAILABE BEHAVIOURS!");
			currentBehaviour = new AIBehaviourIdle ();
		}
	}

	void OnPauseGame () {
		paused = true;
	}

	void OnResumeGame () {
		paused = false;
	}

	public IEnumerator ObstacleAvoidance () {
		while (avoidObstacles) {

			avoidanceVector = Vector3.zero;
			RaycastHit hitInfo;
			float severity = 0f;
			if (Physics.CapsuleCast (transform.position, transform.position + transform.forward * avoidanceCheckDistance, frontCheckRadius, transform.forward, out hitInfo, avoidanceCheckDistance)) {
				//print ("COLLISION IMMINENT FOR !" + transform.name + " with " + hitInfo.transform.name + "ditance: " + hitInfo.distance);

				avoidanceVector += (hitInfo.point - transform.forward);
				severity = hitInfo.distance / avoidanceCheckDistance;
			}

			Ray ray = new Ray (transform.position, Vector3.forward);

			if (Physics.SphereCast (ray, proximityCheckDistance, out hitInfo, proximityCheckDistance)) {
				print ("Object detected within proximity range for " + transform.name);
				//avoidanceVector += hitInfo.point - transform.position;
				severity += hitInfo.distance / proximityCheckDistance;
			}

			severity = Mathf.Clamp (severity, 0, 1);
			avoidanceVector = avoidanceVector.normalized * avoidanceForce * severity;
			yield return new WaitForSeconds (avoidanceCheckInterval);
		}
		yield return null;
	}

	void OnDrawGizmos () {
		Gizmos.DrawWireSphere (transform.position, proximityCheckDistance);
		Debug.DrawRay (transform.position, -avoidanceVector, Color.gray); // Show form which 
		Debug.DrawRay (transform.position, avoidanceVector, Color.green); // shows direction
	}

	public class AIBehaviourIdle : IBehaviour {

		string name = "idle";
		public string Name {
			get {return name;}
		}

		bool done = false;
		public bool IsDone {
			get {return done;}
		}


		public void StartBehaviour () {

		}

		public void EndBehaviour () {

		}
	}
}
