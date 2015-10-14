﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Threading;

public class AITargetFinder : MonoBehaviour {

	public static int targetFindigThreads = 4;
	private static bool threadsInitialized = false;
	private static readonly Object targetGetterLock = new Object ();

	private static List<Thread> targetFinders = new List<Thread> ();
	private static Queue<TargetRequest> targetRequests = new Queue<TargetRequest> ();

	public bool pathfindingDebug = false;

	[SerializeField][Range (0.1f, 5f)]
	private float targetFindInterval = 1f;
	private float timeToTargetFind;

	public float positionUpdateInterval = 0.05f;

	[Header ("Target Information")]
	public Target curretTarget = null;
	public Target weaponTarget = null;
	public Target priorityTarget = null;

	private Vector3 ownPosition;
	private string ownName;
	private bool targetScanDone = false;
	[Range (0, 2000)]
	public float aggressionRange = 600f;

	private float timeToWaypointSwitch;
	[ShowOnlyAttribute]
	public float distanceToCurrent;
	[ShowOnlyAttribute]
	public float distanceToWeapon;
	[ShowOnlyAttribute]
	public float distanceToPrimary;
	[HideInInspector]
	private bool hasPriorityTarget = false;

	public bool HasWeaponTarget {
		get {return (weaponTarget != null) ? true : false;}
	}
	public bool HasPriorityTarget {
		get {return hasPriorityTarget;}
	}

	void Awake () {
		if (!threadsInitialized) {
			for (int i = 0; i < targetFindigThreads; i++) {
				Thread thread = new Thread (new ThreadStart (TargetScan));
				thread.Name = ("Target Scanner " + i);
				thread.Start ();
				targetFinders.Add (thread);
			}
			threadsInitialized = true;
			print ("PATHFINDING THREADS INITILIZED");
		}
	}

	void Start () {
		ownName = transform.name;
		ownPosition = transform.position;
		timeToTargetFind = targetFindInterval;

		StartCoroutine (UpdatePosition (transform));
		AddTargetRequest ();
	}

	void Update () {
		if (timeToTargetFind < 0) {
			AddTargetRequest ();
			timeToTargetFind = targetFindInterval;
		}
		if (targetScanDone = true) {
			timeToTargetFind -= Time.deltaTime;
		}
	}

	void OnApplicationQuit () {
		foreach (Thread thread in targetFinders) {
			thread.Abort ();
		}
	}

	void AddTargetRequest () {
		targetRequests.Enqueue ( new TargetRequest (this));
	}

	TargetRequest GetTargetRequest () {
		lock (targetGetterLock) {
			if (targetRequests.Count > 0) {
				return targetRequests.Dequeue ();
			} 
			return null;
		}
	}


	// Target Scanner main loop
	void TargetScan () {
		TargetRequest request;

		while (true) {
			request = GetTargetRequest ();

			if (request != null) {
				Target target = ScanForPlayers (request);
				request.ai.targetScanDone = true;
				Thread.Sleep (25);
			} else {
				//print ("No need for target finding!");
				Thread.Sleep (500);
			}
		}
	}

	static Target SwitchWaypoint () {
		Target target = PathfindingManager.instance.GetNewRandomTarget ();
		print (target);
		return target;
	}

	static Target ScanForPlayers (TargetRequest request) {
		foreach (Target playerTarget in PathfindingManager.instance.PlayerTargets) {
			float distance = Vector3.Distance (request.ai.ownPosition, playerTarget.position);

			if (distance <= request.aggressionRange) {
				if (request.ai.hasPriorityTarget) {
					if (distance < Vector3.Distance (request.ownPosition, request.ai.priorityTarget.position)) {
						request.ai.priorityTarget = playerTarget;
					}
				} else {
					request.ai.priorityTarget = playerTarget;
				}
				request.ai.distanceToPrimary = distance;
				request.ai.hasPriorityTarget = true;
			}
		}

		return null;
	}

	IEnumerator UpdatePosition (Transform t) {
		while (true) {
			ownPosition = t.position;
			yield return new WaitForSeconds (positionUpdateInterval);
		}
	}

	public class TargetRequest {
		public AITargetFinder ai;
		public float aggressionRange;
		public Vector3 ownPosition;
		public string requesterName;

		public TargetRequest (AITargetFinder _ai) {
			ai = _ai;
			aggressionRange = _ai.aggressionRange;
			ownPosition = _ai.ownPosition;
			requesterName = _ai.transform.name;
		}
	}
}