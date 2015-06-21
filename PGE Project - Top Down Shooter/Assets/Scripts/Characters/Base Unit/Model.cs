using UnityEngine;
using System.Collections;

public class Model : MonoBehaviour
{
    //Every Model has it's own Collision Regions
    public ColliderManager CollisionRegions;
    public bool isAnimated = true;
    public short CurAnimationIndex = 0;

    //Model Facing Dir
    public enum E_Dir
    {
        UP,
        DOWN,
        LEFT,
        RIGHT
    }; public E_Dir E_Direction;

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
        {
            this.GetComponent<Animator>().SetInteger("Direction", AnimationIndex);
            CurAnimationIndex = AnimationIndex;
            E_Direction = (E_Dir)AnimationIndex;
        }
    }

    //Use this for initialization
    void Start()
    {
        //Default to first Animation
        SetAnimation(0);
    }

    //Update is called once per frame
    void Update()
    {

    }
}
