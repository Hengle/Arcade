using UnityEngine;
using System.Collections;

public class ExplodeOnContact : MonoBehaviour {
	
	[SerializeField]
	private Transform explosionEffect;
	
	void OnTriggerEnter (Collider other) {
		Instantiate (explosionEffect, transform.position, Quaternion.identity);
		Destroy (this.gameObject);
	}
}
