using UnityEngine;
using System.Collections;

public class ProjectileWeapon : MonoBehaviour, IWeapon {

	public string name = null;

	[SerializeField]
	private float fireRate;
	[SerializeField]
	private float accuracy;
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

	public bool CanFire {
		get {return (coolDown <= 0) ? true : false;}
	}
	
	public WeaponClass GetWeaponClass {
		get {return weaponClass;}
	}
	
	public WeaponType GetWeaponType {
		get {return weaponType;}
	}

	void Awake () {
		if (transform.FindChild ("ProjectileSpawn").GetComponent<ProjectileMultiSpawn> () != null) {
			projectileSpawn = transform.FindChild ("ProjectileSpawn").GetComponent<ProjectileMultiSpawn> ().SpawnPoint;
			hasMultiSpawn = true;
		} else {
			projectileSpawn = new Transform[] {transform.FindChild ("ProjectileSpawn")};
		}

		//Debug.Log ("Has Multi Spawn: " + hasMultiSpawn + ", Num Spawns: " + projectileSpawn.Length);
	}

	void Update () {
		if (coolDown > 0) {
			this.coolDown -= Time.deltaTime;
		}
	}



	public void Fire () {
		if (coolDown <= 0) {
			Transform go;
			coolDown = 1 / fireRate;
			go = (Transform) Instantiate (weaponProjectile, projectileSpawn[spawnIndex].transform.position, transform.rotation);

			if (spawnIndex == projectileSpawn.Length -1) {
				spawnIndex = 0;
			} else {
				spawnIndex ++;
			}
		}
	}
}
