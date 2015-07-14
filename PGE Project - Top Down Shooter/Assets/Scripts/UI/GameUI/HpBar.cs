using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class HpBar : MonoBehaviour 
{
    // >>> Start of Variables <<< //
    float InitialScale;
    public Unit theUnit;
    // >>> End of Variables <<< //

	// === Initialisation === //
	void Start () 
    {
        // -- Store Initial Scale
        InitialScale = this.transform.localScale.x;

        // -- Fire Coroutines
        StartCoroutine(ModifyScale());
	}
	
	// === Update Scale === //
	IEnumerator ModifyScale() 
    {
        while (true)
        {
            // -- Set Cur Scale
            float CurScaleX = this.transform.localScale.x;

            // -- Set New Scale
            CurScaleX = InitialScale * ((float)theUnit.Stats.HP / (float)theUnit.Stats.MAX_HP);

            // -- Modify Scale
            this.transform.localScale = new Vector3(CurScaleX, this.transform.localScale.y, this.transform.localScale.z);

            yield return new WaitForSeconds(.2f);
        }
	}
}
