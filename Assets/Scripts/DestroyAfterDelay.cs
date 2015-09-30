using UnityEngine;
using System.Collections;

public class DestroyAfterDelay : MonoBehaviour {

	[SerializeField]
	private float lifeTime = 3f;
	[SerializeField]
	private Transform effectOnDestory;
	
	// Update is called once per frame
	void Update () {
		if (lifeTime <= 0) {
			if (effectOnDestory != null) {
				Instantiate (effectOnDestory,transform.position, transform.rotation);
			}
			Destroy (this.gameObject);
		}
		lifeTime -= Time.deltaTime;
	}
}
