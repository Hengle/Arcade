using UnityEngine;
using System.Collections;

public class AsteroidField : MonoBehaviour {

	public int numAsterois = 2000;
	public Vector2 size = new Vector2 (40f, 250f);
	public Vector2 distanceFromBase = new Vector2 (600, 5000);
	public Transform[] asteroids;

	void Start () {
		for(int i = 0; i < numAsterois; ++i) {
			Transform t = (Transform) Instantiate (asteroids[Random.Range (0, asteroids.Length -1)]);
			float x =  Random.Range (distanceFromBase.x, distanceFromBase.y);

			t.transform.position = Random.onUnitSphere * x;

			t.transform.localScale = Vector3.one * Random.Range (size.x, size.y);
			t.SetParent (this.transform);
		}
	}
}
