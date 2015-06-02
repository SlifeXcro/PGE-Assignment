using UnityEngine;
using System.Collections;

public class SelectionOfItem : MonoBehaviour {

    private bool DisplayText;
    public float Price;
    public float ATTdamage;
    public string Recoil;
    public int charCount = 1;
    public GUITexture Pistol;
    public GUITexture HeavyRifle;
    public GUITexture Rifle;
    public GUITexture MP5;
 

    void MouseEnter()
    {

        DisplayText = true;
    }

    void MouseExit()
    {
        DisplayText = false;
    }

    void Update()
    {
    }
        void OnGUI()
        {
            if (Pistol.HitTest(Input.mousePosition))
            {
                // Application.LoadLevel(1);
                GUI.contentColor = Color.blue;
                GUI.Label(new Rect(Input.mousePosition.x + 20, Screen.height - Input.mousePosition.y, 5000, 5000), "Price: " + (Price * charCount).ToString());
                GUI.Label(new Rect(Input.mousePosition.x + 20, Screen.height - Input.mousePosition.y + 10, 5000, 5000), "Total Damage: " + (ATTdamage * charCount).ToString());
             //   GUI.Label(new Rect(Input.mousePosition.x + 20, Screen.height - Input.mousePosition.y + 10, 1000, 1000), "Recoil: " + (Recoil * charCount).ToString());
            }

            if (MP5.HitTest(Input.mousePosition))
            {
                // Application.LoadLevel(1);
                GUI.contentColor = Color.blue;
                GUI.Label(new Rect(Input.mousePosition.x + 20, Screen.height - Input.mousePosition.y, 5000, 5000), "Price: " + (Price * charCount).ToString());
                GUI.Label(new Rect(Input.mousePosition.x + 20, Screen.height - Input.mousePosition.y + 10, 5000, 5000), "Total Damage: " + (ATTdamage * charCount).ToString());
            }

            if (HeavyRifle.HitTest(Input.mousePosition))
            {
                // Application.LoadLevel(1);
                GUI.contentColor = Color.blue;
                GUI.Label(new Rect(Input.mousePosition.x + 20, Screen.height - Input.mousePosition.y, 1000, 1000), "Price: " + (Price * charCount).ToString());
                GUI.Label(new Rect(Input.mousePosition.x + 20, Screen.height - Input.mousePosition.y + 10, 1000, 1000), "Total Damage: " + (ATTdamage * charCount).ToString());
            }
            if (Rifle.HitTest(Input.mousePosition))
            {
                // Application.LoadLevel(1);
                GUI.contentColor = Color.blue;
                GUI.Label(new Rect(Input.mousePosition.x + 20, Screen.height - Input.mousePosition.y, 1000, 1000), "Price: " + (Price * charCount).ToString());
                GUI.Label(new Rect(Input.mousePosition.x + 20, Screen.height - Input.mousePosition.y + 10, 1000, 1000), "Total Damage: " + (ATTdamage * charCount).ToString());
            }

            Rect rect = new Rect(0, 0, 300, 100);
            GUI.contentColor = Color.white;
            Vector3 offset = new Vector3(0, -Camera.main.transform.position.y / 1.5f, 0);

            var point = Camera.main.WorldToScreenPoint(this.transform.position + offset);
            rect.x = point.x;
            //rect.y = Screen.height - point.y - rect.height; // bottom left corner set to the 3D point
            rect.y = point.y;
            GUI.contentColor = Color.blue;
            Rect rect2 = rect;
            rect2.y = rect.y + 10;
            //GUI.Label(rect2, this.name); // display its name, or other string
            //GUI.Label(rect, "C " + charCount.ToString()); // display its
        }
    

}
