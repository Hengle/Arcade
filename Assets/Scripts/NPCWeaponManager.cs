using UnityEngine;
using System.Collections;

public class NPCWeaponManager : MonoBehaviour {

	public GameObject[] weaponMounts;
	private IWeapon[] weaponSystems;

	public bool testWeapons = false;

	// Use this for initialization
	void Start () {
		weaponSystems = new IWeapon[weaponMounts.Length];

		for (int i = 0; i < weaponMounts.Length; i++) {
			weaponSystems[i] = weaponMounts[i].transform.GetChild (0).GetComponent<IWeapon> ();
		}

		if (testWeapons) {
			StartCoroutine ("WeaponTest");
		}
	}

	public void Fire () {
		Ray ray = new Ray (transform.position, transform.forward);
		RaycastHit hit;
		Debug.DrawLine (transform.position, transform.position + transform.forward * 80, Color.red);

		if (Physics.Raycast (ray, out hit, 80)) {
			if (hit.transform.tag.Equals ("Player")) {
				foreach (IWeapon wep in weaponSystems) {
					
					wep.Fire ();
				}
			}
		}
	}

	IEnumerator WeaponTest () {
		while (true) {
			weaponSystems[0].Fire ();
			yield return new WaitForSeconds (0.1f);
		}
	}

}
