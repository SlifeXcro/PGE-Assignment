using UnityEngine;
using System.Collections;

public class CheckPoint : MonoBehaviour {

    public GameObject PlayerObject;
    public Vector3 position;
    public GameObject Checkpoint;
 

	// Use this for initialization
	void Start () {
      
        Vector3 savePosition = new Vector3(PlayerPrefs.GetFloat("playerx"), PlayerPrefs.GetFloat("playery"),0);
        PlayerObject.transform.position = savePosition;
	}
	 
  //void OnTriggerEnter(PlayerObject:Checkpoint)
  //     {
  //              PlayerPrefs.SetFloat("playerx", PlayerObject.transform.position.x);
  //              PlayerPrefs.SetFloat("playery", PlayerObject.transform.position.y);
  //      }
          

	// Update is called once per frame
	void Update () {

         PlayerPrefs.SetFloat("playerx", PlayerObject.transform.position.x);
         PlayerPrefs.SetFloat("playery", PlayerObject.transform.position.y);
       
        
	}
}
