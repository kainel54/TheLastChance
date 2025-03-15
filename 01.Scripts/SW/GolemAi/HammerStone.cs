using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HammerStone : MonoBehaviour
{
    public Transform PlayerPoint {  get; set; }
    private float skllTime;
    private void Update()
    {
        transform.position = new Vector2(PlayerPoint.position.x - 2.3f,PlayerPoint.position.y + 2.6f);
        if(skllTime >= 0.5f)
        {
            skllTime = 0;
            print("³Ê Á×ÀÓ");
            PlayerPoint.GetComponent<PlayerHealth>().ApplyDamage(0);
            CameraManager.Instance.ShakeCam(0.5f, 25f);
            Destroy(gameObject);

        }
        else
        {
            skllTime += Time.deltaTime;
        }
    }
}
