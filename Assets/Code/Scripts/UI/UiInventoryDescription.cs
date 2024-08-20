using System.Collections;
using System.Collections.Generic;
using Microsoft.Unity.VisualStudio.Editor;
using TMPro;
using UnityEngine;

namespace Inventory.UI
{
    public class UiInventoryDescription : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI title;
        [SerializeField] private TextMeshProUGUI type;
        [SerializeField] private TextMeshProUGUI description;
        [SerializeField] private GameObject gameObjectDescription;


        private void Awake()
        {
            ResetDescription();
        }

        public void ResetDescription()
        {
            this.title.text = "";
            this.type.text = "";
            this.description.text = "";
            this.gameObjectDescription.SetActive(false);
        }

        public void SetDescription(string name, string type, string description)
        {
            this.title.text = name;
            this.type.text = type;
            this.description.text = description;
            this.gameObjectDescription.SetActive(true);
        }
    }
}
