﻿using UnityEngine;
using System.Collections;


public class InputController : MonoBehaviour {

	public enum ControlType : byte {KEYBOARD = 0, XBOX = 1, PS = 2}
	public static string[] inputTypeString = new string[3] {"Keyboard", "Controller: Xbox", "Controller: PS"};


	// References
	IMovementController mc;
	WeaponController wc;
	PlayerData pd;

	// Inputs
	Vector3 movement = Vector3.zero;
	Vector3 rotation = Vector3.zero;

	// Variables
	public ControlType inputDevice;

	public float InputVerical {
		get {return movement.z;}
	}
	public float InputHorizontal {
		get {return movement.x;}
	}
	
	// Use this for initialization
	void Start () {
		mc = GetComponent<IMovementController> ();
		wc = GetComponent<WeaponController> ();
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

	void OnGameStart () {
		int input = PlayerPrefs.GetInt ("InputType");
		print (input);
		switch (input) {
		case (int) ControlType.KEYBOARD:
			inputDevice = ControlType.KEYBOARD;
			break;
		case (int) ControlType.PS:
			inputDevice = ControlType.PS;
			break;
		case (int)ControlType.XBOX:
			inputDevice = ControlType.XBOX;
			break;
		default:
			inputDevice = ControlType.KEYBOARD;
			break;
		}
	}
	
	void GetMovementInput () {
		// ADD CHECK FOR PLAYER

		switch (inputDevice) {
		case ControlType.KEYBOARD:
			movement.x = Input.GetAxis ("Horizontal");
			movement.z = Input.GetAxis ("Vertical");

			float mouseX = Input.GetAxis ("Mouse X"), mouseY = Input.GetAxis ("Mouse Y");
			rotation.x = (mouseX >= 0.15 || mouseX <= -0.15) ? mouseX : 0;
			rotation.y = (mouseY >= 0.15 || mouseY <= -0.15) ? mouseY : 0;
			break;
		case ControlType.PS:
			movement.x = Input.GetAxis ("ControllerXBOXLX" + pd.Index);
			movement.y = Input.GetAxis ("ControllerXBOXLY" + pd.Index);
			movement.z = Input.GetButton ("joystick button 6") ? 1 : 0;
			
			rotation.x = Input.GetAxis ("ControllerPSRX" + pd.Index);
			rotation.y = Input.GetAxis ("ControllerPSRY" + pd.Index);
			break;
		case ControlType.XBOX:
			movement.x = Input.GetAxis ("ControllerXBOXLX" + pd.Index);
			movement.z = Input.GetAxis ("ControllerXBOXLY" + pd.Index);
			
			rotation.x = Input.GetAxis ("ControllerXBOXRX" + pd.Index);
			rotation.y = Input.GetAxis ("ControllerXBOXRY" + pd.Index);
			break;
		default: 
			print ("No Controller set!");
			inputDevice = ControlType.KEYBOARD;
			break;
		}
		mc.SetRotationVector (rotation);
		mc.SetMovementVector (movement);
	}

	void GetWeapons () {
		switch (inputDevice) {
		case ControlType.KEYBOARD:

			break;
		case ControlType.PS:
			if (Input.GetButton ("joystick button 7")) {
				wc.FirePrimary ();
			}
			if (Input.GetButton ("joystic button 5")) {
				wc.FireSecondary ();
			}
			break;
		case ControlType.XBOX:
			if (Input.GetAxis ("TriggersXBOX1") < -0.2) {
				wc.FirePrimary ();
			}
			/*if (Input.GetAxis ("TriggersXBOX1") > 0.2) {
				wc.FireSecondary ();
			}*/
			break;
		}

	}
}
