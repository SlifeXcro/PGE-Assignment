using UnityEngine;
using System.Collections;

public class HealthBar : MonoBehaviour {

	public bool isEnemy = true;

	float barScale;					// scale of hp bar (0 - 1.0f, 1 being 100% hp)

	Enemy enemy;
	EnemyStats enemyStats;
	DefendZone defendZone;

	void Start () 
	{
		if(isEnemy)
		{
			enemy = GetComponentInParent<Enemy>();
			enemyStats = transform.parent.GetComponentInChildren<EnemyStats>();
		}
		else
		{
			defendZone = GetComponentInParent<DefendZone>();
		}
	}

	void Update () 
	{
		if(isEnemy)
			barScale = enemy.hp/enemyStats.maxHP;
		else
			barScale = defendZone.hp/defendZone.maxHP;

		transform.GetChild(1).localScale = new Vector3(barScale, transform.GetChild(1).localScale.y, 
		                                               transform.GetChild(1).localScale.z);
	}
}