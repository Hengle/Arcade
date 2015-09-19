using UnityEngine;
using System.Collections;

public class ProjectileWeapon : MonoBehaviour, IWeapon {

	[SerializeField]
	protected float fireRate;
	protected float coolDown;

	protected float Damage;

	[SerializeField]
	protected WeaponClass weaponClass;
	[SerializeField]
	protected WeaponType weaponType;
	[SerializeField]
	protected Transform weaponProjectile;
	[SerializeField]
	protected Transform projectileSpawn;

	public WeaponClass GetWeaponClass () {
		return this.weaponClass;
	}
	
	public WeaponType GetWeaponType () {
		return this.weaponType;
	}

	public void Fire () {
		Instantiate (this.weaponProjectile, projectileSpawn.transform.position, Quaternion.identity);
	}
}
