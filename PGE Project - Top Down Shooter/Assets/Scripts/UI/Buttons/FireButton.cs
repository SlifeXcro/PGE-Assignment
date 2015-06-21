using UnityEngine;
using System.Collections;

public class FireButton : Button 
{
    // *** Inherited Virtual Functions *** //
    public override void ExecuteFunction()
    {
        
    }

	//Use this for initialization
	void Start () 
    {
	    //Set Button Type
        this.ButtonType = BType.BUTTON_FIRE;

        this.OnClick = false;
	}
	
	//Update is called once per frame
	void Update () 
    {
	    //Update from Parent
        this.StaticUpdate();
	}
}
