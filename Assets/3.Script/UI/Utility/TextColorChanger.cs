using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class TextColorChanger : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] TextMeshProUGUI targetTMP;
    [SerializeField] Color normalColor;                 //외부에서 색상 지정
    [SerializeField] Color hoverColor;

    void Start()
    {
        // targetTMP.color = normalColor;
        normalColor = targetTMP.color;
    }


    public void OnPointerEnter(PointerEventData eventData)
    {
        targetTMP.color = hoverColor;
    }

    public void OnPointerExit(PointerEventData eventData) {
        targetTMP.color = normalColor;
    }             
}
