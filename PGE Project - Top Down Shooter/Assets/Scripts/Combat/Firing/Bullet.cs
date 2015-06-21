using UnityEngine;
using System.Collections;

public class Bullet : MonoBehaviour 
{
    public float Damage = 10.0f;
    public Vector3 Dir = Vector3.zero,
                   Vel = new Vector3(10, 10, 0);

	//Use this for initialization
	void Start () 
    {
	
	}
	
	//Update is called once per frame
	void Update () 
    {
        this.transform.position = new Vector3(this.transform.position.x + Dir.x * Vel.x * Time.deltaTime,
                                              this.transform.position.y + Dir.y * Vel.y * Time.deltaTime, 0);

        if (!Camera.main.GetComponent<Collider>().bounds.Contains(this.transform.position))
            Destroy(this.gameObject);
	}
}
