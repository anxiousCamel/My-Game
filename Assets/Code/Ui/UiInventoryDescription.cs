using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UiInventoryDescription : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI title;
    [SerializeField] private TextMeshProUGUI type;
    [SerializeField] private TextMeshProUGUI description;

    private void Awake() 
    {
        ResetDescription();
    }

    public void ResetDescription()
    {
        this.title.text = "";
        this.type.text = "";
        this.description.text = "";
    }

    public void SetDescription(string name, string type, string description)
    {
        this.title.text = name;
        this.type.text = type;
        this.description.text = description;
    }
}
