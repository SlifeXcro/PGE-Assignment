using UnityEngine;
using System.Collections;

public class DefendZone : MonoBehaviour {

	public bool destroyed = false;
	public float maxHP = 5;
	public float hp;


	void Start () {
		hp = maxHP;
	}

	void Update () {
		if(!destroyed && hp <= 0)
		{
			destroyed = true;
			hp = 0;
		}
	}

	void OnTriggerEnter(Collider col)
	{
		if(!destroyed)
		{
			if(col.gameObject.tag == "bullet_enemySP")
			{
				--hp;
				Destroy(col.gameObject);
			}
		}
	}
}
