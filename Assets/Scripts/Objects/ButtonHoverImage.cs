using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ButtonHoverImage : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private Image hoverImage;

    private void Start()
    {
        if (hoverImage)
            hoverImage.enabled = false;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (hoverImage)
            hoverImage.enabled = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (hoverImage)
            hoverImage.enabled = false;;
    }
}