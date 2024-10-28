using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManagerLeon : MonoBehaviour
{
    public Text lightText;

    private void Start()
    {
        lightText = transform.Find("HumanCondition/Resource/light/lightAmount").gameObject.GetComponent<Text>();
        UpdateUI();
        ResourceManagerLeon.instance.OnResourceChanged += UpdateUI;
    }
    private void OnDisable()
    {
        ResourceManagerLeon.instance.OnResourceChanged -= UpdateUI;
    }

    void UpdateUI()//更新UI
    {
        lightText.text =" "+ResourceManagerLeon.instance.GetResourceAmount("light");
    }
}
