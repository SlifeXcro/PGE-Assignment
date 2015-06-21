using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ColliderManager : MonoBehaviour
{
    //Flag to Check if Unit (Enemy) has made contact with Player/objective
    public bool inRng_Chase = false;
    //Flag to Check if Unit (Enemy) has made contact and is now in rng with Player/objective
    public bool inRng_Fire = false;

    //Flag to Check if Unit has made contact with Unwalkable Objects
    public bool CollidedUnwalkable = false;

    //List of Colliders
    public List<CollisionRegion> CollidersList = new List<CollisionRegion>();

    void Update()
    {
        //Loop through CollidersList
        foreach (CollisionRegion col in CollidersList)
        {
            switch (col.Type)
            {
                case CollisionRegion.RegionType.REGION_UNWALKABLE:
                    CollidedUnwalkable = col.inRegion;
                    break;
                case CollisionRegion.RegionType.REGION_RANGE:
                    inRng_Fire = col.inRegion;
                    break;
                case CollisionRegion.RegionType.REGION_SIGHT:
                    inRng_Chase = col.inRegion;
                    break;
            }
        }
    }
}