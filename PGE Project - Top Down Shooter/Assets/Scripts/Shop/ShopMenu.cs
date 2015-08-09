using UnityEngine;
using System.Collections;

public class ShopMenu : MonoBehaviour {

	public bool isPaused = false;
	public GameObject Rifle;
	public GameObject Pistol;
	public GameObject HeavyMachine;
	public GameObject SnipeRifle;
	public GameObject BlackBg;
	public GameObject Menu;
	public GameObject RifleIn;
	public GameObject PistolIn;

	public GameObject ActivateShop;
	public GameObject ExitShop;

	public GameObject Player;	
	public bool OnClicked = false;
	int PurchasedCount = 0;
	public bool RifleBought, PistolBought, SnipeBought, HeavyBought = false;
	public bool SelectPis;
	public bool SelectRif;

	
	// Use this for initialization
	void Start () {
		BlackBg.SetActive (false);
		RifleIn.SetActive (false);
		//ShopOff ();
	}
	
	// Update is called once per frame
	void Update () {
	
		if (isPaused == true) {
			BlackBg.SetActive(true);
			//Debug.Log("Passed in");
		}
	}

	public void ClickedOn(){
		OnClicked = true;
	}
	
	public void ClickedOff(){
		OnClicked = false;
	}

	public void ShopOff(){
		//BlackBg.SetActive(false);
		BlackBg.SetActive (false);
	}

	public void ShopShow(){
		//BlackBg.SetActive(true);
		BlackBg.SetActive (true);
	}

	public void SelectPistol(){
		SelectPis = true;
	}

	public void SelectRifle(){
		SelectRif = true;
	}

	public void PurchaseRifle(){

		//points minus;
		if (Player.GetComponent<PlayerInfo> ().points > 1) {
			Player.GetComponent<PlayerInfo> ().MinusPts (5);
			RifleBought = true;
			RifleIn.SetActive (true);
			if (Player.GetComponent<PlayerInfo> ().points <= 0) {
				Player.GetComponent<PlayerInfo>().points = 0;
			}
			}
		if (Player.GetComponent<PlayerInfo> ().points <= 0) {
			Player.GetComponent<PlayerInfo>().points = 0;
		}
	
	}

	public void PurchasePistol(){
		
		//points minus;
		PistolBought = true;
		 if (Player.GetComponent<PlayerInfo> ().points > 1) {
				Player.GetComponent<PlayerInfo> ().MinusPts (5);
			if (Player.GetComponent<PlayerInfo> ().points <= 0) {
				Player.GetComponent<PlayerInfo>().points = 0;
			}
		}
		if (Player.GetComponent<PlayerInfo> ().points <= 0) {
			Player.GetComponent<PlayerInfo>().points = 0;
		}
	}	

	public void PurchaseSnipeRifle(){
		
		//points minus;		
		if (Player.GetComponent<PlayerInfo> ().points > 1) {
		Player.GetComponent<PlayerInfo> ().MinusPts (5);
			if (Player.GetComponent<PlayerInfo> ().points <= 0) {
				Player.GetComponent<PlayerInfo>().points = 0;
			}
		}
		if (Player.GetComponent<PlayerInfo> ().points <= 0) {
			Player.GetComponent<PlayerInfo>().points = 0;
		}
		SnipeBought = true;
	}	

	public void PurchaseHeavyMachine(){
		
		//points minus;
		if (Player.GetComponent<PlayerInfo> ().points > 1) {
			Player.GetComponent<PlayerInfo> ().MinusPts (5);
			if (Player.GetComponent<PlayerInfo> ().points <= 0) {
				Player.GetComponent<PlayerInfo>().points = 0;
			}
		}
		if (Player.GetComponent<PlayerInfo> ().points <= 0) {
			Player.GetComponent<PlayerInfo>().points = 0;
		}
		HeavyBought = true;
	}
}
