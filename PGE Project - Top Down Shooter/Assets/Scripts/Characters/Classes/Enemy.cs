using UnityEngine;
using System.Collections;

public class Enemy : Unit {

	public short level = 1;					// may not be needed
	public float maxHP = 20;
	public float moveSpeed = 2.0f;			// speed when roaming
	public float alertMoveSpeed = 5.0f;		// speed when chasing/in combat
	public float dmg = 1;			
	public float armour = 0;				// (defense)
	public string type = "Default"; 		// default = normal enemy (shooter)
	public string name = "E_Shooter"; 	

	public float IDLE_DELAY = 3.0f;			// delay time for chging state from idle to roam

	private float hp;
	private float delay;					// idle delay

	private GameObject Player;	
	private HealthBar HPbar;	
	
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
		HPbar = this.GetComponent<HealthBar>();
		HPbar.maxHP = maxHP;

		hp = maxHP;
	}
	
	//Update is called once per frame
	void Update()
	{
		//Update from Parent Class
		this.StaticUpdate();

		// if player's bullet hit me
		if(theModel.other != null)
		{
			if(theModel.other.gameObject.tag == "bullet_player")		
			{
				hp -= 1;	//temp, chg to player's dmg (if we adding dmg in)
				Destroy(theModel.other.gameObject);
				theModel.other = null;
			}
		}

		if(hp <= 0)
		{
			hp = 0;
			Enemy_MState = FSM_M.mSTATE_DEAD;
			Destroy(this.gameObject);
			// add player's pts or sth
		}
		// update HP bar
		HPbar.hp = hp;

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
					Enemy_MState = FSM_M.mSTATE_CHASE;			 
				}
				else 												// Player not in my sight
				{
					//Debug.Log("mIDLE: " + delay + "s");
					delay -= Time.deltaTime;						
					
					if(delay <= 0)									// if Im done slacking,
					{
						delay = IDLE_DELAY;
						Enemy_MState = FSM_M.mSTATE_ROAM;			 
					}
				}
				break;

// #IDLE STATE
// ===================================================================================================================
// #ROAM STATE

			case FSM_M.mSTATE_ROAM:										// I(Enemy) roam around waypts to find player
			Debug.Log(this.theModel.WalkCollisionRegion.inRng_Chase);
				if(this.theModel.WalkCollisionRegion.inRng_Chase)		// if Player/objective in my sight		
				{
					Enemy_MState = FSM_M.mSTATE_CHASE;					 
				}
				else 													// Player went out of my sight		
				{
					Debug.Log("mROAM");									 
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
						Enemy_MState = FSM_M.mSTATE_COMBAT;				 
					}
					else 												// Player/objective not in my rng yet
					{													 
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
					Enemy_MState = FSM_M.mSTATE_IDLE;					 
				}
				break;

// #CHASE STATE
// ===================================================================================================================
// #COMBAT STATE

			case FSM_M.mSTATE_COMBAT:								// in combat with player
				// movement codes here
				// ... 

				EnemyCombatFSM();
				break;

// #COMBAT STATE
// ===================================================================================================================
// #FLEE STATE

			case FSM_M.mSTATE_FLEE:									// flee from player
				Debug.Log("mFLEE");				
				break;

// #FLEE STATE
// ===================================================================================================================
// #DEAD STATE

			case FSM_M.mSTATE_DEAD:
				Debug.Log("mDEAD");
				Enemy_CState = FSM_C.cSTATE_NULL;	
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
				{													 
					if(this.UnitType == UType.UNIT_E_BOMBER)
						Enemy_CState = FSM_C.cSTATE_BOMB;
					else
						Enemy_CState = FSM_C.cSTATE_SHOOT;
				}
				break;

// #NULL STATE
// ===================================================================================================================
// #SHOOT STATE

			case FSM_C.cSTATE_SHOOT:			// when I(Enemy) is in fire range with player (shooter types only)

				if(this.theModel.WalkCollisionRegion.inRng_Fire)	// if Player/objective still in my rng	
				{
					Debug.Log("cSHOOT");
					
					// if I choose to flee, switch cstate to null, mstate to flee
				}
				else 												// Player/objective went out of my rng	
				{
					Enemy_CState = FSM_C.cSTATE_NULL;				 
					Enemy_MState = FSM_M.mSTATE_CHASE;				 
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
