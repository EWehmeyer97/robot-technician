using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

[RequireComponent(typeof(MeshCollider))]
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(XRBaseInteractable))]

public class HoverObject : MonoBehaviour
{
    //Booleans for hand state
    protected bool leftHover = false; //acounts for hand no longer hovering over object upon grab being released
    protected bool grabbed = false;

    //Material Properties
    protected Material mat;
    private readonly int hover = Shader.PropertyToID("_isHover"); //value found in shader to control hover state

    //For Dragging
    protected Rigidbody rb;
    protected XRBaseInteractable interactable;

    protected bool isHovered = false;
    protected bool parentIsHovered = false;
    protected bool attachedToBody = true;

    protected virtual void Awake()
    {
        mat = GetComponent<Renderer>().sharedMaterial;

        rb = GetComponent<Rigidbody>();

        interactable = GetComponent<XRBaseInteractable>();
        interactable.firstHoverEntered.AddListener(EnterHover);
        interactable.lastHoverExited.AddListener(ExitHover);
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
        if (fromParent)
            parentIsHovered = true;
        else
            isHovered = true;

        if((parentIsHovered && attachedToBody) || isHovered)
            mat.SetInteger(hover, 1);
    }

    public virtual void DeactivateHover(bool fromParent = false)
    {
        if (fromParent)
            parentIsHovered = false;
        else 
            isHovered = false;

        if(!parentIsHovered && !isHovered)
            mat.SetInteger(hover, 0);
    }
}
