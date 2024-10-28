using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DisableRectChoose : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public void OnPointerEnter(PointerEventData eventData)
    {
        PlayerController.Instance.disableRectChoose = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        PlayerController.Instance.disableRectChoose = false;
    }

}
