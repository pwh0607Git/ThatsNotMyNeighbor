using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class DialNumberButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] private int num;
    [SerializeField] TextMeshProUGUI targetTMP;
    [SerializeField] Color normalColor;                 //외부에서 색상 지정
    [SerializeField] Color clickedColor;                // 클릭된 상태

    public UnityAction<int> OnClickButton;
    public UnityAction<int> OnDragStart;
    public UnityAction OnDragEnd;

    void Start()
    {
        normalColor = targetTMP.color;
        targetTMP = GetComponentInChildren<TextMeshProUGUI>();
    }

    // 마우스 클릭 다운
    public void OnPointerDown(PointerEventData eventData)
    {
        targetTMP.color = clickedColor;
        OnClickButton?.Invoke(num);
    }

    // 마우스 클릭릭 업
    public void OnPointerUp(PointerEventData eventData)
    {
        targetTMP.color = normalColor;
    }
}
