using UnityEngine;
using System.Collections;

public class ColliderScript : MonoBehaviour 
{
    //Flag to Check if Unit has made contact with other collider
	public bool inRegion = false;

	void OnTriggerEnter2D(Collider2D col)
    {
		if (col.gameObject.tag == "UNWALKABLE" || col.gameObject.tag == "Player")
			inRegion = true;
    }

	void OnTriggerExit2D(Collider2D col)
    {
		if (col.gameObject.tag == "UNWALKABLE" || col.gameObject.tag == "Player")
			inRegion = false;
    }
}