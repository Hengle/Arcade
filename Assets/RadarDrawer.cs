using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RadarDrawer : MonoBehaviour {

	private static List<RadarIcon> radarIcons = new List<RadarIcon> ();

	public int radarRange = 1000;
	public Quaternion defaultRotation;
	public Transform radarCenter;

	private Quaternion rotationOffset;

	void OnStartGame () {
		radarCenter = GameObject.FindWithTag ("Player").transform;
		defaultRotation = transform.rotation;
	}

	void LateUpdate () {
		//rotationOffset = radarCenter.transform.rotation;
		transform.rotation = defaultRotation * rotationOffset;
	}

	public static void AddIcon (RadarIcon rIcon) {
		radarIcons.Add (rIcon);
	}

	public static void RemoveIcon (RadarIcon rIcon) {
		radarIcons.Remove (rIcon);
	}
}
