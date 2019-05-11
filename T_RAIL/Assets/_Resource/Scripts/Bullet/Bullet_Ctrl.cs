using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Bullet_Ctrl : MonoBehaviour
{

    Rigidbody rig;


    Transform tr;
 //   public Slider m_AimSlider;

    //float m_MinLaunchForce = 15.0f;
    //float m_MaxLaunchForce = 30.0f;
    //float m_MaxChargeTime = 0.75f;

    float m_CurrentLaunchForce;
    //float m_ChargeSpeed;
    //bool m_Fired;

    private void Awake()
    {
        rig = GetComponent<Rigidbody>();
        tr = GetComponent<Transform>();
    }

    private void Start()
    {
       // m_ChargeSpeed = (m_MaxLaunchForce - m_MinLaunchForce) / m_MaxChargeTime;
    }

    public void CallMoveCoroutin(float _value)
    {
        StartCoroutine(MoveBullet());
        m_CurrentLaunchForce = _value;

        rig.velocity = transform.forward * m_CurrentLaunchForce;
    }

    private void OnDisable()
    {

    }

    private void OnEnable()
    {
        //  m_CurrentLaunchForce = m_MinLaunchForce;

        rig.isKinematic = true;
        rig.isKinematic = false;
    }

    IEnumerator MoveBullet()
    {
        float timer = 0;
        while (true)
        {
            timer += Time.deltaTime;

            if (timer > 2.0f)
                break;
            else if(tr.position.y < 2.0f)
            {

                gameObject.SetActive(false);
            }

            // m_AimSlider.value = m_MinLaunchForce;
            Vector3 temp = rig.velocity.normalized;

            transform.LookAt(transform.position + temp);
            // 현재 바라보고 있는 방향으로 회전
            yield return null;

        }
        gameObject.SetActive(false);
    }
}
