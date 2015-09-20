using UnityEngine;
using System.Collections;

public class TerrainProperties : MonoBehaviour {

	public string terrainName;
	public ResistanceProfile resistanceProfile = new ResistanceProfile ();
	
	[SerializeField]
	private float health;
	private bool indestrutible = false;



	// Use this for initialization
	void Start () {
		if (health < 0) {
			indestrutible = true;
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
