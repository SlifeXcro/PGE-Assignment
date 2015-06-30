using UnityEngine;
using System.Collections;

public class TurretBehaviour : MonoBehaviour {

	public GameObject BulletPrefab;

	public float FireRate = 1.0f;
	private float LastFired = 0.0f;
	private Firing fire;
	
	// Use this for initialization
	void Start () {
		fire = this.GetComponent<Firing>();
	}
	
	// Update is called once per frame
	void Update () {
		transform.Rotate(0.0f, 0.0f, -0.5f);
		
		float current_time = Time.time;
		if ((current_time - LastFired) > FireRate){
//			GameObject bullet = (GameObject)Instantiate(BulletPrefab, transform.position, Quaternion.identity);
//			bullet.GetComponent<Rigidbody2D>().AddForce(transform.up * 1000.0f);
			fire.Fire(transform.up*1000);

			LastFired = current_time;
		}
	}
}
