using UnityEngine;
using System.Collections;

public class Crosshair : MonoBehaviour {
	
	public LayerMask enemyMask;
	public LayerMask interactionMask;
	public LayerMask friendlyMask;
	
	public Texture crosshairRed;
	public Texture crosshairWhite;
	public Texture crosshairYellow;
	public Texture crosshairGreen;
	
	public float width;
	public float height;

	public float enemydistance;
	public float interactiondistance;
	public float friendlydistance;
	
	private Texture currentTex;
	
	
	
	void Start ()
	{
		Cursor.visible = false;
		Screen.lockCursor = true;
	}
	
	
	void Update ()
	{
		
		RaycastHit hit;
		Ray ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2,Screen.height / 2,0));
		LayerMask hitMask = 1 << 8;
		hitMask = ~hitMask;
		
		if(Physics.Raycast(ray,out hit,enemydistance,enemyMask))
		{
			currentTex = crosshairRed;
		}
		
		else if(Physics.Raycast(ray,out hit,interactiondistance,interactionMask))
		{
			currentTex = crosshairYellow;
		}
		
		else if(Physics.Raycast(ray,out hit,friendlydistance,friendlyMask))
		{
			currentTex = crosshairGreen;
		}
				
		else
		{
			currentTex = crosshairWhite;
		
		}
	}	
	
		void OnGUI ()
	{
		GUI.DrawTexture(new Rect(Screen.width / 2 - width / 2,Screen.height / 2 - height / 2,width,height),currentTex);
	}
	
}
