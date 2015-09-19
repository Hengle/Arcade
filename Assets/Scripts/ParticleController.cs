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
		rb = GetComponent<Rigidbody> ();
		ip = GetComponent<InputController> ();
		exhaust1 = exhaust.transform.FindChild ("Pipe0").FindChild ("ExhaustSmoke").GetComponent<ParticleSystem> ();
		exhaust2 = exhaust.transform.FindChild ("Pipe1").FindChild ("ExhaustSmoke").GetComponent<ParticleSystem> ();
	}
	
	// Update is called once per frame
	void Update () {
		exhaustRate = exhaustBaseRate + exhaustBaseRate * (ip.InputVerical * ip.InputVerical);
		exhaustRate *= exhaustRate / exhaustBaseRate;

		exhaust1.emissionRate = exhaustRate;
		exhaust2.emissionRate = exhaustRate;
	}
}
