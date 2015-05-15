using UnityEngine;
using System.Collections;

public class UserTouch : MonoBehaviour 
{
    //Singleton Structure
    protected static UserTouch mInstance;
    public static UserTouch Instance
    {
        get
        {
            if (mInstance == null)
            {
                GameObject tempObj = new GameObject();
                mInstance = tempObj.AddComponent<UserTouch>();
                Destroy(tempObj);
            }
            return mInstance;
        }
    }

    public bool Touch_Unwalkable = false;
    public bool Touch_Unit = false;
    public bool TouchCollidedBorder;
    public short CurFingerIndex = 0;

    public Sprite[] ListOfSprites;

    //Use this for initialization
    void Start()
    {
        mInstance = this;
    }

    void OnTriggerStay(Collider col)
    {
        //Detects if User has touched on an Unwalkable Region
        if (col.gameObject.tag == "UNWALKABLE")
            Touch_Unwalkable = true;

        //Detects if User has touched on a Unit
        if (col.gameObject.tag == "UNIT")
            Touch_Unit = true;
    }
    void OnTriggerExit(Collider col)
    {
        //Detects if User has left his touch on an Unwalkable Region
        if (col.gameObject.tag == "UNWALKABLE")
            Touch_Unwalkable = false;

        //Detects if User has left his touchs on a Unit
        if (col.gameObject.tag == "UNIT")
            Touch_Unit = false;
    }
	
	//Update is called once per frame
	void Update () 
    {
        Vector3 TouchPos = Vector3.zero;
     
        //Change Touch Position on GUI to World Coordinates
#if UNITY_EDITOR || UNITY_STANDALONE
        TouchPos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0));
        this.transform.position = new Vector3(TouchPos.x, TouchPos.y, 0);
#elif UNITY_ANDROID
        bool isTouching = false;
        for (short i = 0; i < Input.touches.Length; ++i)
        {
            isTouching = true;

            if (Input.touches.Length > 1)
            {
                if (i > 0)
                {
                    //More than 1 touch, Analog Moving
                    //Set Touch pos to 1st Touch Pos (1st Touch = Move, 2nd Touch = Fire)
                    if (Analog.Instance.Move && CurFingerIndex == 0)
                    {
                        TouchPos = Camera.main.ScreenToWorldPoint(new Vector3(Input.touches[0].position.x, Input.touches[0].position.y, 0));
                        CurFingerIndex = 0;
                    }

                    //More than 1 touch, Analog not moving
                    //Indicates currently firing, set Touch Pos to cur touch (1st Touch = Fire, 2nd Touch = Move)
                    else
                    {
                        TouchPos = Camera.main.ScreenToWorldPoint(new Vector3(Input.touches[i].position.x, Input.touches[i].position.y, 0));
                        CurFingerIndex = i;
                    }
                }
            }
            else 
            {
                TouchPos = Camera.main.ScreenToWorldPoint(new Vector3(Input.touches[i].position.x, Input.touches[i].position.y, 0));
                CurFingerIndex = i;
            }
        }
        if (!isTouching || Analog.Instance.Move)
            this.transform.position = new Vector3(TouchPos.x, TouchPos.y, -11);
        else
            this.transform.position = new Vector3(TouchPos.x, TouchPos.y, 0);
#endif

        //Change Sprites
        if (Touch_Unit)
            this.GetComponent<SpriteRenderer>().sprite = ListOfSprites[1];
        else
            this.GetComponent<SpriteRenderer>().sprite = ListOfSprites[0];
	}
}
