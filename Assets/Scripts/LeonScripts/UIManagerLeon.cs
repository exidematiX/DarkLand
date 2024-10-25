using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManagerLeon : MonoBehaviour
{
    public Text lightText;

    private void Start()
    {
        UpdateUI();
        ResourceManagerLeon.instance.OnResourceChanged += UpdateUI;
    }
    private void OnDisable()
    {
        ResourceManagerLeon.instance.OnResourceChanged -= UpdateUI;
    }

    void UpdateUI()//¸üÐÂUI
    {
        lightText.text =" "+ResourceManagerLeon.instance.GetResourceAmount("light");
    }
}
