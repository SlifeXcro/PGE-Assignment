using UnityEngine;
using System.Collections;

public class EnemyStats : MonoBehaviour {
	
	public short level 			= 1;				// may not be needed
	public float maxHP 			= 20;
	public float moveSpeed 		= 2.0f;				// speed when roaming
	public float alertMoveSpeed = 5.0f;				// speed when chasing/in combat
	public float dmg 			= 1;			
	public float armour 		= 0;				// (defense)
	public float IDLE_DELAY 	= 3.0f;				// delay time for chging state from idle to roam
	public float FIRE_RATE		= 0.2f;			
	public float tolLength		= 2.0f;				// length to determine whether AI has reach waypt
	public float strafeLength	= 5.0f;				// strafe length max dist when in combat

	//public string type 			= "Default"; 		// default = normal enemy (shooter)
	//public string name 			= "E_Shooter"; 	

	void Start () 
	{
	}

	void Update () 
	{
	}
}
