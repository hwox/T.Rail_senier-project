using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeDirectinalLight : MonoBehaviour
{

    Color DayLight = new Vector4(1f, 1f, 1f, 1f);
    Color TwilightLight = new Vector4(0.9f, 0.4303677f, 0, 1f);
    Color NightLight = new Vector4(0, 0, 0, 1f);

    Color Light_A, Light_B;

    public Light li;
    public int light_SpinAngle = 1;
    public float DaySpeed = 0.1f;
    void Start()
    {
        StartCoroutine("ChangeLightColor");
        StartCoroutine("SpinLight");
        li = GetComponent<Light>();


    }
    int SkychSign = 0;
    IEnumerator ChangeLightColor()
    {
        if (SkychSign == 0)// ¾ÆÄ§->Àú³á
        {
            Light_A = DayLight; Light_B = TwilightLight;
        }
        else if (SkychSign == 1)// Àú³á->¹ã
        {
            Light_A = TwilightLight; Light_B = NightLight;
        }
        else if (SkychSign == 2)// ¹ã->¾ÆÄ§
        {
           
        }


        for (float i = 0f; i <= 1; i += 0.01f * DaySpeed)
        {

            li.color = Color.Lerp(Light_A, Light_B, i);
            yield return 0;
        }
        SkychSign += 1;

        if (SkychSign == 2)
        {
            li.transform.rotation = Quaternion.Euler(35, -75, -70); // ¾ÆÄ§¿¡ ÇØ¶ã¶§ °¢µµ;
        }
        else if (SkychSign > 2)
        {
            SkychSign = 0;
        }
        StartCoroutine("ChangeLightColor");

    }
    IEnumerator SpinLight()
    {
        while (true)
        {
            li.transform.Rotate(0, light_SpinAngle, 0, Space.World);
            yield return 0;
        }

    }



    // Update is called once per frame
    void Update()
    {

    }
}
