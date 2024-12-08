using System.Collections;
using System.Collections.Generic;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Rendering;

[RequireComponent(typeof(MeshCollider))]
public class GrabObject : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private Rigidbody rb;
    [SerializeField] private Material mat;

    private readonly int hover = Shader.PropertyToID("_isHover"); //value found in shader
    
    private Vector3 offset;
    private EventTrigger trigger;

    private bool grabbed = false;
    public bool Grabbed { get { return grabbed; } }

    //Enables and Disables Hover glow
    public void OnPointerEnter(PointerEventData eventData)
    {
        if(!grabbed)
            mat.SetInteger(hover, 1);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if(!grabbed)
            mat.SetInteger(hover, 0);
    }


    //Grabs, Drags, and Releases Object
    public void OnPointerDown(PointerEventData eventData)
    {
        grabbed = true;

        rb.useGravity = false; //turns off gravity to prevent additional physics on object

        offset = Input.mousePosition - Camera.main.WorldToScreenPoint(rb.position);

        trigger = gameObject.AddComponent<EventTrigger>();
        EventTrigger.Entry entry = new EventTrigger.Entry();
        entry.eventID = EventTriggerType.Drag;
        entry.callback.AddListener((data) => { OnClickDragDelegate((PointerEventData)data); });
        trigger.triggers.Add(entry);
    }

    private void OnClickDragDelegate(PointerEventData eventData)
    {
        Vector3 mouse = Input.mousePosition;
        mouse -= offset;
        mouse.z = Camera.main.WorldToScreenPoint(rb.position).z;
        rb.position = Camera.main.ScreenToWorldPoint(mouse); //As we are exclusively editing Rigidbody's position, we are okay to edit rb outside of FixedUpdate
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        grabbed = false;

        rb.useGravity = true;

        Destroy(trigger);
    }
}
