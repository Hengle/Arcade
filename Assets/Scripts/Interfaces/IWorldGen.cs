using UnityEngine;
using System.Collections;

public interface IWorldGen {

	bool IsDoneGenerating ();

	int GetCurrentGenChance ();

	IEnumerator Generate (Vector3 poisition);
}
