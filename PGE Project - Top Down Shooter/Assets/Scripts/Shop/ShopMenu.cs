using UnityEngine;
using System.Collections;

public class ShopMenu : MonoBehaviour {

	public bool isPaused = false;
	public GameObject Rifle;
	public GameObject Pistol;
	public GameObject BlackBg;
	public GameObject Menu;

	public GameObject Player;	

	int PurchasedCount = 0;

	
	// Use this for initialization
	void Start () {
		BlackBg.SetActive (false);
		//ShopOff ();
	}
	
	// Update is called once per frame
	void Update () {
	
		if (isPaused == true) {
			BlackBg.SetActive(true);
			//Debug.Log("Passed in");
		}
	}

	public void ShopOff(){
		//BlackBg.SetActive(false);
		BlackBg.SetActive (false);
	}

	public void ShopShow(){
		//BlackBg.SetActive(true);
		BlackBg.SetActive (true);
	}

	public void PurchaseRifle(){

		//points minus;
		Player.GetComponent<PlayerInfo>().MinusPts (5);
	}

	public void PurchasePistol(){
		
		//points minus;
		Player.GetComponent<PlayerInfo>().MinusPts (5);
	}	

	public void PurchaseSnipeRifle(){
		
		//points minus;
		Player.GetComponent<PlayerInfo>().MinusPts (5);
	}	

	public void PurchaseHeavyMachine(){
		
		//points minus;
		Player.GetComponent<PlayerInfo>().MinusPts (5);
	}
}
