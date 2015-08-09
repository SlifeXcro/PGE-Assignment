using UnityEngine;
using System.Collections;

public class Weapon : MonoBehaviour {

	public enum WPNtype{
		PISTOL,
		SNIPERIFLE,
		RIFLE,
		HEAVYMACHINEGUN
	}

	//public WPNtype type = WPNtype.PISTOL;
	public float RPM = 1.0f;
	public float BulletCount; 
	public float FireRate;
	ShopMenu SHPM;
	public bool PistolUsed = true;
	public bool RifleUsed = false;
	Timer.TimeBundle FireTimer;

	// Use this for initialization
	void Start () {
		
		PistolOn ();
	
	}
	
	// Update is called once per frame
	void Update () {
	
		if (SHPM.SelectPis == true) {
				PistolOn ();
		} else if (SHPM.SelectRif == true) {
				RifleOn();
		}
	}

	public void PistolOn(){
		WPNtype type = WPNtype.PISTOL;
		PistolUsed = true;
	}

	public void RifleOn(){
		WPNtype type = WPNtype.RIFLE;
		RifleUsed = true;
	}
	public void SnipeOn(){
		WPNtype type = WPNtype.SNIPERIFLE;
	}
	public void HeavyMachineOn(){
		WPNtype type = WPNtype.HEAVYMACHINEGUN;
	}

}
