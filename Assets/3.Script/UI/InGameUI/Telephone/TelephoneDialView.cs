using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;
using TMPro;
using System.Collections.Generic;
using System.Linq;

public class TelephoneDialView : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    public int selectedNumber;
    public string currentNumber;
    public TextMeshProUGUI numberView;

    [SerializeField] DialNumberButton[] buttons;
    [SerializeField] private GameObject roulette;
    [SerializeField] float[] maxRotateAngles;
    [SerializeField] Vector2 dialCenter;

    [Header("Sound Clip")]
    [SerializeField] AudioClip callClip;
    [SerializeField] AudioClip rotateDialClip;
    [SerializeField] AudioClip resetDialClip;
    [SerializeField] AudioClip noneCallClip;

    bool canRotate = true;

    void Start()
    {
        dialCenter = new Vector2(0f, 0f);

        InitAction();
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

    void LotateRoulette()
    {
        if (!isDragging || !canRotate || selectedNumber == 0) return;

        rotateSpeed = 0.8f - (selectedNumber / 50);
        roulette.transform.DORotate(new Vector3(0, 0, maxRotateAngles[selectedNumber]), rotateSpeed, RotateMode.FastBeyond360);
    }

    //룰렛을 원위치.
    private void ResetRoulette()
    {
        canRotate = false;
        roulette.transform.DORotate(Vector3.forward, 0.5f).OnComplete(() => canRotate = true);

        SoundManager.I.SetEffectAudio(resetDialClip);

        if (selectedNumber == 0) return;

        currentNumber += selectedNumber.ToString();
        numberView.text = currentNumber;

        //앵글 리셋후에 전화 시퀀스 시작하기.
        if (numberView.text.Length >= 4)
        {
            StartCallSquence();
        }
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

        //시작시.. 두트윈으롤 로테이션 수행
        LotateRoulette();
    }

    public void OnDrag(PointerEventData eventData)
    {
        rotateSpeed = Mathf.Abs(Vector2.Distance(clickPoint, eventData.position)) * 2f;
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
        Apartment apt = InGameManager.I.SearchApartment(currentNumber);

        if (apt == null || apt.checkAtHome == null) return;

        List<Profile> atHomeProfiles = apt.checkAtHome
                .Where(pair => pair.Value) // 집에 있는 사람만
                .Select(pair => pair.Key)  // Profile만 추출
                .ToList();

        Profile target = null;

        Dialog dialog = null;

        if (atHomeProfiles.Count == 0)
        {
            SoundManager.I.SetEffectAudio(noneCallClip);
            DOVirtual.DelayedCall(3f, () => ResetDial());
            return;
        }
        else if (atHomeProfiles.Count >= apt.checkAtHome.Count)
        {
            target = atHomeProfiles[Random.Range(0, atHomeProfiles.Count)];
            dialog = target.dialogs.Find(d => d.code.Equals("Call_With_Mate"));
        }
        else
        {
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
        SoundManager.I.SetEffectAudio(callClip);
        DOVirtual.DelayedCall(7f, () => InGameUIController.I.ShowTextBox(target, dialog));
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

    public void SetActive(bool on) => gameObject.SetActive(on);
}