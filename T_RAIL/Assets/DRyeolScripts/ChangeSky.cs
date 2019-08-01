using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeSky : MonoBehaviour
{



    // Use this for initialization
    Color GlassLand_A = new Vector4(1f, 1f, 1f, 1f);
    Color GlassLand_B = new Vector4(1f, 0.6316378f, 0.5235849f, 1f);

    Color Desert_A = new Vector4(0.7924528f, 0.2291592f, 0.2130651f, 1f);
    Color Desert_B = new Vector4(0.490566f, 0.1272695f, 0.2161333f, 1f);

    Color SnowField_A = new Vector4(0.4588201f, 0.2337131f, 0.490566f, 1f);
    Color SnowField_B = new Vector4(1f, 1f, 1f, 1f);

    Color Color_A, Color_B;

    int SkychSign;

    void Start()
    {
        

        StartCoroutine("ChangeSkycolor");
     

        if (SceneManager.GetActiveScene().buildIndex == 1)
        {
            Color_A = GlassLand_A; Color_B = GlassLand_B;

        }
        else if (SceneManager.GetActiveScene().buildIndex == 3)
        {
            Color_A = Desert_A; Color_B = Desert_B;

        }
        else if (SceneManager.GetActiveScene().buildIndex == 5)
        {
            Color_A = SnowField_A; Color_B = SnowField_B;
        }
    }


    IEnumerator ChangeSkycolor()
    {
        while (true)
        {
            double i = TrainGameManager.instance.runmeter / GameValue.NextStationMeter;
            GetComponent<MeshRenderer>().material.color = Color.Lerp(Color_A, Color_B, (float)i);
                
            yield return new WaitForSeconds(0.5f);
        }

    }


    void Update()
    {
      

    }
}
