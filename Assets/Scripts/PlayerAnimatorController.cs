using UnityEngine;
using System.Collections;

public class PlayerAnimatorController : MonoBehaviour {

	private Rigidbody rb;
	private Animator anim;
	public SphereCollider baseCollider;

	private bool respawnDone = false;
	private bool initiated = false;

	void Awake () {
		rb = GetComponent<Rigidbody> ();
		anim = GetComponent<Animator> ();
	}

	void Start () {
		rb.isKinematic = true;
		initiated = true;
	}

	void OnEnable () {
		StartCoroutine (RestoreCollision ());

		if (initiated) {
			print ("ENABLEED");
			print (transform.position);

			anim.SetTrigger ("SpawnAnim");
			rb.isKinematic = true;

			anim.enabled = true;

			rb.velocity = Vector3.zero;

			transform.position = WorldManager.instance.playerSpawnPoint.position;
			transform.rotation = WorldManager.instance.playerSpawnPoint.rotation;
		}
	}
	
	IEnumerator RestoreCollision () {
		yield return new WaitForSeconds (3f);

		anim.enabled = false;
		rb.isKinematic = false;
		respawnDone = true;
		rb.velocity = transform.forward * 100;
		yield return null;
	}
}
