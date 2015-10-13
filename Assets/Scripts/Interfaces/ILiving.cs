using UnityEngine;
using System.Collections;

public interface ILiving  {

	bool IsAlive ();

	// should return flase if the entity cannot respawn
	bool Respawn ();

}
