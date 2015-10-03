using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AICore : MonoBehaviour {

	private List<IBehaviour> availableBehaviours = new List<IBehaviour> ();
	private AITargetFinder targetFinder;
	private IBehaviour currentBehaviour;
	[ShowOnlyAttribute]
	public string activeBehaviour;

	private bool inSquad = false;
	
	[SerializeField][Range (1, 1000)]
	private float aggressionRange = 500f;

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
		if (currentBehaviour != null) {
			if (currentBehaviour.IsDone) {
				print ("GETTING NEW BEHAVIOUR FOR: " + transform.name);
				NewBehaviour ();
			}
		} else {
			NewBehaviour ();
		}
	}

	void SquadBehaviour () {

	}

	void NewBehaviour () {
		currentBehaviour = availableBehaviours[0];
		currentBehaviour.StartBehaviour ();
		activeBehaviour = currentBehaviour.Name;
	}
}
