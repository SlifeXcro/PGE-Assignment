﻿using UnityEngine;
using System.Collections;

public class PlayerInfo : Unit
{
    // *** Inherited Virtual Functions *** //
    public override void RandomizeStats()
    {
        Debug.Log("Player Stats Inited.");
        Stats.Set(1, Random.Range(500, 700),
                     Random.Range(190, 270), Random.Range(150, 220),
                     Random.Range(190, 270), Random.Range(150, 220),
                     Random.Range(1.1f, 1.75f), "Player", "Slife");
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

	public GUIText Points_Count;
	public float pt = 0;

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
		
		Points_Count.text = "" + pt;
    }

	public void AddPts(float pts){
		pt += pts;
		
	}
	
	public void MinusPts(float pts){
		
		pt -= pts;
	}
}
