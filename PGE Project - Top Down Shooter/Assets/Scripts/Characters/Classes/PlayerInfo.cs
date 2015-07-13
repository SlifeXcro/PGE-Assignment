using UnityEngine;
using System.Collections;

public class PlayerInfo : Unit
{
    // *** Inherited Virtual Functions *** //
    public override void RandomizeStats()
    {
        Debug.Log("Player Stats Inited.");
//        Stats.Set(1, Random.Range(500, 700),
//                     Random.Range(190, 270), Random.Range(150, 220),
//                     Random.Range(190, 270), Random.Range(150, 220),
//                     Random.Range(1.1f, 1.75f), "Player", "Slife");
    }

    //Singleton Structure
    protected static PlayerInfo mInstance;
    public static PlayerInfo Instance
    {
        get
        {
            if (mInstance == null)
            {
                GameObject tempObj = new GameObject();
                mInstance = tempObj.AddComponent<PlayerInfo>();
                Destroy(tempObj);
            }
            return mInstance;
        }
    }

    //Use this for initialization
    void Start()
    {
        //Set Instance
        if (mInstance != null)
        {
            Destroy(this.gameObject);
            return;
        }
        mInstance = this;

        //Class has been inherited
        Inherited = true;

        //Init from Parent Class
        this.Init();

        //Set Tag
        this.tag = "Player";

        //Init Stats
        this.RandomizeStats();
    }

    //Update is called once per frame
    void Update()
    {
        //Update from Parent Class
        this.StaticUpdate();

    }

	void OnTriggerEnter(Collider col)
	{
		if(col.gameObject.tag == "bullet_enemy")
		{
			//hp -= 1;	//temp, chg to AI's dmg (if we adding dmg in)
			Destroy(col.gameObject);
		}
	}
}
