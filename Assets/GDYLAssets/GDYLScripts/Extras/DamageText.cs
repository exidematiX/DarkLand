using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DamageText : MonoBehaviour
{
    //public TextMeshProUGUI DmgText => GetComponent<TextMeshProUGUI>();
    private TextMeshProUGUI _dmgText;

    private void Awake()
    {
        _dmgText = GetComponent<TextMeshProUGUI>();
    }

    public TextMeshProUGUI DmgText => _dmgText;

    public void ReturnTextToPool()
    {
        transform.SetParent(null);
        ObjectPooler.ReturnToPool(gameObject);
    }
}
