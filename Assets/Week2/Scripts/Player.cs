using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Player : MonoBehaviour, IDragHandler, IEndDragHandler, IBeginDragHandler
{
    public static Player instance; //static reference for the player
    RectTransform rectTrans; //this object's rect transform

    public int score { get; private set; } //the manager can get the score but can't change it

    bool isDragging; //is this being dragged around

    void Awake()
    {
        instance = this;
        rectTrans = GetComponent<RectTransform>();
    }

    //when this touches something
    private void OnTriggerEnter(Collider other)
    {
        switch (other.tag)
        {
            case "Jewel": //when you gain a jewel, +1 point
                Destroy(other.gameObject);
                score++;
                break;
            case "Spike": //when you touch a spike, -1 point
                Destroy(other.gameObject);
                score--;
                break;
        }

    }

    //begin dragging
    public void OnBeginDrag(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            if (!isDragging)
                isDragging = true;
        }
    }

    //set position to where the mouse is
    public void OnDrag(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            if (isDragging)
            {
                Vector3 globalMousePos;
                if (RectTransformUtility.ScreenPointToWorldPointInRectangle(rectTrans, eventData.position, eventData.pressEventCamera, out globalMousePos))
                {
                    //set the player to the mouse position, but capped within the edges of the screen 
                    rectTrans.position = new Vector3(
                        Mathf.Clamp(globalMousePos.x, 0, Screen.width),
                        Mathf.Clamp(globalMousePos.y, 0, Screen.height));
                }
            }
        }
    }

    //stop dragging
    public void OnEndDrag(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            if (isDragging)
                isDragging = false;
        }
    }
}
