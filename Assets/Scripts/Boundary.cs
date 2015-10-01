using UnityEngine;
using System.Collections;

[System.Serializable]
public class Boundary {
	
	public float xMin, xMax, yMin, yMax, zMin, zMax;

	public Boundary (float xMin, float xMax, float yMin, float yMax, float zMin, float zMax) {
		this.xMin = xMin;
		this.zMax = xMax;
		this.yMin = yMin;
		this.yMax = yMax;
		this.zMin = zMin;
		this.zMax = zMax;
	}
}
