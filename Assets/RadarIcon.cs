using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class RadarIcon : MonoBehaviour {

	public Sprite radarImage;

	void Start () {
		RadarDrawer.AddIcon (this);
	}

	void OnDestroy () {
		RadarDrawer.RemoveIcon (this);
	}
}
