using UnityEngine;
using System.Collections;

public class Weapon : MonoBehaviour {

	public enum WPNtype{
		PISTOL,
		MACHINEGHUN,
		SHOTGUN
	}

	public WPNtype type = WPNtype.PISTOL;
	public float RPM = 1.0f;


	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
