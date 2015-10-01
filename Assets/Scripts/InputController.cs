using UnityEngine;
using System.Collections;


public class InputController : MonoBehaviour {

	public enum ControlType : byte {KEYBOARD = 0, XBOX = 1, PS = 2}
	public static string[] inputTypeString = new string[3] {"Keyboard", "Controller: Xbox", "Controller: PS"};


	// References
	IMovementController mc;
	WeaponController weaponController;
	PlayerData pd;

	// Inputs
	Vector3 movement = Vector3.zero;
	Vector3 rotation = Vector3.zero;

	// Variables
	public ControlType inputType;

	public float InputVerical {
		get {return movement.z;}
	}
	public float InputHorizontal {
		get {return movement.x;}
	}

	void Awake () {
		mc = GetComponent<IMovementController> ();
		weaponController = GetComponent<WeaponController> ();
		pd = GetComponent<PlayerData> ();
	}

	void Start () {
		int input = PlayerPrefs.GetInt ("InputType");
		print ("Input Index:" +input);
		switch (input) {
		case (int) ControlType.KEYBOARD:
			inputType = ControlType.KEYBOARD;
			break;
		case (int) ControlType.PS:
			inputType = ControlType.PS;
			break;
		case (int)ControlType.XBOX:
			inputType = ControlType.XBOX;
			break;
		default:
			inputType = ControlType.KEYBOARD;
			break;
		}
	}
	
	void Update () {
		if (mc != null) {
			GetMovementInput ();
		}
		if (weaponController != null) {
			GetWeapons ();
		}
	}
	
	void OnGameStart () {
		int input = PlayerPrefs.GetInt ("InputType");
		print ("Input Index:" +input);
		switch (input) {
		case (int) ControlType.KEYBOARD:
			inputType = ControlType.KEYBOARD;
			break;
		case (int) ControlType.PS:
			inputType = ControlType.PS;
			break;
		case (int)ControlType.XBOX:
			inputType = ControlType.XBOX;
			break;
		default:
			inputType = ControlType.KEYBOARD;
			break;
		}
	}
	
	void GetMovementInput () {
		// ADD CHECK FOR PLAYER

		switch (inputType) {
		case ControlType.KEYBOARD:
			movement.x = Input.GetAxis ("Horizontal");
			movement.z = Input.GetAxis ("Vertical");

			float mouseX = Input.GetAxis ("Mouse X"), mouseY = Input.GetAxis ("Mouse Y");
			rotation.x = (mouseX >= 0.15 || mouseX <= -0.15) ? mouseX : 0;
			rotation.y = (mouseY >= 0.15 || mouseY <= -0.15) ? mouseY : 0;
			break;
		case ControlType.PS:
			movement.x = Input.GetAxis ("ControllerPSLX" + pd.Index);
			movement.y = Input.GetAxis ("ControllerPSLY" + pd.Index);
			movement.z = Input.GetKey (KeyCode.JoystickButton6) ? 1 : 0;

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
			inputType = ControlType.KEYBOARD;
			break;
		}
		mc.SetRotationVector (rotation);
		mc.SetMovementVector (movement);
	}

	void GetWeapons () {
		switch (inputType) {
		case ControlType.KEYBOARD:

			break;
		case ControlType.PS:
			if (Input.GetKey (KeyCode.Joystick8Button7)) {
				weaponController.FirePrimary ();
			}
			if (Input.GetKey (KeyCode.Joystick8Button5)) {
				weaponController.FireSecondary ();
			}
			break;
		case ControlType.XBOX:
			if (Input.GetAxis ("TriggersXBOX1") < -0.2) {
				weaponController.FirePrimary ();
			}
			/*if (Input.GetAxis ("TriggersXBOX1") > 0.2) {
				wc.FireSecondary ();
			}*/
			break;
		}

	}
}
