using UnityEngine;
using System.Threading;
using System.Collections;
using System.Collections.Generic;

public class AIObstacleAvoidance : MonoBehaviour {

	[Header ("Proximity Check")]
	public float proximityCheckRadius = 100f;

	[Header ("Avoidance Movement")]
	[SerializeField] [ShowOnlyAttribute]
	public Vector3 movementVector = Vector3.zero;

	// List of Navigation Requests
	public static volatile Queue<NavigationRequest> navRequests = new Queue<NavigationRequest> ();

	// List of other stuff
	// Threads for avoidance calculations
	[Header ("Thread Info")]
	[SerializeField] [ShowOnlyAttribute]
	static bool threadsInitialized = false;
	static int numAvoidanceThreads = 8;
	private static List<Thread> avoidanceCalculators = new List<Thread> ();
	private static readonly Object navReqLock = new Object ();

	// Own Info
	[SerializeField] [ShowOnlyAttribute]
	Vector3 currentPosition = Vector3.zero;
	Vector3 currentForward = Vector3.zero;
	
	void Start () {
		if (!threadsInitialized) {
			for (int i = 0; i < numAvoidanceThreads; i++) {
				Thread thread = new Thread (new ThreadStart (ProximityCheck));
				thread.Name = "Avoidance Checker " +i;
				thread.Start ();
				avoidanceCalculators.Add (thread);
			}
			threadsInitialized = true;
		}

		StartCoroutine (UpdateProximityObjects ());
	}

	void FixedUpdate () {
		currentPosition = transform.position;
		currentForward = transform.forward;
	}

	void ProximityCheck () {
		while (true) {
			NavigationRequest navReq = GetNavRequest ();
			Vector3 _movement = Vector3.zero;

			if (navReq != null) {
				foreach (NavigationObject navObj in navReq.proximityObjects) {
					Vector3 _currentPos = navReq.requester.currentPosition;
					Vector3 _currentForward = navReq.requester.currentForward;
					Vector3 _targetDir =  navObj.position - navReq.requester.currentPosition;

					float distance = Vector3.Distance (_currentPos, navObj.position);

					_movement += _targetDir;

				}

				navReq.requester.movementVector = -_movement;

				Thread.Sleep (25);
			} else {
				Thread.Sleep (100);
			}
		}
	}

	static NavigationRequest GetNavRequest () {
		lock (navReqLock) {
			if (navRequests.Count > 0) {
				return navRequests.Dequeue ();
			} 
			return null;
		}
	}
	
	IEnumerator UpdateProximityObjects () {

		while (true) {
			Collider[] colliders = Physics.OverlapSphere (transform.position, proximityCheckRadius);

			if (colliders.Length > 0) {
				List<NavigationObject> navObjs = new List<NavigationObject> ();
				
				for (int i = 0; i < colliders.Length; i++) {
					if (!(colliders[i].tag.Equals ("Waypoint") || (colliders[i].tag.Equals ("Projectile")))) {
						navObjs.Add (new NavigationObject (colliders[i]));
					}
				}
				navRequests.Enqueue (new NavigationRequest (transform, this, navObjs.ToArray ()));
			} else {
				movementVector = Vector3.zero;
			}

			yield return new WaitForSeconds (0.05f);
		}

		yield return null;
	}

	public class NavigationRequest {
		public Transform transform;
		public AIObstacleAvoidance requester;
		public string name;
		public List<NavigationObject> proximityObjects;

		public NavigationRequest (Transform _transform, AIObstacleAvoidance _avoidance, NavigationObject[] _objects) {
			proximityObjects = new List<NavigationObject> ();
			requester = _avoidance;
			name = _transform.name;
			transform = _transform;
			proximityObjects.AddRange (_objects);
		}
	}

	public class NavigationObject {
		public Transform transform;
		public Vector3 position;
		public Vector3 closestPoint;
		//public float distance;

		public NavigationObject (Collider col) {
			transform = col.transform;
			closestPoint = col.ClosestPointOnBounds (transform.position);
			position = col.transform.position;
		}
	}

	void OnApplicationQuit () {
		foreach (Thread t in avoidanceCalculators) {
			t.Abort ();
		}
	}
}
