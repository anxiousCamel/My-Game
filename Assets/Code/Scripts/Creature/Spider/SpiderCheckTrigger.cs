using System;
using UnityEngine;

public class SpiderCheckTrigger : MonoBehaviour
{
    Spider spider;

    private void Start()
    {
        spider = GetComponentInParent<Spider>();
    }

    [ReadOnly] public String tagPlayer = "Player";
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(spider.inPlatformMode)
        {
            return;
        }
        else
        if (collision.CompareTag(tagPlayer))
        {
            spider.ChangeDown();
        }
    }
}
