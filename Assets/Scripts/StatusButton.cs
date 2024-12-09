using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
[RequireComponent(typeof(Button))]
[RequireComponent(typeof(EventTrigger))]
public class StatusButton : MonoBehaviour
{
    [SerializeField] private Sprite attachedImage;
    [SerializeField] private Sprite unattachedImage;

    [Space]

    [SerializeField] private TextMeshProUGUI componentText;
    [SerializeField] private TextMeshProUGUI statusText;

    private Image image;
    private Button button;
    private EventTrigger trigger;

    private GrabObject grab;

    private bool attached = true;

    public void Setup(GrabObject grabObject, bool isParent)
    {
        grab = grabObject;
        componentText.text = grabObject.componentName;
        statusText.gameObject.SetActive(!isParent); //Parent will always be attached

        if (!isParent)
            grab.grabUpdate += UpdateState;
    }

    private void Awake()
    {
        image = GetComponent<Image>();
        button = GetComponent<Button>();
        trigger = GetComponent<EventTrigger>();
    }

    private void Start()
    {
        button.onClick.AddListener(Clicked);

        //Hover Setup
        EventTrigger.Entry entry = new EventTrigger.Entry();
        entry.eventID = EventTriggerType.PointerEnter;
        entry.callback.AddListener((data) => { grab.ActivateHover(); });
        trigger.triggers.Add(entry);

        entry = new EventTrigger.Entry();
        entry.eventID = EventTriggerType.PointerExit;
        entry.callback.AddListener((data) => { grab.DeactivateHover(); });
        trigger.triggers.Add(entry);
    }

    private void Clicked()
    {
        grab.ResetPosition(true);

        UpdateState(true);
    }

    public void UpdateState(bool connect)
    {
        attached = connect;
        if (connect)
        {
            statusText.text = "attached";
            statusText.color = Color.green;
            image.sprite = attachedImage;
        } else
        {
            statusText.text = "detached";
            statusText.color = Color.red;
            image.sprite = unattachedImage;
        }
    }
}
