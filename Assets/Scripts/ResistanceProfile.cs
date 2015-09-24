using UnityEngine;
using System.Collections;

[System.Serializable]
public class ResistanceProfile {

	/* The damage reduction works with the folowing formula
		FinalDamage = Damage * Resistance / (100 + Resitance) - Resistance * 0.01
	 */

	public enum DamageType {EXPLOSIVE, PIERCING, PLASMA, LASER}

	[SerializeField]
	private float resistanceExpolsive = 100;
	[SerializeField]
	private float resistanceLaser = 100;
	[SerializeField]
	private float resistancePlasma = 100;
	[SerializeField]
	private float resistancePiercing = 100;

	public float ExplosionResistance {
		get {return resistanceExpolsive;}
	}
	public float PiercingResistance {
		get {return resistancePiercing;}
	}
	public float PlasmaResistance {
		get {return resistancePlasma;}
	}
	public float LaserResistance {
		get {return resistanceLaser;}
	}
}
