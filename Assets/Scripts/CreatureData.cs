using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CreatureData : MonoBehaviour
{
    [SerializeField]
    private float _health = 100f;
    public float atkValue = 30f;
    public float atkRange = 2f;

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
                chosenMark?.gameObject.SetActive(value);
            }
            _isChosen = value;
        }
    }

    private void Start()
    {
        chosenMark = transform.Find("Canvas/ChosenMark");
        healthSlider = transform.Find("Canvas/HealthSlider");
    }    
}
