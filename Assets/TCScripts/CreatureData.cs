using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreatureData : MonoBehaviour
{
    public Transform chosenMark; 
    [SerializeField]
    private bool _isChosen = false;

    private void Start()
    {
        chosenMark = transform.Find("Canvas/ChosenMark");
    }

    public bool IsChosen 
    { 
        get => _isChosen;
        set 
        {
            if(value != _isChosen)
            {
                chosenMark?.gameObject.SetActive(value);
            }
            _isChosen = value;
        } 
    }
}
