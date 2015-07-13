using UnityEngine;
using System.Collections;

public class ColliderScript : MonoBehaviour 
{
    //Flag to Check if Unit has made contact with other collider
	public bool inRegion = false;
	public Transform other;

	void OnTriggerEnter(Collider col)
	{
		Transform colliderOwner = this.transform.parent.parent.parent;
		switch(tag)	// check this col's tag
		{
			case "SIGHT":
			case "RANGE":
				if(colliderOwner.GetComponent<Enemy>().UnitType == Unit.UType.UNIT_E_DESTROYER)
				{
					if(col.gameObject.tag == "defendZone")	// if protected area enters this col
					{
						inRegion = true;
						other = col.transform;
					}
				}
				else
				{
					if(col.gameObject.tag == "Player")	// if player enters this 2 col.,
						inRegion = true;
				}
				break;

			case "HITBOX":
				if(col.gameObject.tag == "bullet_player")		//player's bullet enters
				{
					inRegion = true;
					Destroy(col.gameObject);
				}
				break;

			case "UNWALKABLE":
				if(col.gameObject.tag == tag)
					inRegion = true;
				break;

			default:
				break;
		}
    }

	void OnTriggerExit(Collider col)
	{
		Transform colliderOwner = this.transform.parent.parent.parent;
		switch(tag)	// check this col's tag
		{
			case "SIGHT":
			case "RANGE":
				if(colliderOwner.GetComponent<Enemy>().UnitType == Unit.UType.UNIT_E_DESTROYER)
				{
					if(col.gameObject.tag == "defendZone")	// if protected area enters this col
					{
						inRegion = false;
						other = col.transform;
					}
				}
				else
				{
					if(col.gameObject.tag == "Player")	// if player enters this 2 col.,
						inRegion = false;
				}
				break;
				
			case "HITBOX":
				break;
				
			case "UNWALKABLE":
				if(col.gameObject.tag == tag)
					inRegion = false;
				break;
				
			default:
				break;
		}
    }
}