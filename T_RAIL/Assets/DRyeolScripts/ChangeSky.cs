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
        }
        else if (SkychSign == 1)
        {
            Color_A = Twilight; Color_B = Night;
        }
        else if (SkychSign == 2)
        {
            Color_A = Night; Color_B = Day;
        }


        for (float i = 0f; i <= 1; i += 0.01f)
        {

            GetComponent<MeshRenderer>().material.color = Color.Lerp(Color_A, Color_B, i);
            yield return 0;
        }
        SkychSign += 1;
        if (SkychSign > 2)
            SkychSign = 0;
        StartCoroutine("ChangeSkycolor");
    }


    // Update is called once per frame
    void Update()
    {



    }
}
