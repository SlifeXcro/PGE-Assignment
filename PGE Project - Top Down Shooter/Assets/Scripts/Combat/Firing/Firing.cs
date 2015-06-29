using UnityEngine;
using System.Collections;

public class Firing : MonoBehaviour 
{
	public static bool FIRE_BUTTON = false,
	isFiring = false;
	public Button FireButton;

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

	// *** Variables *** //
    public Bullet BulletPrefab;
    public Transform Parent;
    public Vector3 BulletDir;
	public string bulletTag_player = "bullet_player";
	public string bulletTag_enemy = "bullet_enemy";
	
	//Timer
	Timer.TimeBundle FireTimer;

	//Use this for initialization
	void Start () 
    {
		mInstance = this;
		
		FireTimer.Time = 0.1f;
		FireTimer.TimeIndex = Timer.GetExecuteID(FireTimer.Time);
		
		if (FireButton == null)
			FIRE_BUTTON = false;
	}
	
	//Update is called once per frame
	void Update ()
	{
		if (tag != "Player")
			return;

		if (!InputScript.TouchDown)
			isFiring = false;
		
		bool Proceed = false;
		float VelMultiplier = 1.0f;
		
		//ON-SCREEN FIRE
		if (!FIRE_BUTTON)
		{
			//Remove Fire Button
			if (FireButton != null)
			{
				if (FireButton.GetComponent<SpriteRenderer>() != null && FireButton.GetComponent<SpriteRenderer>().enabled)
					FireButton.GetComponent<SpriteRenderer>().enabled = false;
			}
			
			Proceed = true;
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
		}
		
		//FIRE BUTTON
		else
		{
			//Enable Fire Button
			if (FireButton != null)
			{
				if (FireButton.GetComponent<SpriteRenderer>() != null && !FireButton.GetComponent<SpriteRenderer>().enabled)
					FireButton.GetComponent<SpriteRenderer>().enabled = true;
			}
			
			if (InputScript.InputCollided(FireButton.GetComponent<Collider>(), false))
				Proceed = true;
			
			//Toggle Direction
			#if UNITY_EDITOR || UNITY_STANDALONE
			switch (Movement.Instance.theUnit.theModel.E_Direction)
			{
			case Model.E_Dir.UP:
				BulletDir = Vector3.up;
				break;
			case Model.E_Dir.DOWN:
				BulletDir = Vector3.down;
				break;
			case Model.E_Dir.LEFT:
				BulletDir = Vector3.left;
				break;
			case Model.E_Dir.RIGHT:
				BulletDir = Vector3.right;
				break;
			default:
				break;
			}
			#elif UNITY_ANDROID
			BulletDir = Analog.Instance.GetTravelDir();
			#endif
		}
		
		//Disable on Move for Editor and Exe Modes
		bool ProceedToFire = true;
		#if UNITY_EDITOR || UNITY_STANDALONE
		if (Analog.Instance.Move)
			ProceedToFire = false;
		#endif
		
		//FIRE
		if (Timer.ExecuteTime(FireTimer.Time, FireTimer.TimeIndex) && InputScript.TouchDown && ProceedToFire)
		{
			if (Proceed)
			{
				AudioManager.Instance.Play(AudioFile.A_TYPE.AUDIO_FIRE);
				isFiring = true;
				Bullet NewBullet = Instantiate(BulletPrefab, this.transform.position, Quaternion.identity) as Bullet;
				NewBullet.tag = bulletTag_player;
				NewBullet.transform.parent = Parent;
				NewBullet.Vel.Set(NewBullet.Vel.x * VelMultiplier, NewBullet.Vel.y * VelMultiplier, 0.0f);
				NewBullet.Dir = BulletDir;
			}
			else
				isFiring = false;
		}
	}

	// AI fire
	public void Fire(Vector3 target = new Vector3())
	{
		Bullet NewBullet = Instantiate (BulletPrefab, this.transform.position, Quaternion.identity) as Bullet;
		//NewBullet.transform.parent = Parent;
		NewBullet.transform.parent = Parent;
		NewBullet.tag = bulletTag_enemy;
		BulletDir = (target - this.transform.position).normalized;

		NewBullet.GetComponent<SpriteRenderer>().color = Color.black;	// temp (use other texture)
		NewBullet.Dir = BulletDir;
	}
}
