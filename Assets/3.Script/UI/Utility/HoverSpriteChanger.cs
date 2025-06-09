using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class HoverSpriteChanger : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] Image targetImage;

    [SerializeField] Sprite originalSprite;
    [SerializeField] Sprite hoverSprite;


    public void OnPointerEnter(PointerEventData eventData)
    {
        targetImage.sprite = hoverSprite;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        targetImage.sprite = originalSprite;
    }

    public void OnClickButton()
    {
        
    }
}
