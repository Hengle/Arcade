using UnityEngine;
using System.Collections;

public interface IBehaviour {

	string Name {
		get;
	}

	bool IsDone {
		get;
	}

	void StartBehaviour ();

	void EndBehaviour ();
}
