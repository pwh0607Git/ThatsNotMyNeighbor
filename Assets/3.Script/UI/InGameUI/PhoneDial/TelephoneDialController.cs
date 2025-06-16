using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

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
        roulette.transform.DORotate(Vector3.forward, 0.8f).OnComplete(() => canRotate = true);
        
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

        // 현재 각도가 0~360이므로 -Z 방향 기준으로 재해석
        float normalizedCurrentZ = currentZ > 180 ? currentZ - 360f : currentZ;

        // 목표 각도 계산 (시계 방향 회전 → -값 증가)
        float targetZ = normalizedCurrentZ - rotateSpeed * Time.deltaTime;

        // 최대 회전 각도 제한 (selectedNumber에 따라 -값 기준)
        float maxZ = maxRotateAngles[selectedNumber];

        // Clamp: 목표 각도가 maxZ보다 더 작아지지 않도록
        if (targetZ < maxZ)
        {
            targetZ = maxZ;
            
            hasReachedMaxAngle = true;
        }

        // DOTween 적용 (Fast mode는 0~360 기준이므로 그대로 사용)
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
    [SerializeField] float rotateSpeed;                 //드래그 정도에 따른 동적 속도

    public void OnBeginDrag(PointerEventData eventData)
    {
        isDragging = true;
        clickPoint = eventData.position;

        Debug.Log("다이얼 드래그 시작!");
    }
    public void OnDrag(PointerEventData eventData)
    {
        Debug.Log("드래그 중...");

        // 회전 처리 가능
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
}