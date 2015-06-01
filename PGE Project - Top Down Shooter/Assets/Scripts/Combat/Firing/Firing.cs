using UnityEngine;
using System.Collections;

public class Firing : MonoBehaviour 
{
    //Singleton Structure
    protected static Firing mInstance;
    public static Firing Instance
    {
        get
        {
            if (mInstance == null)
            {
                GameObject tempObj = new GameObject();
                mInstance = tempObj.AddComponent<Firing>();
                Destroy(tempObj);
            }
            return mInstance;
        }
    }

    public Bullet BulletPrefab;
    public Transform Parent;
    public Vector3 BulletDir;
	public string bulletTag_player = "bullet_player";
	public string bulletTag_enemy = "bullet_enemy";

	//Use this for initialization
	void Start () 
    {
        mInstance = this;
	}
	
	//Update is called once per frame
	void Update ()
	{
		if(this.gameObject.tag == "Player" && InputScript.TouchDown)
		{
#if UNITY_EDITOR || UNITY_STANDALONE
        	BulletDir = (UserTouch.Instance.transform.position - this.transform.position).normalized;
#elif UNITY_ANDROID
	        if (InputScript.TouchDown && Input.touches.Length <= 1)
	            BulletDir = (UserTouch.Instance.transform.position - this.transform.position).normalized;
	        else if (Input.touches.Length > 1)
	        {
	            for (short i = 0; i < Input.touches.Length; ++i)
	            {
	                if (i != UserTouch.Instance.CurFingerIndex)
	                {
	                    BulletDir = (Camera.main.ScreenToWorldPoint(Input.touches[i].position) - this.transform.position).normalized;
	                    break;
	                }
	            }
	        }
#endif	
			if(Timer.ExecuteTime(0.15f, 1))
				Fire(true);
		}
	}
	
	public void Fire(bool fromPlayer, Vector3 target = new Vector3())
	{

		Bullet NewBullet = Instantiate (BulletPrefab, this.transform.position, Quaternion.identity) as Bullet;
		//NewBullet.transform.parent = Parent;

		if(fromPlayer)
		{
			NewBullet.tag = bulletTag_player;
		}
		else
		{
			NewBullet.tag = bulletTag_enemy;
			BulletDir = (target - this.transform.position).normalized;

			NewBullet.GetComponent<SpriteRenderer>().color = new Color(0, 0, 0);
		}
		NewBullet.Dir = BulletDir;
	}
}
