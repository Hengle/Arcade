using UnityEngine;
using System.Collections;

public interface IWeapon {

	WeaponClass GetWeaponClass ();

	WeaponType GetWeaponType ();

	void Fire ();
}
