using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AIManager : MonoBehaviour {

	public Enemy EnemyPrefab;

	private List<Enemy> EnemiesList = new List<Enemy>();

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () 
    {
		if (EnemiesList.Count < 3)			// if enemies less thn 3, spawn more
		{
			Enemy newEnemy = Instantiate(EnemyPrefab, this.transform.position, Quaternion.identity) as Enemy;
			EnemiesList.Add(newEnemy);
		}
	}
}