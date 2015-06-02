using UnityEngine;
using System.Collections;

public class Obstacle : MonoBehaviour {
	
	public Vector3 position;
	public GameObject BrickPrefab;
	
	// Use this for initialization
	
	void Awake()
	{
		//Vector3 position = new Vector3(Random.Range(100.0F, 1000.0F), 70, Random.Range(100.0F, 1000.0F));
	}
	
	void Start() 
	{
		int spawned = 0;
		//Brick = new GameObject ();
		
		while (spawned < 100)
		{
			
			position = new Vector3(Random.Range(-50.0F, 50.0F), Random.Range(-50.0F, 50.0F), 0);
			BrickPrefab = Instantiate(BrickPrefab, position, Quaternion.identity) as GameObject;
			spawned++;
			BrickPrefab.transform.parent = this.transform;
		}
		
		
	}
	
	
	// Update is called once per frame
	void Update () {
		
	}
}
