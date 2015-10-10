using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AICore : MonoBehaviour {

	private List<IBehaviour> availableBehaviours = new List<IBehaviour> ();
	private AITargetFinder targetFinder;
	private IBehaviour currentBehaviour;
	[ShowOnlyAttribute]
	public string activeBehaviourName;

	private bool inSquad = false;
	
	[SerializeField][Range (1, 1000)]
	private float aggressionRange = 500f;
	bool priorityOverride = false;


	public float AggressionRange {
		get {return aggressionRange;}
	}

	void Awake () {
		targetFinder = GetComponent<AITargetFinder> ();
		availableBehaviours.AddRange (GetComponents<IBehaviour> ());
	}
	
	void Update () {
		if (inSquad) {
			SquadBehaviour ();
		} else {
			SoloBehaviour ();
		}
	}

	void SoloBehaviour () {
		if (targetFinder.HasPriorityTarget) {
			if (!priorityOverride) {
				foreach (IBehaviour behaviour in availableBehaviours) {
					if (behaviour.Name.Equals ("Attack")) {
						currentBehaviour = behaviour;
						priorityOverride = true;

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
		currentBehaviour = availableBehaviours[0];
		currentBehaviour.StartBehaviour ();
		activeBehaviourName = currentBehaviour.Name;
	}
}
