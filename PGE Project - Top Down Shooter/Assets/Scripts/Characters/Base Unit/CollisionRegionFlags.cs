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

	public bool HitboxTrigger = false;

	List<ColliderScript> CollidersList = new List<ColliderScript>();

	void Start()
	{
		foreach(Transform child in this.transform)	// iterator for child in this transform
		{
			CollidersList.Add(child.gameObject.GetComponent<ColliderScript>());
			//Debug.Log(child.tag);
		}
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
			else if(col.gameObject.tag == "HITBOX")
			{
				HitboxTrigger = col.inRegion;
			}
		}
	}

	public void setHitboxTriggerFalse()
	{
		HitboxTrigger = false;
		foreach(ColliderScript col in CollidersList)
		{
			if(col.gameObject.tag == "HITBOX")
				col.inRegion = HitboxTrigger;
		}
	}
}