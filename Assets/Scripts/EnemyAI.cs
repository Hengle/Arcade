using UnityEngine;
using System.Collections;
using System.Threading;

[RequireComponent (typeof (HealthManager))]
[RequireComponent (typeof (NPCWeaponManager))]
[RequireComponent (typeof (IMovementController))]

public class EnemyAI : MonoBehaviour {

	// References
	private HealthManager healthManager;
	private IMovementController movementController;
	[SerializeField]
	private Target curretTarget;
	private Target weaponTarget;
	private Vector3 ownPosition;
	private string ownName;

	// Variables
	[SerializeField]
	private float aggressionRange = 150f;
	[SerializeField]
	private Range aggressionTime = new Range (10, 25);
	[SerializeField]
	private float targetScanDelay = 1f;
	[SerializeField]
	private Range targetHoldTime = new Range (6, 15);
	private float timeToNewTarget = 0.1f;
	private bool hasPriorityTarget = false;
	[SerializeField]
	private float movementUpdateDelay = 0.1f;
	private float timeToMovementUpdate = 0f;
	[SerializeField]
	private bool canMove = true;
	private Vector3 movementVector = Vector3.zero;
	private Vector3 rotationVector = Vector3.zero;

	private Thread thread;

	void Awake () {
		print (curretTarget.position);
		healthManager = GetComponent<HealthManager> ();
		movementController = GetComponent<IMovementController> ();
	}

	void Start () {
		ownPosition = transform.position;
		ownName = transform.name;
		thread = new Thread (new ThreadStart (TargetScan));
		thread.Name = ("Target Scanner for: " + transform.name);
		thread.Start ();
		StartCoroutine (UpdatePosition (transform));
	}

	void Update () {
		/*if (timeToTargetScan <= 0) {
			timeToNewTarget = targetScanDelay;
			ScanForTargets ();
		}*/
		if (timeToMovementUpdate <= 0) {
			timeToMovementUpdate = timeToMovementUpdate;
			UpdateMovement ();
		}
		timeToMovementUpdate -= Time.deltaTime;
		timeToNewTarget -= Time.deltaTime;
	}

	void OnDisable () {
		thread.Abort ();
	}

	void OnApplicationQuit () {
		thread.Abort ();
	}

	void OnDestroy () {
		thread.Abort ();
	}
	
	void TargetScan () {
		int timeToWait = (int) (targetScanDelay * 1000);
		System.Random random = new System.Random();

		while (true) {
			hasPriorityTarget = false;

			print ("Number of player Targets: " + EnemyTarget.instance.PlayerTargets.Length);
			foreach (Target target in EnemyTarget.instance.PlayerTargets) {
				float distance = Vector3.Distance (ownPosition, target.position);
				
				if (distance <= aggressionRange) {
					if (hasPriorityTarget) {
						//if (distance > Vector3.Distance (transform.position, curretTarget.position))  {
						//	curretTarget = new Target (pd.transform, false, Random.Range (aggressionTime.x, aggressionTime.y));
						//
					}
					print ("Player in rage of " + ownName +  " (" +")");
					curretTarget = target;
					hasPriorityTarget = true;
				}
			}

			if (!hasPriorityTarget) {
				if (curretTarget == null) {
					timeToNewTarget = random.Next (targetHoldTime.min, targetHoldTime.max);
					curretTarget = EnemyTarget.instance.GetNewRandomTarget ();
				} else {
					if (timeToNewTarget <= 0) {
						timeToNewTarget = random.Next (targetHoldTime.min, targetHoldTime.max);
						curretTarget = EnemyTarget.instance.GetNewRandomTarget ();
					}
				}
			}
			print (curretTarget.position);

			print (thread.Name);
			Thread.Sleep (timeToWait);
		}
	}

	void UpdateMovement () {
		if (canMove) {
			if (curretTarget != null) {
				transform.LookAt (curretTarget.position);
				movementVector.z = 1f; 
				movementController.SetRotationVector (rotationVector);
				movementController.SetMovementVector (movementVector);
			}
		} else {
			movementController.SetRotationVector (Vector3.zero);
			movementController.SetMovementVector (Vector3.zero);
		}
	}

	IEnumerator UpdatePosition (Transform t) {
		while (true) {
			ownPosition = t.position;
			yield return new WaitForSeconds (0.1f);
		}
	}

	protected class Range  {
		public int min, max;

		public Range (int min, int max) {
			this.min = min;
			this.max = max;
		}
	}
}
