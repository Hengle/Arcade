using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent (typeof (IMovementController))]
public class AIBehaviourPatrol : MonoBehaviour, IBehaviour {

	public Range numTargetsToPatrol = new Range (3, 5);
	public bool showPatrolRoute = false;

	private List<Target> targetsToPatrol;
	private IMovementController movementController;
	[SerializeField]
	private Target currentTarget = null;

	private static string _name = "Patrol";
	public string Name {
		get {return _name;}
	}

	private bool done = false;
	public bool IsDone {
		get {return done;}
	}

	void Awake () {
		movementController = GetComponent<IMovementController> ();
		targetsToPatrol = new List<Target> ();
	}
	
	public void StartBehaviour () {
		targetsToPatrol = new List<Target> ();

		for (int i = 0; i < 5; i++) {
			targetsToPatrol.Add (PathfindingManager.instance.GetNewRandomTarget ());
		}

		currentTarget = targetsToPatrol[0];
		done = false;
	}

	void Update () {
		if (targetsToPatrol.Count == 0) {
			done = true;
		}
	}

	void FixedUpdate () {
		if (currentTarget != null) {
			transform.LookAt (currentTarget.position);
			movementController.SetMovement (new Vector3 (0, 0, 1));
		}
	}

	void OnTriggerEnter (Collider other) {
		if (other.transform.Equals (currentTarget.transform)) {
			targetsToPatrol.RemoveAt (0);
	
			if (targetsToPatrol.Count > 0) {
				currentTarget = targetsToPatrol[0];
			} else {
				done = true;
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

	[System.Serializable]
	public class Range {
		public int min, max;

		public Range (int _min, int _max) {
			min = _min;
			max = _max;
		}
	}
}
