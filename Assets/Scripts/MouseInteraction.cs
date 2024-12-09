using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MouseInteraction : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerEnterHandler, IPointerExitHandler
{
    private GrabObject grab;

    void Awake()
    {
        grab = GetComponent<GrabObject>();
    }

    //Enables Hover Glow
    public void OnPointerEnter(PointerEventData eventData)
    {
        grab.ActivateHover();
    }

    //Disables Hover Glow
    public void OnPointerExit(PointerEventData eventData)
    {
        grab.DeactivateHover();
    }

    //Grabs Object
    public virtual void OnPointerDown(PointerEventData eventData)
    {
        grab.GrabsObject();
    }

    //Releases Object
    public virtual void OnPointerUp(PointerEventData eventData)
    {
        grab.DropObject();
    }
}
