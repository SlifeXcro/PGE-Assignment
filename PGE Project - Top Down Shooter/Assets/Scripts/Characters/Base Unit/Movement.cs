using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

// *** MOVEMENT SCIPTING *** //
// ***   AUTHOR: SLIFE   *** //

// --- Uses Raycast to debug line of Movement
// --- Analog Movement Implemented

public class Movement : MonoBehaviour
{
    //Singleton Structure
    protected static Movement mInstance;
    public static Movement Instance
    {
        get
        {
            if (mInstance == null)
            {
                GameObject tempObj = new GameObject();
                mInstance = tempObj.AddComponent<Movement>();
                Destroy(tempObj);
            }
            return mInstance;
        }
    }

    List<KeyCode> ListOfMovementKeys = new List<KeyCode>();

    // *** Variables *** //
    bool isMoving = false, isWASD = false;
    bool facingLeft = false, flipped = false;
    public float MovementSpeed = 2.0f; 
    public Unit theUnit;
    public Map theMap;
    KeyCode CurrentKey = KeyCode.V;  
    Vector3 PlayerLastPos;
	//bool isPaused = false; 	//pause
	public ShopMenu ShpM;		//ShopMenu

    void OnTriggerEnter(Collider col)
    {

    }
    void OnTriggerExit(Collider col)
    {

    }

    //Use this for initialization
    void Start()
    {
        //Init Instance
        mInstance = this;

        //Set Movement Keys
        ListOfMovementKeys.Add(KeyCode.LeftArrow);
        ListOfMovementKeys.Add(KeyCode.RightArrow);
        ListOfMovementKeys.Add(KeyCode.UpArrow);
        ListOfMovementKeys.Add(KeyCode.DownArrow);
        ListOfMovementKeys.Add(KeyCode.W);
        ListOfMovementKeys.Add(KeyCode.A);
        ListOfMovementKeys.Add(KeyCode.S);
        ListOfMovementKeys.Add(KeyCode.D);

        //Set Last Pos
        PlayerLastPos = theUnit.transform.position;

		//Shop Close
		//ShpM.ShopShow ();

#if UNITY_EDITOR || UNITY_STANDALONE
#elif UNITY_ANDROID
        MovementSpeed *= 0.8f;
#endif
    }

    public static bool RayCastMovement(Vector3 Pos, Vector3 Dir, float Dist)
    {
        // *** RAYCASTING OF MOVEMENT *** //
        //Check if Path is Clear
        bool ClearPath = true;

        //Raycast Line of Movement
        RaycastHit[] Hit = Physics.RaycastAll(Pos, Dir, Dist);

        //Check if Raycast line has hit unwalkable objects
        if (Hit != null)
        {
            foreach (RaycastHit hit in Hit)
            {
                //Stop Movement
                if (hit.collider.tag == "UNWALKABLE")
                    ClearPath = false;
            }
        }

        //Debug Line
        if (ClearPath)
            Debug.DrawRay(Pos, Dir, Color.green);
        else
            Debug.DrawRay(Pos, Dir, Color.red);

        return !ClearPath;
        // *** END OF RAYCASTING *** //
    }

    //Movement 
    void Move(KeyCode Key)
    {
        isWASD = true;
        switch (Key)
        {
            case KeyCode.LeftArrow:
            case KeyCode.A:
                theUnit.theModel.SetAnimation(2);
                theUnit.transform.Translate(-MovementSpeed * Time.deltaTime, 0, 0);
                break;
            case KeyCode.RightArrow:
            case KeyCode.D:
                theUnit.theModel.SetAnimation(3);
                theUnit.transform.Translate(MovementSpeed * Time.deltaTime, 0, 0);
                break;
            case KeyCode.UpArrow:
            case KeyCode.W:
                theUnit.theModel.SetAnimation(0);
                //if (!(theUnit.theModel.CollisionRegion.CollidedUnwalkable && CurrentKey == Key))
                    theUnit.transform.Translate(0, MovementSpeed * Time.deltaTime, 0);
                //if (!theUnit.theModel.CollisionRegion.CollidedUnwalkable)
                    CurrentKey = Key;
                break;
            case KeyCode.DownArrow:
            case KeyCode.S:
                theUnit.theModel.SetAnimation(1);
                //if (!(theUnit.theModel.CollisionRegion.CollidedUnwalkable && CurrentKey == Key))
                    theUnit.transform.Translate(0, -MovementSpeed * Time.deltaTime, 0);
                //if (!theUnit.theModel.CollisionRegion.CollidedUnwalkable)
                    CurrentKey = Key;
                break;
            default:
                isMoving = false;
                break;
        }
    }

    //Update is called once per frame
    void Update()
    {
		//Set Camera Pan Unit
		if (CameraPan.Instance.theUnit != null && CameraPan.Instance.theUnit != this.theUnit)
			CameraPan.Instance.theUnit = this.theUnit;

		bool AllKeysClear = true;
#if UNITY_EDITOR || UNITY_STANDALONE
		//Check if Unit is Moving
		for (short i = 0; i < ListOfMovementKeys.Count; ++i) {
			if (Input.GetKey (ListOfMovementKeys [i])) {
				AllKeysClear = false;

				//Move Unit
				Move (ListOfMovementKeys [i]);
			} else if (Input.GetKeyUp (ListOfMovementKeys [i]))
				flipped = isWASD = false;
		}
		isMoving = !AllKeysClear;
#endif

		if (Input.GetKeyUp (KeyCode.P)) {
			ShpM.isPaused = true;
		}else if(Input.GetKeyUp (KeyCode.Q)){
			ShpM.isPaused = false;
		}
	
		//if (ShpM.isPaused == true) {
		if(ShpM.OnClicked == true || ShpM.isPaused == true){
			Time.timeScale = 0;
			ShpM.ShopShow();
		} else if (ShpM.OnClicked == false || ShpM.isPaused == false) {
			Time.timeScale = 1;
			ShpM.ShopOff();

		}

        if (Analog.Instance.Move)
        {
            //RayCast
            Vector3 theDir = new Vector3(Analog.Instance.GetTravelDir().x, Analog.Instance.GetTravelDir().y, 0);
            //bool ClearPath = !RayCastMovement(theUnit.theModel.CollisionRegion.transform.position, theDir, 1.0f);
            bool ClearPath = true;

            isMoving = true;
            theUnit.theModel.SetAnimation(1);

            //Only Move when path is clear
            if (ClearPath)
                theUnit.transform.Translate(Analog.Instance.GetTravelDir() * MovementSpeed * Time.deltaTime * 1.5f);
        }
        else if (AllKeysClear)
        {
            isMoving = false;
            theUnit.transform.Translate(Vector3.zero);
        }

        //Set Facing Direction
        bool FireButtonPriority = false;
#if UNITY_EDITOR || UNITY_STANDALONE
#elif UNITY_ANDROID
        if (Firing.FIRE_BUTTON)
            FireButtonPriority = true;
#endif
        Vector3 facingDir = Vector3.zero;
        if ((Analog.Instance.Move && !Firing.isFiring) || FireButtonPriority)
            facingDir = Analog.Instance.GetTravelDir();
        else if (!Firing.FIRE_BUTTON)
            facingDir = Firing.Instance.BulletDir;

        //Toggle Animation
        if (!(isWASD && !Firing.isFiring))
        {
            if (facingDir.x < 0 && facingDir.y > facingDir.x)
                theUnit.theModel.SetAnimation(2);
            else if (facingDir.x > 0 && facingDir.y < facingDir.x)
                theUnit.theModel.SetAnimation(3);
            else if (facingDir.y < 0)
                theUnit.theModel.SetAnimation(1);
            else if (facingDir.y > 0)
                theUnit.theModel.SetAnimation(0);
        }

        //Set Player Sprite & Animation to IDLE if Game is Paused
        //if (!Global.GamePause || Global.StopMovement)
        //    theUnit.theModel.SetAnimation(0);
        
        //Reset Movement Vector3 & Flags
        if (!isMoving || Global.StopMovement /*|| theUnit.CollidedUnwalkable*/)
        {
            //if (Global.FreeCam)
            //{
                flipped = false;
                PlayerLastPos = theUnit.transform.position;

            if (!InputScript.TouchDown)
                theUnit.theModel.GetComponent<Animator>().enabled = false;
            //}
        }

        if (isMoving || InputScript.TouchDown)
            theUnit.theModel.GetComponent<Animator>().enabled = true;
    }
}
