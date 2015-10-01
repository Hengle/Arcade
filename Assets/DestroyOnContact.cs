using UnityEngine;
using System.Collections;

public class DestroyOnContact : MonoBehaviour {

	public string[] tagsToDestroyWith;
	public DamageProfile damageProfile;

	void OnCollisionEnter (Collision col)
	{
		for (int i = 0; i < tagsToDestroyWith.Length; i++){ 
			print (tagsToDestroyWith.Length);
			if (col.transform.tag.Equals (tagsToDestroyWith[i]) ) {
				print ("ASDASDAS" + col.transform.tag);
				Destroy (gameObject);
				return;
			}
		}
		IDamageable damage = col.transform.GetComponent<IDamageable> ();

		if (damage != null) {
			damage.Damage (damageProfile);
		}
	}
}
