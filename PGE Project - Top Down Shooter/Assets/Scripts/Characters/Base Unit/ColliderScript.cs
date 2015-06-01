using UnityEngine;
using System.Collections;

public class ColliderScript : MonoBehaviour 
{
    //Flag to Check if Unit has made contact with other collider
	public bool inRegion = false;

	void OnTriggerEnter2D(Collider2D col)
    {
		switch(tag)	// check this col's tag
		{
			case "SIGHT":
			case "RANGE":
				if(col.gameObject.tag == "Player")	// if player enters this 2 col.,
					inRegion = true;
				break;

			case "HITBOX":
				if(this.transform.root.gameObject.tag == "Player")	//if hitbox belongs to player
				{
					if(col.gameObject.tag == "bullet_enemy")		//and enemy's bullet enters
					{
						//inRegion = true;
						Destroy(col.gameObject);
					}
				}
				else	//if hitbox doesnt belong to player
				{
					if(col.gameObject.tag == "bullet_player")		//and enemy's bullet enters
					{
						//inRegion = true;
						Destroy(col.gameObject);
					}
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

	void OnTriggerExit2D(Collider2D col)
    {
		switch(tag)	// check this col's tag
		{
		case "SIGHT":
		case "RANGE":
			if(col.gameObject.tag == "Player")	// if player enters this 2 col.,
				inRegion = false;
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