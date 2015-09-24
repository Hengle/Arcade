using UnityEngine;
using System.Collections;

[RequireComponent (typeof (HealthManager))]
[RequireComponent (typeof (NPCWeaponManager))]

public class EnemyAI : MonoBehaviour {

	// References
	private HealthManager healthManager;

	// Variables
	[SerializeField]
	private float targetScanDelay = 0.2f;

	void Start () {
		healthManager = GetComponent<HealthManager> ();
	}

	void FixedUpdate () {

	}

	IEnumerator ScanForTargets () {
		while (true) {
			yield return new WaitForSeconds (targetScanDelay);
		}
	}


}
