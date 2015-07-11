using UnityEngine;
using System.Collections;

public class Bullet : MonoBehaviour 
{
    public float Damage = 10.0f;
    public Vector3 Dir = Vector3.zero,
                   Vel = new Vector3(10, 10, 0);
	public ParticleEmitter BulletExplosion;

	//Use this for initialization
	void Start () 
    {
	
	}

	public void OnTriggerEnter(Collider col) {
		Vector3 bullet_pos = transform.position;
		if (col.tag == "STATIC_OBJ" || (col.tag == "Player" && this.transform.parent.tag != "Player")) {
			Destroy (gameObject);
			ParticleEmitter explosion = (ParticleEmitter)Instantiate
				(BulletExplosion, bullet_pos, Quaternion.identity);
			explosion.Emit();
		}
	}
	
	//Update is called once per frame
	void Update () 
    {
        this.transform.position = new Vector3(this.transform.position.x + Dir.x * Vel.x * Time.deltaTime,
                                              this.transform.position.y + Dir.y * Vel.y * Time.deltaTime, 0);

        if (!Camera.main.GetComponent<RectTransform>().rect.Contains(this.transform.position))
            Destroy(this.gameObject);
	}
}
