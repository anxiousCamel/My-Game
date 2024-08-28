
using System.Collections;
using System.Collections.Generic;
using Inventory.UI;
using UnityEngine;
using UnityEngine.UI;

public class DescriptionFollower : MonoBehaviour
{
    [ReadOnly][SerializeField] Canvas canvas;

    private void Awake()
    {
        canvas = transform.root.GetComponent<Canvas>();
    }

    private void Update()
    {
        Vector2 position;

        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            (RectTransform)canvas.transform,
            Input.mousePosition,
            canvas.worldCamera,
            out position
        );

        transform.position = canvas.transform.TransformPoint(position);
    }

    public void Toggle(bool val)
    {
        Debug.Log($"Item toggle {val}");
        gameObject.SetActive(val);
    }
}
