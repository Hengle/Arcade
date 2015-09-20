using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayMenuLogic : MonoBehaviour {

	Coroutine coroutine;
	Animator animator;
	bool awakeDone;
	bool buttonsInitialized;

	public GameObject buttonContainer;

	void Start () {
		animator = GetComponent<Animator> ();
		coroutine = StartCoroutine ("DetectControllers");
		awakeDone = false;
		buttonsInitialized = false;

		if (buttonContainer != null) {
			buttonContainer.SetActive (false);
		}
	}
	
	void Update () {
		if (!awakeDone) {
			awakeDone = animator.GetBool ("AwakeDone");
		}

		if (!buttonsInitialized && awakeDone) {
			InitilizeButtons ();
		}
	}

	void InitilizeButtons () {
		if (buttonContainer != null) {
			buttonContainer.SetActive (true);
		}
		buttonsInitialized = true;
	}

	IEnumerator DetectControllers () {
		while (true) {
			Debug.Log (Input.GetJoystickNames().Length);
			yield return new WaitForSeconds (1f);
		}
	}
}
