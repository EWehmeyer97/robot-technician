using UnityEngine;

[RequireComponent(typeof(MeshCollider))]
[RequireComponent(typeof(MouseInteraction))]

public class HoverObject : MonoBehaviour
{
    public string componentName;

    [SerializeField] protected Rigidbody rb;

    //Material Properties
    protected Material mat;
    private readonly int hover = Shader.PropertyToID("_isHover"); //value found in shader to control hover state

    protected bool isHovered = false;
    protected bool parentIsHovered = false;
    protected bool attachedToBody = true;

    public virtual void ActivateHover(bool fromParent = false)
    {
        if (fromParent)
            parentIsHovered = true;
        else
            isHovered = true;

        if ((parentIsHovered && attachedToBody) || isHovered)
            mat.SetInteger(hover, 1);
    }

    public virtual void DeactivateHover(bool fromParent = false)
    {
        if (fromParent)
            parentIsHovered = false;
        else
            isHovered = false;

        if ((!parentIsHovered || !attachedToBody) && !isHovered)
            mat.SetInteger(hover, 0);
    }
}
