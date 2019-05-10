using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MachineGun_Ctrl : MonoBehaviour
{
    // 얘는 총알... 발사.. 해야지..
    // 이거 오브ㅔㄱ트풀링으로 하자


    // 얘를 아예 기차마다 달아놓고
    // 만약에 list.count == 기차 인덱스하면 setactive true할까?




    public void gun_up()
    {
        if (transform.localRotation.y > -40.0f)
        {

            Quaternion rotation = Quaternion.identity;
            rotation.eulerAngles = new Vector3(0, -20.0f * Time.deltaTime, 0); 
            this.gameObject.transform.localRotation *= rotation;
            Debug.Log(transform.localRotation.y);

        }
    }
    public void gun_down()
    {
        if (transform.localRotation.y < 40.0f)
        {
            Quaternion rotation = Quaternion.identity;
            rotation.eulerAngles = new Vector3(0, 20.0f * Time.deltaTime, 0);
            this.gameObject.transform.localRotation *= rotation; Debug.Log(transform.localRotation.y);
        }
    }

    public void gun_fire(bool fire)
    {
        if (fire)
        {

        }
        else if (!fire)
        {

        }
    }
}


