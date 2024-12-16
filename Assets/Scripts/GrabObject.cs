using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

[RequireComponent(typeof(XRGrabInteractable))]
public class GrabObject : HoverObject
{
    [SerializeField] protected DetachObject[] detachable;

    //Starting Location
    protected Vector3 originPos;
    protected Quaternion originRot;

    protected override void Awake()
    {
        base.Awake();

        originPos = rb.transform.localPosition;
        originRot = rb.transform.localRotation;

        interactable.firstSelectEntered.AddListener(EnterSelect);
        interactable.lastSelectExited.AddListener(ExitSelect);
    }

    public override void ActivateHover(bool fromParent = false)
    {
        if (!grabbed)
        {
            base.ActivateHover(fromParent);
            foreach (var item in detachable)
                item.ActivateHover(true);
        }
    }

    public override void DeactivateHover(bool fromParent = false)
    {
        if (!grabbed)
        {
            base.DeactivateHover(fromParent);
            foreach (var item in detachable)
                item.DeactivateHover(true);
        }
    }

    #region"Methods required to interact through an XR Interactable"
    private void EnterSelect(SelectEnterEventArgs args)
    {
        GrabsObject(args.interactorObject.transform);
    }

    private void ExitSelect(SelectExitEventArgs args)
    {
        DropObject();
    }

    #endregion

    //Grabs Object
    public virtual void GrabsObject(Transform toFollow)
    {
        grabbed = true;

        rb.useGravity = false; //turns off gravity to prevent additional physics on object
        rb.velocity = Vector3.zero; //removes all current movement from rigidbody
    }

    public virtual void DropObject()
    {
        grabbed = false;
        rb.useGravity = true;

        if (leftHover) //triggers hover deactivate in case left during the drag
            DeactivateHover();
        leftHover = false;
    }

    public virtual void ResetPosition()
    {
        rb.transform.SetLocalPositionAndRotation(originPos, originRot);
        foreach (var item in detachable)
            item.ResetPosition();
    }
}
