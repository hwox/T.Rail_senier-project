using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeSky : MonoBehaviour
{

    // Use this for initialization
    Color Day = new Vector4(1f, 1f, 1f, 1f);
    Color Twilight = new Vector4(1f, 0.7496176f, 0.4386f, 1f);
    Color Night = new Vector4(0.1037736f, 0.02006942f, 0.0935378f, 1f);
    Color Color_A, Color_B;

    Color DayLight = new Vector4(1f, 1f, 1f, 1f);
    Color TwilightLight = new Vector4(0.9f, 0.4303677f, 0, 1f);
    Color NightLight = new Vector4(0, 0, 0, 1f);

    Color Light_A, Light_B;

    public Light li;
    public int light_SpinAngle = 1;
    public float DaySpeed = 0.1f;

    public static ChangeSky instance = null;
    public int SkychSign { get; set; }

    private void Awake()
    {
        instance = this;

    }


    void Start()
    {
        StartCoroutine("ChangeSkycolor");
        SkychSign = 0;
    }





    IEnumerator ChangeSkycolor()
    {
        if (SkychSign == 0)
        {
            Color_A = Day; Color_B = Twilight;
            Light_A = DayLight; Light_B = TwilightLight;
        }
        else if (SkychSign == 1)
        {
            Color_A = Twilight; Color_B = Night;
            Light_A = TwilightLight; Light_B = NightLight;
        }
        else if (SkychSign == 2)
        {
            Color_A = Night; Color_B = Day;
            Light_A = NightLight; Light_B = DayLight;
        }


        for (float i = 0f; i <= 1; i += 0.01f * DaySpeed)
        {

            GetComponent<MeshRenderer>().material.color = Color.Lerp(Color_A, Color_B, i);
            li.color = Color.Lerp(Light_A, Light_B, i);
            yield return 0;
        }
        SkychSign += 1;
        if (SkychSign == 2)
        {
            li.transform.rotation = Quaternion.Euler(35, -75, -70); // 아침에 해뜰때 각도;
        }
        if (SkychSign > 2)
            SkychSign = 0;


        yield return new WaitForSeconds(0.5f);

        StartCoroutine("ChangeSkycolor");
    }


    // Update is called once per frame
    void Update()
    {



    }
}
