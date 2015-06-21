using UnityEngine;
using System.Collections;

public class CheckBox : MonoBehaviour 
{
    public SpriteRenderer Tick;

	//Update is called once per frame
	void Update () 
    {
        //Check and Uncheck Box
        if (Tick != null)
            Tick.enabled = Firing.FIRE_BUTTON;

        //Toggle Fire Button
        if (InputScript.InputCollided(this.GetComponent<Collider>(), true))
            Firing.FIRE_BUTTON = !Firing.FIRE_BUTTON;
	}
}
