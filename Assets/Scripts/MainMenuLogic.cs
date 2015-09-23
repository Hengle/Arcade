using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MainMenuLogic : MonoBehaviour {

	public Button firstSelected;
	private bool isInitialization = true;
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnEnable () {
		if (!isInitialization) {
			firstSelected.Select ();
		} else {
			isInitialization = false;
		}
	}
}
