using UnityEngine;

public class GUIManager : MonoBehaviour {

	public RenderTexture MinimapTex;
	public Material MinimapMat;

	public float minX;
	public float maxX;
	public float minY;
	public float maxY;

	void Awake () {
	}

	void OnGUI () {
		if(Event.current.type == EventType.Repaint)
			Graphics.DrawTexture(new Rect(minX, 
			                              minY, 
			                              maxX, 
			                              maxY), 
			                     MinimapTex, 
			                     MinimapMat);
	}
}
