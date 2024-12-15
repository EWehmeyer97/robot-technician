using System;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.XR.Interaction.Toolkit;

public class GrabObject : HoverObject
{
    public string componentName;
    [SerializeField] protected DetachObject[] detachable;
    

    //Starting Location
    protected Vector3 originPos;
    protected Quaternion originRot;

    public delegate void OnGrabDelegate(bool connected);
    public event OnGrabDelegate grabUpdate;

    public DetachObject[] Detachables { get { return detachable; } }

    protected override void Awake()
    {
        base.Awake();

        originPos = rb.transform.localPosition;
        originRot = rb.transform.localRotation;

        interactable.selectEntered.AddListener(EnterSelect);
        interactable.selectExited.AddListener(ExitSelect);
    }

    public override void ActivateHover(bool fromParent = false)
    {
        base.ActivateHover(fromParent);
        if (!grabbed)
        {
            foreach (var item in detachable)
                item.ActivateHover(true);
        }
    }

    public override void DeactivateHover(bool fromParent = false)
    {
        base.DeactivateHover(fromParent);
        if (!grabbed)
        {
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

        Vector3 offsetPosition = toFollow.position - rb.position;
        Quaternion startRotation = rb.rotation * Quaternion.Inverse(toFollow.rotation);

        StartCoroutine(DragObject(toFollow, offsetPosition, startRotation));

        grabUpdate?.Invoke(false);
    }

    private IEnumerator DragObject(Transform toFollow, Vector3 offsetPosition, Quaternion startRotation)
    {
        yield return null;

        rb.position = toFollow.position - offsetPosition;
        rb.rotation = toFollow.rotation * startRotation;

        if (grabbed)
            StartCoroutine(DragObject(toFollow, offsetPosition, startRotation));
    }

    public virtual void DropObject()
    {
        grabbed = false;
        rb.useGravity = true;

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
