using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CollisionRegionFlags : MonoBehaviour 
{
	//Flag to Check if Unit (Enemy) has made contact with Player/objective
	public bool inRng_Chase = false;
	//Flag to Check if Unit (Enemy) has made contact and is now in rng with Player/objective
	public bool inRng_Fire = false;

    //Flag to Check if Unit has made contact with Unwalkable Objects
	public bool CollidedUnwalkable = false;

	public List<ColliderScript> CollidersList = new List<ColliderScript>();

	void Start()
	{
	}

	void Update()
	{
		foreach(ColliderScript col in CollidersList)
		{
			if(col.gameObject.tag == "UNWALKABLE")
			{
				CollidedUnwalkable = col.inRegion;
			}
			else if(col.gameObject.tag == "SIGHT")
			{
				inRng_Chase = col.inRegion;
			}
			else if(col.gameObject.tag == "RANGE")
			{
				inRng_Fire = col.inRegion;
			}
		}
	}
}