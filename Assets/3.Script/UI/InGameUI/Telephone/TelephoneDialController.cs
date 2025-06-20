using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;
using TMPro;

public class TelephoneDialController : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    public int selectedNumber;
    public string currentNumber;
    public TextMeshProUGUI numberView;

    [SerializeField] DialNumberButton[] buttons;
    [SerializeField] private GameObject roulette;
    [SerializeField] float[] maxRotateAngles;
    [SerializeField] Vector2 dialCenter;

    bool canRotate = true;

    void Start()
    {
        dialCenter = new Vector2(0f, 0f);

        InitAction();
    }

    void LateUpdate()
    {
        if (!isDragging || !canRotate || selectedNumber == 0) return;
        LotateRoulette();
    }

    void InitAction()
    {
        foreach (var btn in buttons)
        {
            btn.OnClickButton += SetSelectButton;
            btn.OnDragStart += SetSelectButton;
            btn.OnDragEnd += ResetRoulette;
        }
    }

    private void SetSelectButton(int num)
    {
        selectedNumber = num;
    }

    //룰렛을 원위치.
    private void ResetRoulette()
    {
        canRotate = false;
        roulette.transform.DORotate(Vector3.forward, 0.5f).OnComplete(() => canRotate = true);

        if (!hasReachedMaxAngle) return;

        currentNumber += selectedNumber.ToString();
        numberView.text = currentNumber;

        hasReachedMaxAngle = false;

        //앵글 리셋후에 전화 시퀀스 시작하기.
        if (numberView.text.Length >= 4)
        {
            StartCallSquence();
        }
    }

    private bool hasReachedMaxAngle = false;

    void LotateRoulette()
    {
        if (roulette == null) return;

        float currentZ = roulette.transform.localEulerAngles.z;
        float normalizedCurrentZ = currentZ > 180 ? currentZ - 360f : currentZ;
        float targetZ = normalizedCurrentZ - rotateSpeed * Time.deltaTime;
        float maxZ = maxRotateAngles[selectedNumber];

        if (targetZ < maxZ)
        {
            targetZ = maxZ;
            hasReachedMaxAngle = true;
        }

        roulette.transform.DOLocalRotate(new Vector3(0, 0, targetZ), 0.4f, RotateMode.Fast)
                        .SetEase(Ease.Linear);
    }

    public void OnClickDialExit()
    {
        selectedNumber = 0;
        this.gameObject.SetActive(false);
    }

    private bool isDragging = false;

    [SerializeField] Vector2 clickPoint;
    [SerializeField] float rotateSpeed;

    public void OnBeginDrag(PointerEventData eventData)
    {
        isDragging = true;
        clickPoint = eventData.position;

        Debug.Log("다이얼 드래그 시작!");
    }

    public void OnDrag(PointerEventData eventData)
    {
        Debug.Log("드래그 중...");
        rotateSpeed = Mathf.Abs(Vector2.Distance(clickPoint, eventData.position)) * 5f;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        isDragging = false;
        Debug.Log("다이얼 드래그 종료!");
        ResetRoulette();
    }

    public void OnClickDialNumber(int n)
    {
        selectedNumber = n;
    }

    void StartCallSquence()
    {
        Debug.Log("전화 중...");
        Sequence seq = DOTween.Sequence();
    }

    void OnEnable()
    {
        numberView.text = "";
        currentNumber = "";
        selectedNumber = 0;
    }
}