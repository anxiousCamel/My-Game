using Inventory.Model;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    [field: SerializeField]
    public ItemSO InventoryItem { get; private set; }

    [field: SerializeField]
    public int Quantity { get; set; } = 1;

    [Header("Animation")]
    [SerializeField]
    public float duration = 0.5f;

    [Header("Sound")]
    public AudioClip soundPickUp;
    [ReadOnly] public AudioSource audioSource;
    AudioManager audioManager;


    private void Start()
    {
        audioManager = GetComponent<AudioManager>();
        GetComponent<SpriteRenderer>().sprite = InventoryItem.ItemImage;
        audioSource = GetComponent<AudioSource>();
    }

    public void DestroyItem()
    {
        GetComponent<Collider2D>().enabled = false;
        StartCoroutine(AnimateItemPickup());

    }

    private IEnumerator AnimateItemPickup()
    {
        audioManager.PlaySoundRandomPitch(soundPickUp);
        Vector3 startScale = transform.localScale;
        Vector3 endScale = Vector3.zero;
        float currentTime = 0;
        while (currentTime < duration)
        {
            currentTime += Time.deltaTime;
            transform.localScale =
                Vector3.Lerp(startScale, endScale, currentTime / duration);
            yield return null;
        }
        Destroy(gameObject);
    }
}