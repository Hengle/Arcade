using UnityEngine;
using System.Collections;
using System.Threading;

[RequireComponent (typeof (HealthManager))]
[RequireComponent (typeof (NPCWeaponManager))]
[RequireComponent (typeof (IMovementController))]

public class OldEnemyAI : MonoBehaviour {

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
	float timeSincePrioritytarget = 0f;
	[SerializeField]
	private float movementUpdateDelay = 0.2f;
	private float timeToMovementUpdate = 0.2f;
	[SerializeField]
	private bool canMove = true;
	public bool overrideManouverRange = false;
	public float overrideDistance;
	public float manouverDistance = 100f;
	private float manouverDistanceBackup;

	private Vector3 movementVector = Vector3.zero;
	private Vector3 rotationVector = Vector3.zero;

	public bool debugInfo = false;

	private static int maxUpdatesPerFrame = 5;
	private static int updatesPerFrame = 0;
	private Thread scanThread;
	//private Thread moveThread;

	void Awake () {
		//print (curretTarget.position);
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
		manouverDistanceBackup = manouverDistance;
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
		timeSincePrioritytarget -= Time.deltaTime;
	}

	void LateUpdate () {
		updatesPerFrame = 0;
	}

	void FixedUpdate () {
		Ray ray = new Ray (transform.position, transform.forward);
		Debug.DrawRay (transform.position, transform.forward * 350, Color.cyan);
		RaycastHit hit;

		if (Physics.Raycast (ray, out hit, 350)) {
			overrideManouverRange = false;

			if (hit.transform.tag.Equals ("Player")) {
				weapons.Fire ();
			} else if ( hit.transform.tag.Equals ("Asteroid")) {
				weapons.Fire ();
				if (weaponTarget != null) {
					weaponTarget = new Target (hit.transform, false, 5f);
				}
			} else if ( hit.transform.tag.Equals ("PlayerBase")) {
				weapons.Fire ();
				if (weaponTarget != null) {
					weaponTarget = new Target (hit.transform, false, 5f);
				}
				if (timeSincePrioritytarget > 10f) {
					curretTarget = new Target (hit.transform, false, 5f);
					timeSincePrioritytarget = 0f;
					hasPriorityTarget = true;
				} else {
					hasPriorityTarget = false;
					curretTarget = null;
					//curretTarget = new Target (transform, transform.position -transform.forward * Random.Range (200, 500) + transform.right * Random.Range (-200, 200)  + transform.up * Random.Range (-200, 200), false, 10f);
				}
				overrideManouverRange = true;
				overrideDistance = Vector3.Distance (hit.point, hit.transform.position);
			}
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
			if (curretTarget == null) {
				foreach (Target target in PathfindingManager.instance.PlayerTargets) {
					float distance = Vector3.Distance (ownPosition, target.position);
					
					if (distance <= aggressionRange) {
						if (hasPriorityTarget) {
							//if (distance > Vector3.Distance (transform.position, curretTarget.position))  {
							//	curretTarget = new Target (pd.transform, false, Random.Range (aggressionTime.x, aggressionTime.y));
							//
						}
						//print ("Player in rage of " + ownName +  " (" +")");
						curretTarget = target;
						weaponTarget = target;
						hasPriorityTarget = true;
					}
				}
				
				if (!hasPriorityTarget) {
					weaponTarget = null;
					if (curretTarget == null) {
						timeToNewTarget = random.Next (targetHoldTime.min, targetHoldTime.max);
						curretTarget = PathfindingManager.instance.GetNewRandomTarget ();
					} else {
						if (timeToNewTarget <= 0) {
							timeToNewTarget = random.Next (targetHoldTime.min, targetHoldTime.max);
							curretTarget = PathfindingManager.instance.GetNewRandomTarget ();
						}
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
				Quaternion targetRotation;
				float rotTime;

				if (weaponTarget != null) {
					targetRotation = Quaternion.LookRotation (weaponTarget.position - transform.position);
					rotTime = 100f;
				} else {
					//transform.LookAt (curretTarget.position);
					targetRotation = Quaternion.LookRotation (curretTarget.position - transform.position);
					rotTime = 250f;
				}
				transform.rotation = Quaternion.Slerp (transform.rotation, targetRotation, rotTime);

				float distance = Vector3.Distance (ownPosition, curretTarget.position);
				if (debugInfo) {
					print ("Distance to target: " + distance);
				}

				if (overrideManouverRange) {
					distance -= overrideDistance;
					manouverDistance = manouverDistanceBackup + overrideDistance;
				} else {
					manouverDistance = manouverDistanceBackup;
				}

				if (distance < manouverDistance) {

					if (hasPriorityTarget) {

						if (distance < manouverDistance * 0.1f) {
							movementVector.z = -1f;
							
						} else if (distance < manouverDistance * 0.15f) {
							movementVector.z = -0.8f;
						} else if (distance < manouverDistance * 0.3f) {
							movementVector.z = -0.3f;

						} else if (distance < manouverDistance * 0.7f) {
							movementVector.z = 0.2f - Random.value / 4;

						} else {
							movementVector.z = 0f;
						}

					} else {
						if (overrideManouverRange) {
							if (distance < manouverDistance * 0.3f) {
								movementVector.z = -0.8f;
							} else if (distance < manouverDistance * 0.7f) {
								movementVector.z = 0.2f - Random.value / 4;
							} else {
								movementVector.z = -0.1f;
							}
						} else {
							if (distance < manouverDistance * 0.2f) {
								print ("Refreshin target");
								curretTarget = PathfindingManager.instance.GetNewRandomTargetExcluding (curretTarget);
							}
						}
					}
					movementVector.y = 1 - Random.value *2;
					movementVector.z = 1 - Random.value *2;
					
				} else {
					movementVector.z = 1f; 
				}

				movementController.SetRotation (rotationVector);
				movementController.SetMovement (movementVector);
			}
		} else {
			movementController.SetRotation(Vector3.zero);
			movementController.SetMovement (Vector3.zero);
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
