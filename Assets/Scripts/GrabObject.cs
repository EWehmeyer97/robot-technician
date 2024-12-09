using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(MeshCollider))]
[RequireComponent(typeof(MouseInteraction))]

public class GrabObject : MonoBehaviour
{
    public string componentName;
    
    [SerializeField] protected Rigidbody rb;
    [SerializeField] protected DetachObject[] detachable;

    //Material Properties
    protected Material mat;
    private readonly int hover = Shader.PropertyToID("_isHover"); //value found in shader to control hover state
    
    //For Dragging
    private Vector3 offset;
    private EventTrigger trigger;

    //Starting Location
    protected Vector3 originPos;
    protected Quaternion originRot;

    //Booleans for mouse state
    protected bool grabbed = false;
    private bool mouseLeftHover = false; //acounts for mouse no longer hovering over object upon grab being released

    public delegate void OnGrabDelegate(bool connected);
    public event OnGrabDelegate grabUpdate;

    public DetachObject[] Detachables { get { return detachable; } }

    //Adds a rigidbody in case one has not been set manually
    protected virtual void Awake()
    {
        if(rb == null)
            rb = gameObject.AddComponent<Rigidbody>();

        mat = GetComponent<Renderer>().sharedMaterial;

        originPos = rb.transform.localPosition;
        originRot = rb.transform.localRotation;
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

        grabUpdate?.Invoke(false);
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

    public virtual void ResetPosition(bool fromUI = false)
    {
        rb.transform.SetLocalPositionAndRotation(originPos, originRot);
        foreach (var item in detachable)
            item.ResetPosition(fromUI);

        if(fromUI)
            ActivateHover();

        grabUpdate?.Invoke(true);
    }
}
