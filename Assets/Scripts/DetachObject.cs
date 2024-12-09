using UnityEngine;
using UnityEngine.EventSystems;

public class DetachObject : GrabObject
{
    [SerializeField] private float distanceSnap = 0.1f;

    private Vector3 originPos;
    private Quaternion originRot;

    private bool attachedToBody = true;

    protected override void Awake()
    {
        base.Awake();

        rb.isKinematic = true; //detach objects only react when clicked

        mat = new Material(mat); //Instantiate New Material
        SetupMaterial(mat, transform); //Place New Material on Correct Objects


        originPos = transform.localPosition;
        originRot = transform.localRotation;
    }

    private void SetupMaterial(Material material, Transform trans)
    {
        if(trans.GetComponent<Renderer>() != null)
            trans.GetComponent<Renderer>().sharedMaterial = material;

        for(int i = 0; i < trans.childCount; i++)
            if(trans.GetChild(i).GetComponent<DetachObject>() == null)
                SetupMaterial(material, trans.GetChild(i));
    }

    public override void OnPointerDown(PointerEventData eventData)
    {
        //Activate Rigidbody
        rb.isKinematic = false;
        attachedToBody = false;

        base.OnPointerDown(eventData);
    }

    public override void OnPointerUp(PointerEventData eventData)
    {
        base.OnPointerUp(eventData);

        //Reattach if located near anchor point
        if (Vector3.Distance(originPos, transform.localPosition) <= distanceSnap)
            Reattach();
    }

    //Reattaches ligament to body 
    public void Reattach()
    {
        rb.isKinematic = true;

        transform.localPosition = originPos;
        transform.localRotation = originRot;
        attachedToBody = true;
    }

    public override void ActivateHover(bool fromParent = false)
    {
        if(!fromParent || attachedToBody)
            base.ActivateHover(fromParent);
    }

    public override void DeactivateHover(bool fromParent = false)
    {
        if (!fromParent || attachedToBody)
            base.DeactivateHover(fromParent);
    }
}
