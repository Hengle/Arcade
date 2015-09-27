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
	private NPCWeaponManager weapons;
	[SerializeField]
	private Target curretTarget;
	[SerializeField]
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
	private float movementUpdateDelay = 0.2f;
	private float timeToMovementUpdate = 0.2f;
	[SerializeField]
	private bool canMove = true;
	private Vector3 movementVector = Vector3.zero;
	private Vector3 rotationVector = Vector3.zero;

	public bool debugInfo = false;

	private static int maxUpdatesPerFrame = 5;
	private static int updatesPerFrame = 0;
	private Thread scanThread;
	//private Thread moveThread;

	void Awake () {
		print (curretTarget.position);
		healthManager = GetComponent<HealthManager> ();
		movementController = GetComponent<IMovementController> ();
		weapons = GetComponent<NPCWeaponManager> ();
	}

	void Start () {
		ownPosition = transform.position;
		ownName = transform.name;

		scanThread = new Thread (new ThreadStart (TargetScan));
		scanThread.Name = ("Target Scanner for: " + transform.name);
		scanThread.Start ();

		//moveThread = new Thread (new ThreadStart (UpdateMovement));
		//moveThread.Name = ("Movement Update for: " + transform.name);
		//moveThread.Start ();

		StartCoroutine (UpdatePosition (transform));
	}

	void Update () {
		if (timeToMovementUpdate <= 0) {
			if (updatesPerFrame < maxUpdatesPerFrame) {
				UpdateMovement ();
				timeToMovementUpdate = movementUpdateDelay;
			}
			updatesPerFrame++;
		}                               
		timeToMovementUpdate -= Time.deltaTime;
		timeToNewTarget -= Time.deltaTime;
	}

	void LateUpdate () {
		updatesPerFrame = 0;
	}

	void FixedUpdate () {
		if (weaponTarget != null) {
			weapons.Fire ();
		}
	}

	void OnApplicationQuit () {
		scanThread.Abort ();
	}

	void OnDestroy () {
		scanThread.Abort ();
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
					weaponTarget = target;
					hasPriorityTarget = true;
				}
			}

			if (!hasPriorityTarget) {
				weaponTarget = null;
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
			//print (curretTarget.position);

			//print (thread.Name);
			Thread.Sleep (timeToWait);
		}
	}

	void UpdateMovement () {
		if (canMove) {
			if (curretTarget != null) {
				if (weaponTarget != null) {
					transform.LookAt (weaponTarget.position);
				} else {
					//transform.LookAt (curretTarget.position);
					Quaternion targetRotation = Quaternion.LookRotation (curretTarget.position - transform.position);
					transform.rotation = Quaternion.Slerp (transform.rotation, targetRotation, 25 * Time.deltaTime);
				}

				float distance = Vector3.Distance (ownPosition, curretTarget.position);
				if (debugInfo) {
					print ("Distance to target: " + distance);
				}

				if (distance < 20) {
					if (hasPriorityTarget) {
						if (distance < 10) {
							movementVector.z = -1f;
						} else {
							movementVector.z = 0f;
							movementVector.y = 1 - Random.value *2;
							movementVector.z = 1 - Random.value *2;
						}
					} else {
						curretTarget = EnemyTarget.instance.GetNewRandomTargetExcluding (curretTarget);
					}
					
				} else {
					movementVector.z = Random.value * Random.value / 2; 
				}
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
