using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//Global Class
public class Global : MonoBehaviour
{
	// *** Global Variables *** //
    public static bool GamePause = false;
    public static bool StopMovement = false; //Stops Player Movement
    public static Item.ItemType CurrentItemType = Item.ItemType.ITEM_DEFAULT; //Only 1 instance of Item's type
    public static int CurrentItemID = -1; //Only 1 instance of Item's ID
    public static bool FreeCam = false; //Detect if Camera is "Free"
    public static int EnemyKillCount = 0;
    public static bool b_Win = false;
    public static bool b_StartChecking = false;
    public static List<Enemy> ListOfEnemies = new List<Enemy>();

    //Start Function
    void Start()
    {
        //Set Screen Orientation
        Screen.orientation = ScreenOrientation.LandscapeLeft;

        b_Win = b_StartChecking = false;
        ListOfEnemies.Clear();
    }

    //Update Function
    void Update()
    {
        //Stop Char Movement during Free Cam
        if (FreeCam)
            StopMovement = true;

        Debug.Log(ListOfEnemies.Count);

        //Detect Win
        if (ListOfEnemies.Count == 0 && b_StartChecking)
            b_Win = true;

        for (short i = 0; i < ListOfEnemies.Count; ++i )
        {
            if (ListOfEnemies[i] == null)
            {
                Enemy Temp = new Enemy();
                ListOfEnemies[i] = Temp;
                ListOfEnemies.RemoveAt(i);
            }
        }
    }
}
