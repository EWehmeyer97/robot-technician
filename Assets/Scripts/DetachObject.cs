using UnityEngine;

public class DetachObject : GrabObject
{
    [SerializeField] private float distanceSnap = 0.1f;

    protected override void Awake()
    {
        base.Awake();

        rb.isKinematic = true; //detach objects only react when clicked

        mat = new Material(mat); //Instantiate New Material
        SetupMaterial(mat, transform); //Place New Material on Correct Objects
    }

    private void SetupMaterial(Material material, Transform trans)
    {
        if(trans.GetComponent<Renderer>() != null)
            trans.GetComponent<Renderer>().sharedMaterial = material;

        for(int i = 0; i < trans.childCount; i++)
            if(trans.GetChild(i).GetComponent<DetachObject>() == null)
                SetupMaterial(material, trans.GetChild(i));
    }

    public override void GrabsObject()
    {
        //Activate Rigidbody
        rb.isKinematic = false;
        attachedToBody = false;

        base.GrabsObject();
    }

    public override void DropObject()
    {
        base.DropObject();

        //Reattach if located near anchor point
        if (Vector3.Distance(originPos, transform.localPosition) <= distanceSnap)
            ResetPosition();
    }

    //Reattaches ligament to body 
    public override void ResetPosition(bool fromUI = false, bool fromParent = false)
    {
        rb.isKinematic = true;
        attachedToBody = true;

        base.ResetPosition(fromUI);

        if(fromUI && fromParent)
            DeactivateHover();
    }
}
