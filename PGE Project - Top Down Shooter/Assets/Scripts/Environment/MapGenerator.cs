﻿using UnityEngine;
using System.Collections;

public class MapGenerator : MonoBehaviour 
{
	public float OffSet_X = 25.0f, OffSet_Y = 10.0f;
    public SpriteRenderer GroundTex; //Seamless Tile Tex
	public Transform Parent;
	public static int GROUND_HOR = 8, GROUND_VER = 4;

	//Use this for initialization
	void Awake () 
    {
	    //Generate Ground
        for (short i = 0; i < GROUND_HOR; ++i)
        {
            for (short j = 0; j < GROUND_VER; ++j)
            {
                GroundTex = Instantiate(GroundTex, new Vector3(GroundTex.GetComponent<Collider>().bounds.size.x * i - OffSet_X,
                                                               GroundTex.GetComponent<Collider>().bounds.size.y * j - OffSet_Y, 10),
                                                               Quaternion.identity) as SpriteRenderer;
                GroundTex.transform.parent = Parent;
            }
        }
	}

    //Update is called once per frame
    void Update() 
    {
	
	}
}
