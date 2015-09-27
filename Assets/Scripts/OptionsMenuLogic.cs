using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class OptionsMenuLogic : MonoBehaviour {

	int inputType = 0;
	public Text inputTypeText;

	void Start () {
		if (PlayerPrefs.HasKey ("InputType")) {
			inputType = PlayerPrefs.GetInt ("InputType");
		}

		inputTypeText.text = InputController.inputTypeString[inputType];
	}

	public void CycleInputSetting () {
		if (inputType < InputController.inputTypeString.Length -1) {
			inputType++;
		} else {
			inputType = 0;
		}
		inputTypeText.text = InputController.inputTypeString[inputType];
	}

	void OnDisable () {
		PlayerPrefs.SetInt ("InputType", inputType);
		PlayerPrefs.Save ();
	}
}
