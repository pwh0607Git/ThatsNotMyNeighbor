using UnityEngine;
using UnityEngine.EventSystems;

public class TelephoneDialController : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    public int selectedNumber;
    public string currentNumber;

    [SerializeField] DialNumberButton[] buttons;

    [SerializeField] private GameObject Roulette;

    [SerializeField] float[] maxRotateAngles;
    [SerializeField] Vector2 dialCenter;

    bool isRotate = false;              //룰렛 회전 여부
    bool canRotate = true;

    void Start()
    {
        dialCenter = new Vector2(0f, 0f);

        // InitAction();
    }

    void LateUpdate()
    {
        if (isDragging)
        {
            LotateRoulette();
        }
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

    }

    void LotateRoulette()
    {
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direction = mousePos - dialCenter;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
    }

    public void OnClickDialExit()
    {
        selectedNumber = 0;
        this.gameObject.SetActive(false);
    }

    private bool isDragging = false;

    public void OnBeginDrag(PointerEventData eventData)
    {
        isDragging = true;
        Debug.Log("다이얼 드래그 시작!");
    }
    public void OnDrag(PointerEventData eventData)
    {
        Debug.Log("드래그 중...");
        // 회전 처리 가능
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        isDragging = false;
        Debug.Log("다이얼 드래그 종료!");
    }
}