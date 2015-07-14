using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Enemy : Unit {
	
	public float hp;	

	bool followingPath;
	int nextWaypt;							// stores next waypoint location
	int targetIndex;
	float delay;							// idle delay
	
	float[] random_values = new float[4];	// to store random values (chance) of strafe movement in combat
	Vector3 strafeTargetPos;				// strafing move pos when in combat

	Vector3 targetCurrentPos;	
	Vector3[] path = new Vector3[0];		// path to travel to target(player/generator)
	
	public List<Transform> waypointList = new List<Transform>();
	
	Transform Target;
	Transform Player;	
	EnemyStats enemyStats;
	Firing firingScript;


	private enum FSM_M		// movement FSM
	{
		SPAWN,
		IDLE,
		ROAM,		 
		CHASE,		 
		COMBAT,		 
		FLEE,		 
		DEAD
	} private FSM_M mState = FSM_M.SPAWN;


	private enum FSM_C		// combat FSM	
	{
		NULL,		// used when enemy is not in combat state (i.e. not in fire rng)
		SHOOT,		 
		DESTROY	
	} private FSM_C cState = FSM_C.NULL;


	void Start()
	{
		Inherited = true;	//Class has been inherited
		this.Init();		//Init from Parent Class
		
		Player = GameObject.FindGameObjectWithTag("Player").transform;
		enemyStats = transform.GetComponentInChildren<EnemyStats>();
		firingScript = this.GetComponent<Firing>();

		delay = enemyStats.IDLE_DELAY;
		hp = enemyStats.maxHP;
	
		nextWaypt = 0;	
		targetIndex = 0;
		followingPath = false;
		if(Target != null)
		{
			targetCurrentPos = Target.position;
			PathRequestManager.RequestPath(transform.position, Target.position, OnPathFound);
		}
		else 
		{
			// since AI dont have any target assigned to it, target the 1st waypt instead
			if(waypointList.Count > 0)
			{
				Target = waypointList[nextWaypt];	
				targetCurrentPos = Target.position;
				PathRequestManager.RequestPath(transform.position, Target.position, OnPathFound);
			}
		}

		// (coming out from door)
		//this.GetComponent<BoxCollider>().enabled = false;
	}

	public void SetTarget(Transform _target)
	{
		//StopCoroutine("FollowPath");
		this.Target = _target;
		targetCurrentPos = Target.position;

		PathRequestManager.RequestPath(transform.position, Target.position, OnPathFound);
	}

	void Update()
	{
		this.StaticUpdate();	// Update from Parent Class

		HitBoxUpdate();

		if(mState != FSM_M.COMBAT)
		{
			if(Target != null)
			{
				if(targetCurrentPos != Target.position || !followingPath)			// if target moves
				{
					targetCurrentPos = Target.position;								// update its curr pos
					PathRequestManager.RequestPath(transform.position, 				// request path agn
					                               Target.position, OnPathFound);	
				}
			}
			else 
			{
				if(waypointList.Count > 1)
				{
					targetIndex = 0;
					Target = waypointList[nextWaypt];	
					targetCurrentPos = Target.position;
					PathRequestManager.RequestPath(transform.position, Target.position, OnPathFound);
				}
			}
		}

		MovementFSM();
	}

	void MovementFSM()
	{
		switch(mState)
		{
			case FSM_M.SPAWN:
				if(waypointList.Count > 0)
				{
					transform.position = Vector3.MoveTowards(transform.position, 
					                                         waypointList[nextWaypt].position, 
					                                         enemyStats.moveSpeed*Time.deltaTime);
					// once reached start pos
					if((waypointList[nextWaypt].position-transform.position).sqrMagnitude < enemyStats.tolLength)
					{
						//this.GetComponent<BoxCollider>().enabled = true;
						mState = FSM_M.ROAM;
					}
				}
				break;

			case FSM_M.IDLE:
				if(this.UnitType != UType.UNIT_E_DESTROYER)
				{
					if(this.theModel.CollisionRegion.inRng_Chase)		// if Player in sight		
					{
						if(this.UnitType != UType.UNIT_E_DESTROYER)
						Target = Player;
						mState = FSM_M.CHASE;			 
					}
				}

				delay -= Time.deltaTime;						
				
				if(delay <= 0)								
				{
					delay = enemyStats.IDLE_DELAY;
					mState = FSM_M.ROAM;			 
				}
				break;

			case FSM_M.ROAM:	
				if(this.UnitType != UType.UNIT_E_DESTROYER)
				{
					if(this.theModel.CollisionRegion.inRng_Chase)	// if Player in sight		
					{	
						Target = Player;
						mState = FSM_M.CHASE;	
					}
				}
				else
				{
					if(theModel.CollisionRegion.inRng_Fire)		// if in fire range with generator
					{
						if(!theModel.CollisionRegion.other.GetComponent<DefendZone>().destroyed)
						{
							delay = 0;
							mState = FSM_M.COMBAT;
							cState = FSM_C.DESTROY;	
						}
					}
				}

				if(Target == waypointList[nextWaypt])				// if target is waypt
				{
					// if AI reaches targeted waypt
					if((Target.position-transform.position).sqrMagnitude < enemyStats.tolLength*1.2f)
					{
						++nextWaypt;								// move to next waypt
						if(nextWaypt > waypointList.Count-1)		
						{
							nextWaypt = 0;
						}
						if(nextWaypt == 1)							// rest once reached start waypt 
							mState = FSM_M.IDLE;

						Target = waypointList[nextWaypt];	
						targetCurrentPos = Target.position;
						targetIndex = 0;
						PathRequestManager.RequestPath(transform.position, Target.position, OnPathFound);
					}
				}
				break;

			case FSM_M.CHASE:			 
				if(this.UnitType != UType.UNIT_E_DESTROYER)
				{
					if(!this.theModel.CollisionRegion.inRng_Chase)		// Player out of sight		
					{
						Target = null;
						mState = FSM_M.IDLE;					 
					}
 					else if(this.theModel.CollisionRegion.inRng_Fire)	// Player in rng	
					{
						delay = 0;
						strafeTargetPos	= this.transform.position;
						mState = FSM_M.COMBAT;	
					}						
				}
				break;

			case FSM_M.COMBAT:											
				if(this.UnitType != UType.UNIT_E_DESTROYER)
				{
					if(!this.theModel.CollisionRegion.inRng_Chase)		// Player out of sight		
					{
						Target = null;
						mState = FSM_M.IDLE;					 
					}

					// strafe movement randomization - look for btr way to do this
					for(int i = 0; i < random_values.Length; ++i)
						random_values[i] = Random.value;
					if(random_values[0] < 0.05)	// 5% chance of strafing rightwards
						strafeTargetPos.x += 10;	
					if(random_values[1] < 0.05)	// 5% chance of strafing leftwards
						strafeTargetPos.x -= 10;	
					if(random_values[2] < 0.05)	// 5% chance of strafing upwards
						strafeTargetPos.y += 10;	
					if(random_values[3] < 0.05)	// 5% chance of strafing downwards
						strafeTargetPos.y -= 10;	
							
					// if strafe too far, reset strafe
					if((strafeTargetPos-Player.position).sqrMagnitude > enemyStats.strafeLength)
						strafeTargetPos	= this.transform.position;

					// strafe movement 
					transform.position = Vector3.MoveTowards(transform.position, strafeTargetPos,
					                                         enemyStats.alertMoveSpeed*Time.deltaTime);
				}

				CombatFSM();
				break;

			case FSM_M.FLEE:									
				Debug.Log("mFLEE");				
				break;

			case FSM_M.DEAD:
				cState = FSM_C.NULL;	
				break;
		}
	}
	
	void CombatFSM()
	{
		switch(cState)
		{
			case FSM_C.NULL:											 
				if(this.UnitType == UType.UNIT_E_DESTROYER)
					cState = FSM_C.DESTROY;
				else
					cState = FSM_C.SHOOT;
				break;

			case FSM_C.SHOOT:			
				if(this.theModel.CollisionRegion.inRng_Fire)	// if Player/objective still in rng	
				{
					delay -= Time.deltaTime;						
					
					if(delay <= 0)									// fire rate ctrl
					{
						delay = enemyStats.FIRE_RATE;
						firingScript.Fire(Player.position);
					}
				}
				else 												// Player/objective went out of rng	
				{
					cState = FSM_C.NULL;				 
					mState = FSM_M.CHASE;				 
				}
				break;

			case FSM_C.DESTROY:	 	
				DefendZone defendZone = theModel.CollisionRegion.other.GetComponent<DefendZone>();
					
				if(defendZone.destroyed)
				{			 
					mState = FSM_M.IDLE;	// rest a while
					cState = FSM_C.NULL;
					Target = null;
				}

				delay -= Time.deltaTime;
				if(delay <= 0)				// fire rate ctrl	
				{
					firingScript.Fire(defendZone.transform.position, "SP");
					delay = enemyStats.FIRE_RATE;
				}
				break;

			default:
				cState = FSM_C.NULL;	
				break;
		}
	}

	public void OnPathFound(Vector3[] newPath, bool pathSuccessful) 
	{
		if(pathSuccessful) {
			path = newPath;
			StopCoroutine("FollowPath");
			StartCoroutine("FollowPath");
		}
	}

	IEnumerator FollowPath() {
		followingPath = true;
		Vector3 currentWaypoint = Vector3.zero;
		if(path.Length != 0)
			currentWaypoint = path[0];

		while(true) {
			if(path.Length == 0)
				yield break;

			if(currentWaypoint == transform.position)
			//if((currentWaypoint-transform.position).sqrMagnitude < enemyStats.tolLength)
			{
				++targetIndex;

				if(targetIndex >= path.Length){
					// reset
					targetIndex = 0;
					path = new Vector3[0];
					yield break;
				}

				currentWaypoint = path[targetIndex];
			}

			float speed = enemyStats.moveSpeed*Time.deltaTime;
			if(mState == FSM_M.CHASE)
				speed *= enemyStats.alertMoveSpeed/enemyStats.moveSpeed;

			if(mState == FSM_M.ROAM || mState == FSM_M.CHASE)
				transform.position = Vector3.MoveTowards(transform.position, currentWaypoint, speed);

			yield return null;
		}
	}


	void HitBoxUpdate() {
		if(theModel.CollisionRegion.HitboxTrigger)
		{
			hp -= 1;	//temp, chg to player's dmg (if we adding dmg in)
			theModel.CollisionRegion.setHitboxTriggerFalse();
			
			if(hp <= 0)
			{
				hp = 0;
				mState = FSM_M.DEAD;
				++Global.EnemyKillCount;
				StopCoroutine("FollowPath");
				Player.GetComponent<PlayerInfo>().AddPts(10);
				 //Destroy(this.gameObject);
				GetComponentInParent<AIManager>().DestroyEnemy(this);
			}
		}
	}
	
#if UNITY_EDITOR
	// path visualization
	public void OnDrawGizmos() {
		if(path != null) {
			for(int i = targetIndex; i < path.Length; ++i) 
			{
				Gizmos.color = Color.black;
				Gizmos.DrawCube(path[i], Vector3.one*0.25f);

				if(i == targetIndex) 
					Gizmos.DrawLine(transform.position, path[i]);
				else
					Gizmos.DrawLine(path[i-1], path[i]);
			}
		}
	}
#endif
	// doesnt work cos AI got multiple colliders
//	void OnTriggerEnter(Collider col)
//	{
//		if(col.gameObject.tag == "bullet_player")
//		{
//			hp -= 1;	//temp, chg to player's dmg (if we adding dmg in)
//			Destroy(col.gameObject);
//		}
//	}
}