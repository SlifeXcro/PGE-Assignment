using UnityEngine;
using System.Collections;

public class Model : MonoBehaviour 
{
	public bool isAnimated = true;
    //Every Model has it's own Walking Collision Region
    public CollisionRegionFlags WalkCollisionRegion;
	public Collision2D other;								// obj colliding with model

    //Returns own Animator
    public Animator GetAnimation()
    {
        if (isAnimated)
            return this.GetComponent<Animator>();
        return null;
    }

    //Set Animation
    public void SetAnimation(short AnimationIndex)
    {
        if (isAnimated)
            this.GetComponent<Animator>().SetInteger("Direction", AnimationIndex);
    }

	//Use this for initialization
	void Start () 
    {
        //Default to first Animation
        SetAnimation(0);
	}
	
	//Update is called once per frame
	void Update () 
    {
	
	}

	void OnCollisionEnter2D(Collision2D col)
	{
		Debug.Log("MODEL COLLIDE");
		other = col;				// rmb to set model's other to null after using this col info
	}
}
