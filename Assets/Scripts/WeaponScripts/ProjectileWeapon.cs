using UnityEngine;
using System.Collections;

public class ProjectileWeapon : MonoBehaviour, IWeapon {

	public string name = null;

	[SerializeField]
	private float fireRate;
	private float coolDown;
	private int spawnIndex = 0;

	[SerializeField]
	private WeaponClass weaponClass;
	[SerializeField]
	private WeaponType weaponType;
	[SerializeField]
	private Transform weaponProjectile;
	private Transform[] projectileSpawn;
	private bool hasMultiSpawn = false;

	void Awake () {
		if (transform.FindChild ("ProjectileSpawn").GetComponent<ProjectileMultiSpawn> () != null) {
			projectileSpawn = transform.FindChild ("ProjectileSpawn").GetComponent<ProjectileMultiSpawn> ().SpawnPoint;
			hasMultiSpawn = true;
		} else {
			projectileSpawn = new Transform[] {transform.FindChild ("ProjectileSpawn")};
		}

		Debug.Log ("Has Multi Spawn: " + hasMultiSpawn + ", Num Spawns: " + projectileSpawn.Length);
	}

	void Update () {
		if (coolDown > 0) {
			this.coolDown -= Time.deltaTime;
		}
	}

	public bool CanFire () {
		if (coolDown <= 0) {
			return true;
		}
		return false;
	}

	public WeaponClass GetWeaponClass () {
		return this.weaponClass;
	}
	
	public WeaponType GetWeaponType () {
		return this.weaponType;
	}

	public void Fire () {
		if (coolDown <= 0) {
			coolDown = 1 / fireRate;
			Instantiate (weaponProjectile, projectileSpawn[spawnIndex].transform.position, transform.root.rotation);

			if (spawnIndex == projectileSpawn.Length -1) {
				spawnIndex = 0;
			} else {
				spawnIndex ++;
			}
		}
	}
}
