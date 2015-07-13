using UnityEngine;
using System.Collections;

public class DoorAnim : MonoBehaviour {
	public bool open = false;
	
	Animator[] anim;

	void Start () {
		anim = GetComponentsInChildren<Animator>();
	}

	void Update () {
		for(int i = 0; i < anim.Length; ++i)
			anim[i].SetBool("open", open);
	}
}
