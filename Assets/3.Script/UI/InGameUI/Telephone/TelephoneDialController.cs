using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;
using TMPro;
using System.Collections.Generic;
using System.Linq;

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
        float maxZ = maxRotateAngles[selectedNumber] < -180f ? maxRotateAngles[selectedNumber] + 360 : maxRotateAngles[selectedNumber];

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
    }

    public void OnDrag(PointerEventData eventData)
    {
        rotateSpeed = Mathf.Abs(Vector2.Distance(clickPoint, eventData.position)) * 5f;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        isDragging = false;
        ResetRoulette();
    }

    public void OnClickDialNumber(int n)
    {
        selectedNumber = n;
    }

    void StartCallSquence()
    {
        Debug.Log("전화 중...");
        Apartment apt = InGameManager.I.SearchApartment(currentNumber);

        //아파트에 지금 주민이 있는지 확인하기
        /*
            경우의수 체크.
            1. 집에 아무도 없는 경우 Dic Value가 모두 false
            2. 집에 한명만 있는 경우 Dic Value 중 하나만 False
            3. 집에 모두 있는 경우.
        */

        if (apt == null || apt.checkAtHome == null) return;

        List<Profile> atHomeProfiles = apt.checkAtHome
                .Where(pair => pair.Value) // 집에 있는 사람만
                .Select(pair => pair.Key)  // Profile만 추출
                .ToList();

        Profile target = null;

        Dialog dialog = null;

        if (atHomeProfiles.Count == 0)
        {
            Debug.Log("아무도 집에 없습니다.");

            DOVirtual.DelayedCall(3f, () => ResetDial());
            return;
        }
        else if (atHomeProfiles.Count >= apt.checkAtHome.Count)
        {
            Debug.Log("모두 집에 있습니다.");
            target = atHomeProfiles[Random.Range(0, atHomeProfiles.Count)];
            dialog = target.dialogs.Find(d => d.code.Equals("Call_With_Mate"));
        }
        else
        {
            Debug.Log("부분 인원만 집에 있습니다.");
            target = atHomeProfiles[Random.Range(0, atHomeProfiles.Count)];
            dialog = target.dialogs.Find(d => d.code.Equals("Call_With_None"));
        }

        Debug.Log($"Target : {target}");
        Debug.Log($"DiaLog : {dialog.code}");

        if (dialog == null)
        {
            Debug.Log("적절한 대화가 없습니다.");
            return;
        }

        // 3초 후 대사 출력
        DOVirtual.DelayedCall(3f, () => InGameUIController.I.ShowTextBox(dialog));
    }

    void OnEnable()
    {
        ResetDial();
    }

    private void ResetDial()
    {
        numberView.text = "";
        currentNumber = "";
        selectedNumber = 0;
    }
}