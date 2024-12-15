using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

[RequireComponent(typeof(MeshCollider))]
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(XRSimpleInteractable))]

public class HoverObject : MonoBehaviour
{
    //Booleans for mouse state
    protected bool mouseLeftHover = false; //acounts for mouse no longer hovering over object upon grab being released
    protected bool grabbed = false;

    //Material Properties
    protected Material mat;
    private readonly int hover = Shader.PropertyToID("_isHover"); //value found in shader to control hover state

    //For Dragging
    protected Rigidbody rb;
    protected XRSimpleInteractable interactable;

    protected virtual void Awake()
    {
        mat = GetComponent<Renderer>().sharedMaterial;

        rb = GetComponent<Rigidbody>();

        interactable = GetComponent<XRSimpleInteractable>();
        interactable.hoverEntered.AddListener(EnterHover);
        interactable.hoverExited.AddListener(ExitHover);
    }

    #region"Methods required to interact through an XR Interactable"
    private void EnterHover(HoverEnterEventArgs args)
    {
        ActivateHover();
    }

    private void ExitHover(HoverExitEventArgs arg0)
    {
        DeactivateHover();
    }
    #endregion

    public virtual void ActivateHover(bool fromParent = false)
    {
        if (!grabbed)
        {
            mat.SetInteger(hover, 1);
        }
        else
        {
            mouseLeftHover = false;
        }
    }

    public virtual void DeactivateHover(bool fromParent = false)
    {
        if (!grabbed)
        {
            mat.SetInteger(hover, 0);
        }
        else
        {
            mouseLeftHover = true;
        }
    }
}
