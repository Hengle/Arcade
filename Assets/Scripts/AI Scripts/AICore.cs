using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AICore : MonoBehaviour {
	
	public enum BehaviourType {PASSIVE, DEFENSIVE, OFFENSIVE}
	public bool avoidObstacles;

	private List<IBehaviour> availableBehaviours = new List<IBehaviour> ();
	private AITargetFinder targetFinder;
	private IBehaviour currentBehaviour;
	[ShowOnlyAttribute]
	public string activeBehaviourName;
	[HideInInspector]
	public WeaponController wc;

	private bool inSquad = false;

	public bool updateBehaviour = false;

	public AITargetFinder GetTargetFinder {
		get {return targetFinder;}
	}
	
	[SerializeField][Range (1, 1000)]
	private float aggressionRange = 500f;
	bool priorityOverride = false;


	public float AggressionRange {
		get {return aggressionRange;}
	}

	void Awake () {
		targetFinder = GetComponent<AITargetFinder> ();
		wc = GetComponent<WeaponController> ();

		availableBehaviours.AddRange (GetComponents<IBehaviour> ());
	}

	void Start () {
		if (currentBehaviour == null) {
			updateBehaviour = true;
		}
	}
	
	void Update () {
		if (updateBehaviour) {
			if (inSquad) {
				SquadBehaviour ();
			} else {
				SoloBehaviour ();
			}
			updateBehaviour = false;
		}
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
