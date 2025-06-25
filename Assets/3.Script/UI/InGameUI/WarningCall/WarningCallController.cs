using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;

public class WarningCallController : MonoBehaviour
{
    [Header("Siren")]
    [SerializeField] GameObject siren;
    public ResidentController ddd { get; private set; }

    public UnityAction OnCompleteCleaning;

    Sequence cleaningSeq;

    public void Init(ResidentController npc)
    {
        this.ddd = npc;
    }

    void StartCleaningProtocol()
    {
        Debug.Log("Start Cleaning.!!");

        cleaningSeq = DOTween.Sequence();

        cleaningSeq.AppendInterval(4f)
                    .AppendCallback(() =>
                    {
                        ddd.gameObject.SetActive(true);
                        ddd.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
                        
                        SirenActive(false);

                        InteractionManager.I.CleanDoppel();
                        InGameUIController.I.MoveShutDownDoor(800f);
                    })
                    .AppendInterval(2f)
                    .AppendCallback(() =>
                    {
                        ddd.TalkByCode("Call");
                    });
    }

    public void OnClickDangerButton()
    {
        SirenActive(true);

        //shutdowndoor 닫기
        InGameUIController.I.MoveShutDownDoor(0f);
        Debug.Log("Danger Button Click!!");

        StartCleaningProtocol();
    }

    private void SirenActive(bool on)
    {
        siren.transform.GetChild(0).gameObject.SetActive(on);
    }
}