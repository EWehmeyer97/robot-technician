using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(MeshCollider))]
[RequireComponent(typeof(MouseInteraction))]

public class GrabObject : MonoBehaviour
{
    [SerializeField] protected Rigidbody rb;
    [SerializeField] protected DetachObject[] detachable;

    protected Material mat;
    private readonly int hover = Shader.PropertyToID("_isHover"); //value found in shader to control hover state
    
    private Vector3 offset;
    private EventTrigger trigger;

    protected Vector3 originPos;
    protected Quaternion originRot;

    protected bool grabbed = false;
    private bool mouseLeftHover = false; //acounts for mouse no longer hovering over object upon grab being released

    //Adds a rigidbody in case one has not been set manually
    protected virtual void Awake()
    {
        if(rb == null)
            rb = gameObject.AddComponent<Rigidbody>();

        mat = GetComponent<Renderer>().sharedMaterial;

        originPos = transform.localPosition;
        originRot = transform.localRotation;
    }

    public virtual void ActivateHover(bool fromParent = false)
    {
        if (!grabbed)
        {
            mat.SetInteger(hover, 1);
            foreach(var item in detachable)
                item.ActivateHover(true);
        } else
        {
            mouseLeftHover = false;
        }
    }

    public virtual void DeactivateHover(bool fromParent = false)
    {
        if (!grabbed)
        {
            mat.SetInteger(hover, 0);
            foreach(var item in detachable)
                item.DeactivateHover(true);
        } else
        {
            mouseLeftHover = true;
        }
    }

    //Grabs Object
    public virtual void GrabsObject()
    {
        grabbed = true;

        rb.useGravity = false; //turns off gravity to prevent additional physics on object
        rb.velocity = Vector3.zero; //removes all current movement from rigidbody

        offset = Input.mousePosition - Camera.main.WorldToScreenPoint(rb.position);

        trigger = gameObject.AddComponent<EventTrigger>();
        EventTrigger.Entry entry = new EventTrigger.Entry();
        entry.eventID = EventTriggerType.Drag;
        entry.callback.AddListener((data) => { DragObject((PointerEventData)data); });
        trigger.triggers.Add(entry);
    }

    //Drags Object
    private void DragObject(PointerEventData eventData)
    {
        Vector3 mouse = Input.mousePosition;
        mouse -= offset;
        mouse.z = Camera.main.WorldToScreenPoint(rb.position).z;
        rb.position = Camera.main.ScreenToWorldPoint(mouse); //As we are exclusively editing Rigidbody's position, we are okay to edit rb outside of FixedUpdate
        rb.velocity = Vector3.zero; //removes all current movement from rigidbody
    }

    public virtual void DropObject()
    {
        grabbed = false;

        rb.useGravity = true;

        Destroy(trigger);

        if (mouseLeftHover) //triggers hover deactivate in case mouse left during the drag
            DeactivateHover();
        mouseLeftHover = false;
    }

    public virtual void ResetPosition()
    {
        transform.localPosition = originPos;
        transform.localRotation = originRot;
    }
}
