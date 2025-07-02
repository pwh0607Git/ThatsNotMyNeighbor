using DG.Tweening;
using UnityEngine;

public class RecordPanel : MonoBehaviour
{
    private RectTransform rectTransform;

    [SerializeField] Vector3 originPoint;
    [SerializeField] Vector3 endPoint;

    void Start()
    {
        TryGetComponent(out rectTransform);
    }

    void OnEnable()
    {
        OpenView();
    }

    void OnDisable()
    {
        rectTransform.anchoredPosition = originPoint;
    }


    void OpenView()
    {
        Sequence seq = DOTween.Sequence();

        seq.AppendCallback(() => gameObject.SetActive(true))
        .AppendCallback(() => rectTransform.DOAnchorPos(endPoint, 1f));
    }
}