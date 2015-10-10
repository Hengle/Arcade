using UnityEngine;
using System.Collections;

public class AIBehaviourAttack : MonoBehaviour, IBehaviour {


	private static string _name = "Attack";
	public string Name {
		get {return _name;}
	}
	
	private bool done = false;
	public bool IsDone {
		get {return done;}
	}


	public void StartBehaviour () {
		print ("STARTING ATTACK BEHAVIOUR FOR " + transform.name);
	}
	
	void Update () {
	
	}
}
