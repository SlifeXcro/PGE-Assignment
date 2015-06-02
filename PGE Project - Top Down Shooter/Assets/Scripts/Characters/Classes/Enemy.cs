using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Enemy : Unit {

	// Stats
	public short level 			= 1;				// may not be needed
	public float maxHP 			= 20;
	public float moveSpeed 		= 2.0f;				// speed when roaming
	public float alertMoveSpeed = 5.0f;				// speed when chasing/in combat
	public float dmg 			= 1;			
	public float armour 		= 0;				// (defense)
	public string type 			= "Default"; 		// default = normal enemy (shooter)
	public string name 			= "E_Shooter"; 	

	public float IDLE_DELAY 	= 3.0f;				// delay time for chging state from idle to roam
	public float FIRE_RATE		= 0.2f;			

	public float tolLength		= 2.0f;				// length to determine whether AI has reach waypt
	public float strafeLength	= 5.0f;				// strafe length max dist when in combat

	// Waypoints
	public List<Transform> waypointList = new List<Transform>();

	int nextWaypt;							// stores next waypoint location
	float hp;
	float delay;							// idle delay
	
	float[] random_values = new float[4];	// to store random values (chance) of strafe movement in combat
	Vector2 strafeTargetPos;				// strafing move pos when in combat
	Firing firingScript;

	GameObject Player;	

	// UI
	HealthBar HPbar;	

	// movement FSM
	private enum FSM_M		
	{
		IDLE,
		ROAM,		 
		CHASE,		 
		COMBAT,		 
		FLEE,		 
		DEAD
	} private FSM_M mState = FSM_M.IDLE;

	// combat FSM
	private enum FSM_C		
	{
		NULL,		// used when enemy is not in combat state (i.e. not in fire rng)
		SHOOT,		 
		BOMB
	} private FSM_C cState = FSM_C.NULL;


	void Start()
	{
		//Class has been inherited
		Inherited = true;
		
		//Init from Parent Class
		this.Init();
	
		Player = GameObject.FindGameObjectWithTag("Player");

		delay = IDLE_DELAY;
		
		hp = maxHP;
		HPbar = this.GetComponent<HealthBar>();
		HPbar.maxHP = maxHP;

		firingScript = this.GetComponentInChildren<Firing>();

		
		// Spawn pos (temp)
		nextWaypt = 0;
		transform.position = waypointList[nextWaypt].position;		
		
		//Stats.Set(level, maxHP, dmg, armour, 0, 0, 0, type, "E_Shooter");
		
		//Init Unit's Type
		switch(type)
		{
			case "Bomber":
				this.UnitType = UType.UNIT_E_BOMBER;
				break;
				
			default:
				this.UnitType = UType.UNIT_E_SHOOTER;
				break;
		}	
	}


	void Update()
	{
		//Update from Parent Class
		this.StaticUpdate();

		// sprite's collision
		if(theModel.WalkCollisionRegion.HitboxTrigger)
		{
			// (bullet's owner check done in collision script)
			//if(theModel.other.gameObject.tag == "bullet_player")		
			//{
				hp -= 1;	//temp, chg to player's dmg (if we adding dmg in)
				theModel.WalkCollisionRegion.setHitboxTriggerFalse();
				//(bullet destroyed in coll script too)
				//Destroy(theModel.other.gameObject);
				//theModel.other = null;
			//}
		}

		if(hp <= 0)
		{
			hp = 0;
			mState = FSM_M.DEAD;
			Destroy(this.gameObject);
			// add player's pts or sth
		}

		// update HP bar
		HPbar.hp = hp;

		EnemyMovementFSM();
	}

	void EnemyMovementFSM()
	{
		switch(mState)
		{
			case FSM_M.IDLE:
				if(this.theModel.WalkCollisionRegion.inRng_Chase)	// if Player in sight		
				{
					delay = IDLE_DELAY;
					mState = FSM_M.CHASE;			 
				}
				else 												// Player not in sight
				{
					//Debug.Log("mIDLE: " + delay + "s");
					delay -= Time.deltaTime;						
					
					if(delay <= 0)								
					{
						delay = IDLE_DELAY;
						mState = FSM_M.ROAM;			 
					}
				}
				break;

			case FSM_M.ROAM:										
				if(this.theModel.WalkCollisionRegion.inRng_Chase)		// if Player/objective in sight		
				{
					mState = FSM_M.CHASE;					 
				}
				else 													// Player went out of sight		
				{
					if(waypointList.Count > 1)
					{
						this.transform.position = Vector2.MoveTowards(this.transform.position, 
					                                              	  waypointList[nextWaypt].position,
						                                              moveSpeed*Time.deltaTime);
						
						// if AI reaches targeted waypt
						if( (waypointList[nextWaypt].position-transform.position).sqrMagnitude < tolLength )
						{
							++nextWaypt;				// move to next waypt
							if(nextWaypt > waypointList.Count-1)
							{
								mState = FSM_M.IDLE;
								nextWaypt = 0;
							}
						}
					}
				}
				break;

			case FSM_M.CHASE:									
				if(this.theModel.WalkCollisionRegion.inRng_Chase)		// if Player/objective still in sight		
				{
					if(this.theModel.WalkCollisionRegion.inRng_Fire)	// if Player/objective in rng	
					{
						delay = 0;
						mState = FSM_M.COMBAT;	
						
						Vector2 playerPos = new Vector2(Player.transform.position.x, Player.transform.position.y);
						if((strafeTargetPos-playerPos).magnitude > strafeLength*strafeLength)
							strafeTargetPos	= new Vector2(this.transform.position.x, this.transform.position.y);
					}
					else 												// Player/objective not in rng yet
					{													 
						if(this.UnitType == UType.UNIT_E_BOMBER)		
							Debug.Log("mOBJECTIVE FOUND, HEADING THR...");
						else
						{
							Debug.Log("mPLAYER IN SIGHT, CHASING...");
							// move to player
							this.transform.position = Vector2.MoveTowards(this.transform.position, 
						                                              	  Player.transform.position,
						                                     	 		  alertMoveSpeed*Time.deltaTime);
						}
					}
				}
				else 													// Player went out of sight		
				{
					mState = FSM_M.IDLE;					 
				}
				break;

			case FSM_M.COMBAT:								// in combat with player
				// strafe movement randomization
				for(int i = 0; i < random_values.Length; ++i)
				{
					random_values[i] = Random.value;
				}
				if(random_values[0] < 0.05)	// 5% chance of strafing rightwards
				{
					strafeTargetPos.x += 10;	
				}
				if(random_values[1] < 0.05)	// 5% chance of strafing leftwards
				{
					strafeTargetPos.x -= 10;	
				}
				if(random_values[2] < 0.05)	// 5% chance of strafing upwards
				{
					strafeTargetPos.y += 10;	
				}
				if(random_values[3] < 0.05)	// 5% chance of strafing downwards
				{
					strafeTargetPos.y -= 10;	
				}

				// strafe movement 
				this.transform.position = Vector2.MoveTowards(this.transform.position,
			                                               	  strafeTargetPos,
				                                              alertMoveSpeed*Time.deltaTime);
				EnemyCombatFSM();
				break;

			case FSM_M.FLEE:									
				Debug.Log("mFLEE");				
				break;

			case FSM_M.DEAD:
				Debug.Log("mDEAD");
				cState = FSM_C.NULL;	
				break;

			default:
				mState = FSM_M.IDLE;	
				break;
		}
	}
	
	void EnemyCombatFSM()
	{
		switch(cState)
		{
			case FSM_C.NULL:											 
				if(this.UnitType == UType.UNIT_E_BOMBER)
					cState = FSM_C.BOMB;
				else
					cState = FSM_C.SHOOT;
				break;

			case FSM_C.SHOOT:			
				if(this.theModel.WalkCollisionRegion.inRng_Fire)	// if Player/objective still in rng	
				{
					delay -= Time.deltaTime;						
					
					if(delay <= 0)									// fire rate ctrl
					{
						delay = FIRE_RATE;
						firingScript.Fire(false, Player.transform.position);
					}
					// if I choose to flee, switch cstate to null, mstate to flee
				}
				else 												// Player/objective went out of rng	
				{
					cState = FSM_C.NULL;				 
					mState = FSM_M.CHASE;				 
				}
				break;

			case FSM_C.BOMB:	 			
				Debug.Log("cBOMB");
				break;

			default:
				cState = FSM_C.NULL;	
				break;
		}
	}
}
