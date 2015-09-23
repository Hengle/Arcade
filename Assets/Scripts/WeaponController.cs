using UnityEngine;
using System.Collections;

public class WeaponController : MonoBehaviour {

	public GameObject primaryWeapon;
	public GameObject secondaryWeapon;

	private IWeapon pWeapon;
	private IWeapon sWeapon;

	private bool primaryFired;
	private bool secondaryFired;

	// Use this for initialization
	void Start () {
		pWeapon = primaryWeapon.GetComponent<IWeapon> ();
		sWeapon = secondaryWeapon.GetComponent<IWeapon> ();
	}

	// Update is called once per frame
	void Update () {
		if (Input.GetAxis ("Triggers1") < -0.2) {
			primaryFired = true;
		}
		if (Input.GetAxis ("Triggers1") > 0.2) {
			secondaryFired = true;
		}
	}

	void FixedUpdate () {
		if (pWeapon.CanFire () && primaryFired) {
			primaryFired = false;
			pWeapon.Fire ();
		}
		if (sWeapon.CanFire () && primaryFired) {
			secondaryFired = false;
			pWeapon.Fire ();
		}
	}

}
