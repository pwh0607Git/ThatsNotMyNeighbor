using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;

public class WarningCallController : MonoBehaviour
{
    [Header("Siren")]
    [SerializeField] GameObject siren;
    public ResidentController ddd { get; private set; }
    [SerializeField] GameObject sirenBack;

    public UnityAction OnCompleteCleaning;

    Sequence cleaningSeq;

    public void Init(ResidentController npc)
    {
        this.ddd = npc;
    }

    public void SetActiveSireBack(bool on)
    {
        sirenBack.SetActive(on);
    }

    void OnEnable()
    {
        SetActiveSireBack(false);
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
                        SetActiveSireBack(false);

                        InGameUIController.I.MoveShutDownDoor(800f);
                    })
                    .AppendInterval(3f)
                    .AppendCallback(() =>
                    {
                        ddd.TalkByCode("CleanProtocol");
                    });
    }
    [SerializeField] AudioClip sirenClip;
    public void OnClickDangerButton()
    {
        SirenActive(true);

        //shutdowndoor 닫기
        InGameUIController.I.MoveShutDownDoor(0f);
        Debug.Log("Danger Button Click!!");
        SoundManager.I.SetEffectAudio(sirenClip);
        StartCleaningProtocol();

        SetActiveSireBack(true);
    }

    private void SirenActive(bool on)
    {
        siren.transform.GetChild(0).gameObject.SetActive(on);
    }
}