using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeLight : MonoBehaviour {

  

    Color GlassLand_LightA = new Vector4(1f, 1f, 1f, 1f);
    Color GlassLand_LightB = new Vector4(0.9056604f, 0.498312f, 0.2264151f, 1f);

    Color Desert_LightA = new Vector4(0.4142043f, 0.4142043f, 0.490566f, 1f);
    Color Desert_LightB = new Vector4(0.1041296f, 0.1041296f, 0.490566f, 1f);

    Color SnowField_LightA = new Vector4(0.6548199f, 0.4488697f, 0.7264151f, 1f);
    Color SnowField_LightB = new Vector4(1f, 1f, 1f, 1f);


    Color Light_A, Light_B;
    int SceneNumber;

    int angle;

    public Light li;
    
    // Use this for initialization
    void Start () {
        StartCoroutine("Changelightcolor");
        
        li = GetComponent<Light>();
        SceneNumber = SceneManager.GetActiveScene().buildIndex;

        if (SceneNumber == 1)
        {

            Light_A = GlassLand_LightA; Light_B = GlassLand_LightB;
            angle = 0;
        }
        else if (SceneNumber == 3)
        {

            Light_A = Desert_LightA; Light_B = Desert_LightB;
            angle = 120;
        }
        else if (SceneNumber == 5)
        {

            Light_A = SnowField_LightA ; Light_B = SnowField_LightB;
            angle = 240;
        }

    }

    IEnumerator Changelightcolor()
    {
        while (true)
        {
            double i = TrainGameManager.instance.runmeter / GameValue.NextStationMeter;

            //Debug.Log(i);

            li.color = Color.Lerp(Light_A, Light_B, (float)i);
           
           
            li.transform.rotation = Quaternion.Euler(34.945f, (float)i * 120 + angle, -70.30901f);
            yield return new WaitForSeconds(0.5f);
        }

    }

    // Update is called once per frame
    void Update () {
      
    }
}
