using UnityEngine;
using System.Collections;

public class DestroyOnContact : MonoBehaviour {

	public string[] tagsToDestroyWith;
	public DamageProfile damageProfile;

	bool genDestroy = true;

	void Awake () {
		genDestroy = true;
	}

	void OnCollisionEnter (Collision col)
	{
		if (genDestroy) {
			for (int i = 0; i < tagsToDestroyWith.Length; i++){ 
				if (col.transform.tag.Equals (tagsToDestroyWith[i]) ) {
					Destroy (gameObject);
					return;
				}
			}
		}

		IDamageable damage = col.transform.GetComponent<IDamageable> ();

		if (damage != null) {
			damage.Damage (damageProfile);
		}
	}

	void OnGameStart () {
		StartCoroutine (DisableGenerationCollision ());
	}

	IEnumerator DisableGenerationCollision () {
		yield return new WaitForSeconds (2f);

		genDestroy = false;

	}
}
