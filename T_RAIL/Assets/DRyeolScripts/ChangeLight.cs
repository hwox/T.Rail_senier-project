using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeLight : MonoBehaviour {

    Color DayLight = new Vector4(1f, 1f, 1f, 1f);
    Color TwilightLight = new Vector4(0.9f, 0.4303677f, 0, 1f);
    Color NightLight = new Vector4(0, 0, 0, 1f);

    Color Light_A, Light_B;
    int SkychSign;
    float time_count = 0;
    float light_SpinAngle = 0;

    public Light li;
    
    // Use this for initialization
    void Start () {
        StartCoroutine("Changelightcolor");
        SkychSign = 0;
        li = GetComponent<Light>();
    }

    IEnumerator Changelightcolor()
    {
        while (true)
        {
            double i = 0.1 * time_count;
         
           
             li.color = Color.Lerp(Light_A, Light_B, (float)i);
            time_count += 0.1f;
            if (time_count >= 16)
            {
                time_count = 0;
                SkychSign += 1;
                if (SkychSign > 2)
                    SkychSign = 0;
            }
            li.transform.Rotate(0, 0.75f, 0, Space.World);
            yield return new WaitForSeconds(0.5f);
        }

    }

    // Update is called once per frame
    void Update () {
        if (SkychSign == 0)
        {
          
            Light_A = DayLight; Light_B = TwilightLight;
        }
        else if (SkychSign == 1)
        {
            
            Light_A = TwilightLight; Light_B = NightLight;
        }
        else if (SkychSign == 2)
        {
          
            Light_A = NightLight; Light_B = DayLight;
        }
    }
}
