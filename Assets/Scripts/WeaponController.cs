using UnityEngine;
using System.Collections;

public class WeaponController : MonoBehaviour {

	public GameObject primaryWeapon;
	public GameObject secondaryWeapon;

	private IWeapon pWeapon;
	private IWeapon sWeapon;

	// Use this for initialization
	void Start () {
		pWeapon = primaryWeapon.GetComponent<IWeapon> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetMouseButtonDown (0)) {
			FirePrimary ();
		}
	}

	void FirePrimary () {
		if (pWeapon.CanFire ()) {
			Debug.Log ("Firing Primary Weapon.");
			pWeapon.Fire ();
		}
	}

	void FireSecondary () {
		Debug.Log ("Firing Secondary Weapon.");
	}
}
