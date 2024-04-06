using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Hover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] Image image;
    [SerializeField] TMP_Text description;

    private void Start()
    {
        image.gameObject.SetActive(false);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        image.gameObject.SetActive(true);
        description.text = this.name;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        image.gameObject.SetActive(false);
    }
}
