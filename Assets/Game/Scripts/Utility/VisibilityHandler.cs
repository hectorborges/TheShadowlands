using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisibilityHandler : MonoBehaviour
{
    MeshRenderer rend;
    float visibilityTimer;
    bool visible;
    bool triggeredVisibility;
    
	void Start ()
    {
        rend = GetComponent<MeshRenderer>();
        visible = true;
        rend.material.SetFloat("_SliceAmount", 0);
    }

	void Update ()
    {
        if(triggeredVisibility)
        {
            visibilityTimer = 0;
            triggeredVisibility = false;
        }

	    if(visible && rend.material.GetFloat("_SliceAmount") != 0)
        {
            rend.material.SetFloat("_SliceAmount", Mathf.Lerp(1, 0, visibilityTimer));

            visibilityTimer += 1f * Time.deltaTime;
        }
        else if(!visible && rend.material.GetFloat("_SliceAmount") != 1)
        {
            rend.material.SetFloat("_SliceAmount", Mathf.Lerp(0, 1, visibilityTimer));

            visibilityTimer += 1f * Time.deltaTime;
        }
	}

    public void IsVisible(bool visibility)
    {
        triggeredVisibility = true;
        visible = visibility;
    }
}
