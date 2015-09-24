using UnityEngine;
using System.Collections;

public class ParticleController : MonoBehaviour {

	// References
	Rigidbody rb;
	InputController ip;
	public GameObject exhaust;

	ParticleSystem exhaust1, exhaust2;
	private float exhaustRate = 7f;
	private float exhaustBaseRate = 7f;

	// Use this for initialization
	void Start () {
		this.rb = GetComponent<Rigidbody> ();
		this.ip = GetComponent<InputController> ();
		this.exhaust1 = exhaust.transform.FindChild ("Pipe0").FindChild ("ExhaustSmoke").GetComponent<ParticleSystem> ();
		this.exhaust2 = exhaust.transform.FindChild ("Pipe1").FindChild ("ExhaustSmoke").GetComponent<ParticleSystem> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (ip != null) {
			this.exhaustRate = exhaustBaseRate + exhaustBaseRate * (ip.InputVerical * ip.InputVerical);
			this.exhaustRate *= exhaustRate / exhaustBaseRate;
		}
		this.exhaust1.emissionRate = exhaustRate;
		this.exhaust2.emissionRate = exhaustRate;
	}
}
