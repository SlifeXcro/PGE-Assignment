using UnityEngine;

public class Tracker : MonoBehaviour {

	public float minX = 0.1f;
	public float maxX = 0.9f;
	public float minY = 0.1f;
	public float maxY = 0.9f;

	Transform goToTrack;
	SpriteRenderer TrackRenderer;
	public Sprite altsprite;
	bool swap = false;

	void Start() {
		goToTrack = transform.parent;
		TrackRenderer = GetComponent<SpriteRenderer>();
	}

	void Update () {
		Vector3 v3Screen = Camera.main.WorldToViewportPoint(goToTrack.position);

		if (v3Screen.x > -0.01f && v3Screen.x < 1.01f && v3Screen.y > -0.01f && v3Screen.y < 1.01f) {
			if(!swap){
				swap = true;
				Sprite tempSprite = TrackRenderer.sprite;
				TrackRenderer.sprite = altsprite;
				altsprite = tempSprite;
			}
		}
		else
		{
			if(swap){
				swap = false;
				Sprite tempSprite = TrackRenderer.sprite;
				TrackRenderer.sprite = altsprite;
				altsprite = tempSprite;
			}
			v3Screen.x = Mathf.Clamp (v3Screen.x, minX, maxX);
			v3Screen.y = Mathf.Clamp (v3Screen.y, minY, maxY);
			transform.position = Camera.main.ViewportToWorldPoint (v3Screen);
		}
		
	}
}
