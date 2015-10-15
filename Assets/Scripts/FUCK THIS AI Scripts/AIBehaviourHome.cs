using UnityEngine;
using System.Collections;

public class AIBehaviourHome : MonoBehaviour, IBehaviour {

	AITargetFinder ai;


	string name = "Home";
	public string Name {
		get {return name;}
	}
	
	bool done = false;
	bool paused = false;

	public bool IsDone {
		get {return done;}
	}

	public float turnDamping = 1f;
	public float maxTurnAngle = 10f;

	[ShowOnlyAttribute]
	bool disabled = false;

	// Use this for initialization
	void Awake () {
		ai = GetComponent<AITargetFinder> ();
	}

	public void StartBehaviour () {
		enabled = true;
	}

	public void EndBehaviour () {
		enabled = false;
	}

	void Start () {
		StartCoroutine (AngleChecker ());
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		if (paused) {
			return;
		}
	
		if (!disabled) {
			if (ai.HasPriorityTarget) {
				Quaternion targetRotation = Quaternion.LookRotation (ai.priorityTarget.position - transform.position);
				transform.rotation = Quaternion.Slerp (transform.rotation, targetRotation, Time.fixedDeltaTime * turnDamping);
				
				GameManager.instance.informationText.gameObject.SetActive (true);
				GameManager.instance.informationText.text = "MISSILE\nLOCK ON!";
			}
		}
	}

	void OnPauseGame () {
		paused = true;
	}

	void OnResumeGame () {
		paused = false;
	}

	IEnumerator AngleChecker () {
		while (true) {
			if (ai.HasPriorityTarget) {
				if (Vector3.Angle (transform.forward, ai.priorityTarget.position - transform.position) > maxTurnAngle) {
					disabled = true;
					GameManager.instance.informationText.gameObject.SetActive (false);
					
				}
			}
			yield return new WaitForSeconds (0.1f);
		}
		yield return null;
	}
}
