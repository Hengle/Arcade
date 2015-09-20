using UnityEngine;
using System.Collections;

public interface IWeapon {

	bool CanFire ();
	
	WeaponClass GetWeaponClass ();

	WeaponType GetWeaponType ();

	void Fire ();

}
