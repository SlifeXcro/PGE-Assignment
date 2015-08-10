using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class AIManager : MonoBehaviour {

	enum SPAWN_AREA {
		TOP,
		BTM,
		LEFT,
		RIGHT
	}
	
	enum TARGET_AREA {		// for destroyers to target
		TOP_L,
		BTM_L,
		TOP_R,
		BTM_R
	}

	public Enemy EnemyShooterPrefab;
	public Enemy EnemyDestroyerPrefab;
	public List<Transform> spawnAreasList = new List<Transform>();

	int waveCount = 0;
	public int spawnEnemiesCount = 3;
	int destroyerEnemiesCount = 0;

	// find more efficient way to do these below
	List<Transform>[] waypointsGroup = new List<Transform>[4];
	List<Transform> topWaypointsList = new List<Transform>();
	List<Transform> btmWaypointsList = new List<Transform>();
	List<Transform> leftWaypointsList = new List<Transform>();
	List<Transform> rightWaypointsList = new List<Transform>();
	// areas for player to protect, enemy destroyers to attack
	//public List<Transform> protectAreasList = new List<Transform>();	

	List<DoorAnim> doorsList = new List<DoorAnim>();	
	List<Enemy> EnemiesList = new List<Enemy>();

	Timer.TimeBundle SpawnTimer;

	void Start () 
	{
		DoorAnim[] doors = FindObjectsOfType<DoorAnim>();
		doorsList = doors.OrderBy(go=>go.name).ToList();

		Transform waypointsTemp = GameObject.FindGameObjectWithTag("waypointsTop").transform;
		foreach(Transform child in waypointsTemp)
			topWaypointsList.Add(child);

		waypointsTemp = GameObject.FindGameObjectWithTag("waypointsBtm").transform;
		foreach(Transform child in waypointsTemp)
			btmWaypointsList.Add(child);
		
		waypointsTemp = GameObject.FindGameObjectWithTag("waypointsLeft").transform;
		foreach(Transform child in waypointsTemp)
			leftWaypointsList.Add(child);
		
		waypointsTemp = GameObject.FindGameObjectWithTag("waypointsRight").transform;
		foreach(Transform child in waypointsTemp)
			rightWaypointsList.Add(child);

		waypointsGroup[0] = topWaypointsList;
		waypointsGroup[1] = btmWaypointsList;
		waypointsGroup[2] = leftWaypointsList;
		waypointsGroup[3] = rightWaypointsList;

//		Transform protectAreas = GameObject.FindGameObjectWithTag("protectAreas").transform;
//		foreach(Transform child in protectAreas)
//			protectAreasList.Add(child);

		SpawnTimer.Time = 2.0f;
		SpawnTimer.TimeIndex = Timer.GetExecuteID(SpawnTimer.Time);
	}

	void Update () 
	{
		for(int i = 0; i < doorsList.Count; ++i)
		{
			if(doorsList[i].open && Timer.ExecuteTime(SpawnTimer.Time, SpawnTimer.TimeIndex))
			{
				doorsList[i].open = false;
			}
		}

		if(EnemiesList.Count == 0 || destroyerEnemiesCount == 0)
		{
			++waveCount;

			// spawn logic here
			int randValue = Random.Range(0, 3);
			if(waveCount == 1)
				randValue = 0;
			SpawnWave(1, Unit.UType.UNIT_E_DESTROYER, (SPAWN_AREA)randValue, 
			          spawnAreasList[randValue].position, (TARGET_AREA)randValue);
			++destroyerEnemiesCount;
			SpawnWave(spawnEnemiesCount, Unit.UType.UNIT_E_SHOOTER, (SPAWN_AREA)randValue, 
			          spawnAreasList[randValue].position);
		}
	}

	public void DestroyEnemy(Enemy deadEnemy)
	{
		if (deadEnemy.UnitType == Unit.UType.UNIT_E_DESTROYER)
			--destroyerEnemiesCount;
		EnemiesList.Remove(deadEnemy);
		Destroy(deadEnemy.gameObject);
	}

	void SpawnWave(int enemyCount, Unit.UType type, SPAWN_AREA spawnArea, 
	               Vector3 spawnPos, TARGET_AREA targetArea = TARGET_AREA.TOP_L)
	{
		doorsList[(int)spawnArea].open = true;

		if(enemyCount <= 0)
			return;

		for(int i = 0; i < enemyCount; ++i)
		{
			Enemy newEnemy = null; 

			if(type == Unit.UType.UNIT_E_DESTROYER)
			{
			   newEnemy = Instantiate( EnemyDestroyerPrefab, spawnPos, Quaternion.identity ) as Enemy;
			   //newEnemy.SetTarget(protectAreasList[(int)targetArea]);
			}
			else
			{
				newEnemy = Instantiate( EnemyShooterPrefab, spawnPos, Quaternion.identity ) as Enemy;

				// assign target
//				if(EnemiesList.Count > 0)
//				{
//					Transform target = null;
//					foreach(Enemy enemy in EnemiesList)
//					{
//						if(enemy.UnitType != Unit.UType.UNIT_E_DESTROYER)
//							continue;
//
//						if(target == null)
//							target = enemy.transform;
//						else
//						{
//							// if new target's dist < current target dist
//							if( Vector3.Distance(enemy.transform.position, newEnemy.transform.position) < 
//						   		Vector3.Distance(target.position, newEnemy.transform.position) )
//								target = enemy.transform;
//						}
//					}
//
//					if(target != null)
//						newEnemy.SetTarget(target);
//				}
			}

			Vector3 tempLocalScale = newEnemy.transform.localScale;
			newEnemy.transform.parent = this.transform;
			newEnemy.transform.localScale = tempLocalScale;
			newEnemy.waypointList = waypointsGroup[(int)spawnArea];
			newEnemy.GetComponent<Firing>().Parent = GameObject.FindGameObjectWithTag("BULLET").transform;
			EnemiesList.Add(newEnemy);
		}
	}
}