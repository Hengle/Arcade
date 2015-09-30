using UnityEngine;
using System.Collections;

public class Done_DestroyByContact : MonoBehaviour
{
	public GameObject explosion;
	public GameObject playerExplosion;
	public int scoreValue;
	private Done_GameController gameController;

	void OnTriggerEnter (Collider other)
	{
		if (other.tag == "Asteroid" || other.tag == "Player")
		{
			Destroy (other.gameObject);
			Destroy (gameObject);
		}
	}
}