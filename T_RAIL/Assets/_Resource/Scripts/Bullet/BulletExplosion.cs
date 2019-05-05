using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletExplosion : MonoBehaviour {

    Rigidbody rig;

    private void Awake()
    {
        rig = GetComponent<Rigidbody>();
    }

    IEnumerator Explosionhide()
    {
        int Rand = Random.Range(3, 5);
        float timer = 0;
        while (true)
        {
            timer += Time.deltaTime;
            if(timer > Rand)
            {
                break;
            }
            yield return null;
        }

        // 물체가 사라진 후 다시 소환됐을 때 전에 받던 물리력을 없애주기 위해 kinematic다시
        rig.isKinematic = true;
        rig.isKinematic = false; // 물리력 다시 받기 위해서 해제
        this.gameObject.SetActive(false);
    }

    public void StartExplosionHide()
    {
        StartCoroutine(Explosionhide());
    }
}
