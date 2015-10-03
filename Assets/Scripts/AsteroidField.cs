using UnityEngine;
using System.Collections;

public class AsteroidField : MonoBehaviour, IWorldGen {

	[Range (1, 20)]
	public int maxIterationsPerFrame = 10;
	public int generationWeigth = 20;
	public int minLevel = 0;

	public int numAsterois = 200;
	public Vector2 asteroidSize = new Vector2 (40f, 250f);
	public Vector2 distanceFromBase = new Vector2 (200, 1000);
	public Transform[] asteroidPrefabs;


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
		float sizeMult;

		for(int i = 0; i < numAsterois; ++i) {
			float distance =  Random.Range (distanceFromBase.x, distanceFromBase.y);
			Vector3 pos = Random.onUnitSphere * distanceFromBase.y -Random.onUnitSphere * distance;

			Transform t = (Transform) Instantiate (asteroidPrefabs[Random.Range (0, asteroidPrefabs.Length -1)], pos, Quaternion.identity);

			sizeMult = Random.Range (asteroidSize.x, asteroidSize.y);
			t.transform.localScale = Vector3.one * sizeMult;
			t.GetComponent<HealthManager> ().SetHealthMod (sizeMult / 100);
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
