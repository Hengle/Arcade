using UnityEngine;
using System.Collections;

[RequireComponent (typeof (HealthManager))]
public class EnemyAI : MonoBehaviour {

	private HealthManager healthManager;

	void Start () {
		healthManager = GetComponent<HealthManager> ();
	}


}
