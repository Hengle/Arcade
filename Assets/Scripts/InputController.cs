using UnityEngine;
using System.Collections;


public class InputController : MonoBehaviour {

	// References
	MovementController mc;
	WeaponController wc;

	// Inputs
	Vector3 movement = Vector3.zero;
	float inputV, inputH;

	public float InputVerical {
		get {return inputV;}
	}
	public float InputHorizontal {
		get {return inputH;}
	}
	
	// Use this for initialization
	void Start () {
		mc = GetComponent<MovementController> ();
	}
	
	void Update () {
		if (mc != null) {
			GetMovementInput ();
		}
		if (wc != null) {
			GetWeapons ();
		}
	}

	void GetMovementInput () {
		inputV = Input.GetAxis ("Vertical");
		inputH = Input.GetAxis ("Horizontal");
		
		if (inputV > 0.1 || inputV < -0.1) {
			mc.moveInput.z = inputV;
		} else {
			mc.moveInput.z = 0f;
		}
		if (inputH > 0.1 || inputH < -0.1) {
			mc.moveInput.x = inputH;
		} else {
			mc.moveInput.x = 0f;
			}
	}

	void GetWeapons () {

	}
}
