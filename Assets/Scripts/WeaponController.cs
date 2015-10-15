using UnityEngine;
using System.Collections;

public class WeaponController : MonoBehaviour {

	public GameObject primaryWeaponMount;
	public GameObject secondaryWeaponMount;

	private IWeapon pWeapon;
	private IWeapon sWeapon;

	private bool primaryFired;
	private bool secondaryFired;

	// Use this for initialization
	void Start () {
		if (primaryWeaponMount != null) {
			pWeapon = primaryWeaponMount.GetComponentInChildren<IWeapon> ();
			print (pWeapon);
		}
		if (secondaryWeaponMount != null) {
			sWeapon = secondaryWeaponMount.GetComponentInChildren<IWeapon> ();
		}

		print ("FOUND WEAPON SYSTEMS" + pWeapon);
	}

	void FixedUpdate () {
		if (pWeapon != null) {
			if (primaryFired) {
				pWeapon.Fire ();
			}		
			primaryFired = false;
		}

		if (sWeapon != null) {
			if (primaryFired) {
				sWeapon.Fire ();
			}
			secondaryFired = false;
		}
	}

	public void FirePrimary () {
		primaryFired = true;
	}

	public void FireSecondary () {
		secondaryFired = true;
	}
}
