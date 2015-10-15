using UnityEngine;
using System.Collections;

public interface IWeapon {

	bool CanFire  {
		get;
	}
	
	WeaponClass GetWeaponClass {
		get;
	}

	WeaponType GetWeaponType {
		get;
	}

	void Fire ();

}
