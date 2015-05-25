using UnityEngine;
using System.Collections;

public class Enemy : Unit {

	public short level = 1;					// may not be needed
	public float moveSpeed = 2.0f;			// speed when roaming
	public float alertMoveSpeed = 5.0f;		// speed when chasing/in combat
	public float maxHP = 50;
	public float dmg = 1;			
	public float armour = 0;				// (defense)
	public string type = "Default"; 		// default = normal enemy (shooter)
	public string name = "E_Shooter"; 	

	public float IDLE_DELAY = 3.0f;			// delay for chging state from idle to roam
	private float delay;

	private GameObject Player;	
	
	private enum FSM_M		// movement FSM
	{
		mSTATE_IDLE,
		mSTATE_ROAM,		// enemy roam around waypts to find player
		mSTATE_CHASE,		// enemy targets and chases player or generator 
		mSTATE_COMBAT,		// enemy in combat with player
		mSTATE_FLEE,		// enemy flee from player
		mSTATE_DEAD
	} private FSM_M Enemy_MState = FSM_M.mSTATE_IDLE;
	
	private enum FSM_C		// combat FSM
	{
		cSTATE_NULL,		// used when enemy mState is not in combat state
		cSTATE_SHOOT,		// when enemy in range with player
		cSTATE_BOMB
	} private FSM_C Enemy_CState = FSM_C.cSTATE_NULL;

	public void RandomizeStats()
	{
		Debug.Log("Enemy Shooter Stats Inited.");
		Stats.Set(level, maxHP, dmg, armour, 0, 0, 0, type, "E_Shooter");
	}
	
	//Use this for initialization
	void Start()
	{
		//Class has been inherited
		Inherited = true;
		
		//Init from Parent Class
		this.Init();
		
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

		delay = IDLE_DELAY;

		Player = GameObject.FindGameObjectWithTag("Player");
	}
	
	//Update is called once per frame
	void Update()
	{
		//Update from Parent Class
		this.StaticUpdate();

		EnemyMovementFSM();
	}

	void EnemyMovementFSM()
	{
		switch(Enemy_MState)
		{
// ===================================================================================================================
// #IDLE STATE

			case FSM_M.mSTATE_IDLE:
				
				if(this.theModel.WalkCollisionRegion.inRng_Chase)	// if Player in my sight		
				{
					delay = IDLE_DELAY;
					Enemy_MState = FSM_M.mSTATE_CHASE;				// I chase
				}
				else 												// Player not in my sight
				{
					//Debug.Log("mIDLE: " + delay + "s");
					delay -= Time.deltaTime;						
					
					if(delay <= 0)									// if Im done slacking,
					{
						delay = IDLE_DELAY;
						Enemy_MState = FSM_M.mSTATE_ROAM;			// I roam ard
					}
				}
				break;

// #IDLE STATE
// ===================================================================================================================
// #ROAM STATE

			case FSM_M.mSTATE_ROAM:										// I(Enemy) roam around waypts to find player
				
				if(this.theModel.WalkCollisionRegion.inRng_Chase)		// if Player/objective in my sight		
				{
					Enemy_MState = FSM_M.mSTATE_CHASE;					// I chase
				}
				else 													// Player went out of my sight		
				{
					Debug.Log("mROAM");									// I roam
				}
				break;

// #ROAM STATE
// ===================================================================================================================
// #CHASE STATE

			case FSM_M.mSTATE_CHASE:									// I(Enemy) targets and chases player/objective
				
				if(this.theModel.WalkCollisionRegion.inRng_Chase)		// if Player/objective still in my sight		
				{
					if(this.theModel.WalkCollisionRegion.inRng_Fire)	// if Player/objective in my rng	
					{
						Enemy_MState = FSM_M.mSTATE_COMBAT;				// I fight/do sth
					}
					else 												// Player/objective not in my rng yet
					{													// I ctn chase 
						if(this.UnitType == UType.UNIT_E_BOMBER)		
							Debug.Log("mOBJECTIVE FOUND, HEADING THR...");
						else
						{
							Debug.Log("mPLAYER IN SIGHT, CHASING...");
							// move to player
							this.transform.position = Vector2.MoveTowards(this.transform.position, Player.transform.position,
						                                     	 alertMoveSpeed*Time.deltaTime);
						}
					}
				}
				else 													// Player went out of my sight		
				{
					Enemy_MState = FSM_M.mSTATE_IDLE;					// I slack, then roam
				}
				break;

// #CHASE STATE
// ===================================================================================================================
// #COMBAT STATE

			case FSM_M.mSTATE_COMBAT:								// Enemy in combat with player
				// movement codes here
				// ... 

				EnemyCombatFSM();
				break;

// #COMBAT STATE
// ===================================================================================================================
// #FLEE STATE

			case FSM_M.mSTATE_FLEE:									// Enemy flee from player
				Debug.Log("mFLEE");				
				break;

// #FLEE STATE
// ===================================================================================================================
// #DEAD STATE

			case FSM_M.mSTATE_DEAD:
				Debug.Log("mDEAD");
				break;

// #DEAD STATE
// ===================================================================================================================
// #DEFAULT
			default:
				Enemy_MState = FSM_M.mSTATE_IDLE;	
				break;
		}
	}
	
	void EnemyCombatFSM()
	{
		switch(Enemy_CState)
		{
// ===================================================================================================================
// #NULL STATE

			case FSM_C.cSTATE_NULL:	

				if(this.theModel.WalkCollisionRegion.inRng_Fire)	// if Player/objective enters my rng	
				{													// I shoot/destroy
					if(this.UnitType == UType.UNIT_E_BOMBER)
						Enemy_CState = FSM_C.cSTATE_BOMB;
					else
						Enemy_CState = FSM_C.cSTATE_SHOOT;
				}
				break;

// #NULL STATE
// ===================================================================================================================
// #SHOOT STATE

			case FSM_C.cSTATE_SHOOT:			// when I(Enemy) is in range with player (shooter types only)

				if(this.theModel.WalkCollisionRegion.inRng_Fire)	// if Player/objective still in my rng	
				{
					Debug.Log("cSHOOT");
					
					// if I choose to flee, switch cstate to null, mstate to flee
				}
				else 												// Player/objective went out of my rng	
				{
					Enemy_CState = FSM_C.cSTATE_NULL;				// I stop firing
					Enemy_MState = FSM_M.mSTATE_CHASE;				// I chase
				}
				break;

// #SHOOT STATE
// ===================================================================================================================
// #BOMB STATE

			case FSM_C.cSTATE_BOMB:	 			// when I(Enemy) is in range with obj. (bomber types only)
				Debug.Log("cBOMB");
				break;

// #BOMB STATE
// ===================================================================================================================
// #DEFAULT

			default:
				Enemy_CState = FSM_C.cSTATE_NULL;	
				break;
		}
	}


}
