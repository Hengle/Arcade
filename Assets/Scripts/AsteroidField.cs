using UnityEngine;
using System.Collections;

public class AsteroidField : MonoBehaviour, IWorldGen {

	[Range (1, 20)]
	public int maxIterationsPerFrame = 10;
	public int generationWeigth = 20;
	public int minLevel = 0;

	public int numAsterois = 200;
	public Vector2 size = new Vector2 (40f, 250f);
	public Vector2 distanceFromBase = new Vector2 (200, 1000);
	public Transform[] asteroids;


	bool done;

	public bool IsDoneGenerating () {
		return done;
	}

	public int GetCurrentGenChance () {
		return generationWeigth + GameManager.instance.LevelIndex;
	}
	
	public IEnumerator Generate (Vector3 position) {
		print ("[WORLDGEN]: Starting Asteroid Field gen");
		done = false;
		int iterations = 0;

		for(int i = 0; i < numAsterois; ++i) {
			Transform t = (Transform) Instantiate (asteroids[Random.Range (0, asteroids.Length -1)]);
			float x =  Random.Range (distanceFromBase.x, distanceFromBase.y);
			
			t.transform.position = Random.onUnitSphere * x;
			
			t.transform.localScale = Vector3.one * Random.Range (size.x, size.y);
			t.SetParent (this.transform);

			iterations++;
			if (iterations > maxIterationsPerFrame) {
				yield return new WaitForEndOfFrame ();
				iterations = 0;
			}	
		}
		done = true;
		yield return null;
	}
}
