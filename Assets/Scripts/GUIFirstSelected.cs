using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GUIFirstSelected : MonoBehaviour {
	
	private Selectable sel;

	void Start () {
		sel = this.GetComponent<Selectable> ();
	}

	void OnEnable () {
		StopCoroutine ("SelectFirstElement");
		StartCoroutine ("SelectFirstElement");
	}

	IEnumerator SelectFirstElement () {
		yield return new WaitForEndOfFrame ();
		sel.Select ();
		yield return null;
	}
}
