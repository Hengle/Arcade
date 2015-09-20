using UnityEngine;
using System.Collections;

public class DestroyAfterDelay : MonoBehaviour {

	public float lifeTime = 3f;
	
	// Update is called once per frame
	void Update () {
		if (lifeTime <= 0) {
			Destroy (this.gameObject);
		}
		lifeTime -= Time.deltaTime;
	}
}
