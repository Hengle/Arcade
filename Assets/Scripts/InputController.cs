using UnityEngine;
using System.Collections;


public class InputController : MonoBehaviour {

	// References
	IMovementController mc;
	WeaponController wc;
	PlayerData pd;

	// Inputs
	Vector3 movement = Vector3.zero;
	Vector3 rotation = Vector3.zero;

	// Variables
	public bool keyboard = false;

	public float InputVerical {
		get {return movement.z;}
	}
	public float InputHorizontal {
		get {return movement.x;}
	}
	
	// Use this for initialization
	void Start () {
		mc = GetComponent<IMovementController> ();
		pd = GetComponent<PlayerData> ();
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
		// ADD CHECK FOR PLAYER

		movement.x = Input.GetAxis ("ControllerLX" + pd.Index);
		movement.z = Input.GetAxis ("ControllerLY" + pd.Index);

		rotation.x = Input.GetAxis ("ControllerRX" + pd.Index);
		rotation.z = Input.GetAxis ("ControllerRY" + pd.Index);

		mc.SetRotationVector (rotation);
		mc.SetMovementVector (movement);
	}

	void GetWeapons () {

	}
}
