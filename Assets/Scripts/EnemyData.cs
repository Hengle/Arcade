using UnityEngine;
using System.Collections;

public class EnemyData : MonoBehaviour {

	public int scoreValue = 100;

	void OnDeath () {
		GameObject.Find ("GameManager").SendMessage ("EnemyDestroyed", scoreValue);
	}
}
