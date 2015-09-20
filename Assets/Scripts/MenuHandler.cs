using UnityEngine;
using System.Collections;

public class MenuHandler : MonoBehaviour {

	public GameObject mainMenu;
	public GameObject playMenu;

	// Use this for initialization
	void Start () {
		if (mainMenu != null) {
			mainMenu.SetActive (true);
		}

		if (playMenu != null) {
			playMenu.SetActive (false);
		}
	}
}
