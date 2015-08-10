using UnityEngine;
using System.Collections;

public class DefendZone : MonoBehaviour {

	public bool destroyed = false;
	public float maxHP = 5;
	public float hp;
	public GameObject explosionPrefab;
	//PlayerInfo Player;

	void Start () {
		hp = maxHP;

		//Player = PlayerInfo.Instance;
	}

	void Update () {
		if(!destroyed && hp <= 0)
		{
			GetComponent<SpriteRenderer>().color = new Color(0.1f, 0.1f, 0.1f);
			// play explosion
			GameObject explosion = null;
			for(int i = 0; i < 3; ++i)
			{
				explosion = (GameObject)Instantiate(explosionPrefab, transform.position+transform.up*(i-1)*2-transform.right*(i-1)*2, Quaternion.identity);
				explosion.transform.parent = this.transform;
			}
			destroyed = true;
			hp = 0;

			PlayerInfo.Instance.Stats.HP -= PlayerInfo.Instance.Stats.MAX_HP*0.25f;	// player's health dec by 1/4
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
