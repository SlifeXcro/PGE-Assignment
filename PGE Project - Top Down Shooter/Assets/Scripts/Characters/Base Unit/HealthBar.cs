using UnityEngine;
using System.Collections;

public class HealthBar : MonoBehaviour {

	public float maxHP = 100f;		//in percentage
	public float hp;
	public GameObject HPbar;

	float barScale;					// scale of hp bar (0 - 1.0f, 1 being 100% hp)

	// Use this for initialization
	void Start () 
	{
		hp = maxHP;
	}
	
	// Update is called once per frame
	void Update () 
	{
		barScale = hp/maxHP;
		HPbar.transform.localScale = new Vector3(barScale, HPbar.transform.localScale.y, HPbar.transform.localScale.z);
	}
}