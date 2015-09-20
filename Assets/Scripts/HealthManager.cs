using UnityEngine;
using System.Collections;

public class HealthManager : MonoBehaviour, IDamageable, ILiving {

	[SerializeField]
	private float maxHealth = 100;
	[SerializeField]
	private float currentHealth;
	private bool isAlive = true;
	public bool isPlayer = false;

	[SerializeField]
	private ResistanceProfile resistanceProfile = new ResistanceProfile ();

	void Start () {
		currentHealth = maxHealth;
	}

	void Update () {
		if (currentHealth <= 0) {
			isAlive = false;

			if (!isPlayer) {
				Destroy (this.gameObject);
			}
		}
	}

	public void Damage (DamageProfile dp) {
		currentHealth -= CalculateDamage (resistanceProfile, dp, 1);
	}

	public void DamageWithFallOff (DamageProfile dp, float reduction) {
		if (1 - reduction > 0) {
			currentHealth -= CalculateDamage (resistanceProfile, dp, 1 - reduction);
		}
	}

	public bool Respawn () {
		isAlive = true;
		return true;
	}

	public bool IsAlive () {
		return isAlive;
	}

	private static float CalculateDamage (ResistanceProfile rp, DamageProfile dp, float multip) {
		float damage = 0;
		float explosionDamage = dp.ExplosionDamage, laserDamage = dp.LaserDamage, plasmaDamage = dp.PlasmaDamage, piercingDamage = dp.PiercingDamage;
		if (explosionDamage > 0) {
			damage += explosionDamage * (rp.ExplosionResistance / (100 + rp.ExplosionResistance));
		}
		if (laserDamage > 0) {
			damage += laserDamage * (rp.LaserResistance / (100 + rp.LaserResistance));
		}
		if (plasmaDamage > 0) {
			damage += plasmaDamage * (rp.PlasmaResistance / (100 + rp.PlasmaResistance));
		}
		if (piercingDamage > 0) {
			damage += piercingDamage * (rp.PiercingResistance / (100 + rp.PlasmaResistance));
		}
		Debug.Log ("Damage dealt: " + damage);
		return damage * multip;
	}
}
