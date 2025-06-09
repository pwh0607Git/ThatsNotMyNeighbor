using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InteractiveItemStateChanger : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    private bool isOnClicked;

    [SerializeField] Image targetImage;
    [SerializeField] Sprite prevSprite;
    
    [SerializeField] Sprite originalSprite;
    [SerializeField] Sprite hoverSprite;

    [SerializeField] Sprite clickedSprite;
    [SerializeField] Sprite unClickedSprite;

    void Start()
    {
        isOnClicked = false;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (isOnClicked)
        {
            
        }
        else
        {
            
        }
    }

    public void OnPointerEnter(PointerEventData eventData) {
        targetImage.sprite = hoverSprite;
    }

    public void OnPointerExit(PointerEventData eventData) {
        targetImage.sprite = originalSprite;
    }
}