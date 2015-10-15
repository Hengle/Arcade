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
		anim.enabled = false;
		rb.isKinematic = true;
	}

	void OnRespawn () {
		rb.isKinematic = true;
		OnStartGame ();
	}

	void OnStartGame () {
		StartCoroutine (RestoreCollision ());

		if (anim.isActiveAndEnabled) {
			anim.SetTrigger ("SpawnAnim");
		} else {
			anim.enabled = true;
		}
		rb.isKinematic = true;

		rb.velocity = Vector3.zero;

		transform.position = WorldManager.instance.playerSpawnPoint.position;
		transform.rotation = WorldManager.instance.playerSpawnPoint.rotation;
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
