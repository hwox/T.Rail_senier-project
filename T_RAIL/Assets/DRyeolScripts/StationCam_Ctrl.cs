using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StationCam_Ctrl : MonoBehaviour
{
    public GameObject cam;
    float player_position_x;
    
    
    // Use this for initialization
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }
    private void LateUpdate()
    {
        //float targetX = cam.gameObject.transform.position.x;
        //Debug.Log(targetX);

        //cam.transform.position = new Vector3(targetX, cam.transform.position.y, cam.transform.position.z);
    }
    public void GetPlayerX(float position_x)
    {
        cam.transform.position = new Vector3(position_x, cam.transform.position.y, cam.transform.position.z);
    }

}
