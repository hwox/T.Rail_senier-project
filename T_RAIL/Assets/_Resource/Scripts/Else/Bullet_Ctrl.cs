using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet_Ctrl : MonoBehaviour {


    public void CallMoveCoroutin()
    {
        StartCoroutine(MoveBullet());
    }

	IEnumerator MoveBullet()
    {
        float timer = 0;
        while (true)
        {
            timer += Time.deltaTime;

            if (timer > 2)
                break;

            transform.Translate(Vector3.forward * Time.deltaTime * GameValue.bullet_speed);
            yield return null;

        }
        gameObject.SetActive(false);
    }
}
