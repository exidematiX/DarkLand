using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnitData : MonoBehaviour
{
    [SerializeField]
    private float _health = 100f;
    public float Health
    {
        get => _health;
        set
        {
            if (value != _health)
            {
                if (healthSlider.GetComponent<Slider>() != null)
                {
                    healthSlider.GetComponent<Slider>().value = value;
                }
            }
            _health = value;
        }
    }

    public GameObject chooseUIPrefab;
    public Transform chosenMark;
    public Transform healthSlider;

    [SerializeField]
    private bool _isChosen = false;

    public bool IsChosen
    {
        get => _isChosen;
        set
        {
            if (value != _isChosen)
            {
                Debug.Log(value);
                chosenMark?.gameObject.SetActive(value);
            }
            _isChosen = value;
        }
    }
    protected virtual void Start()
    {
        chosenMark = transform.Find("Canvas/ChosenMark");
        healthSlider = transform.Find("Canvas/HealthSlider");
    }

    void Update()
    {
        
    }
}
