using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ResultSceneManager : MonoBehaviour
{
    [SerializeField] GameObject capturedDoppelCountPan;
    [SerializeField] GameObject enterDoppelCountPan;
    [SerializeField] GameObject deadResidentCountPan;

    [SerializeField] GameObject rankPan;

    [SerializeField] TextMeshProUGUI capturedDoppelCount;
    [SerializeField] TextMeshProUGUI enterDoppelCount;
    [SerializeField] TextMeshProUGUI deadResidentCount;

    [SerializeField] TextMeshProUGUI rank;
    [SerializeField] Image mark;

    [SerializeField] AudioClip potClip;
    [SerializeField] AudioClip stampClip;

    void OnEnable()
    {
        SetTMP();
        ShowResult();
        mark.gameObject.SetActive(false);

        //레코드 저장하기
        if (LevelData.mode.Equals(GameMode.Endless))
        {
            RecordManager.SaveRecord(LevelData.level);
            LevelData.ResetLevel();    
        }
    }

    void SetTMP()
    {
        capturedDoppelCount.text = Record.capturedDoppelCount.ToString();
        enterDoppelCount.text = Record.enterDoppelCount.ToString();
        deadResidentCount.text = Record.deadResidentCount.ToString();

        rank.text = Record.GetRank();

        capturedDoppelCountPan.SetActive(false);
        enterDoppelCountPan.SetActive(false);
        deadResidentCountPan.SetActive(false);
        rankPan.SetActive(false);

        SetActiveButton(false);
    }

    void ShowResult()
    {
        Sequence showSeq = DOTween.Sequence();

        showSeq.AppendInterval(1.5f)
                .AppendCallback(() =>
                {
                    capturedDoppelCountPan.SetActive(true);
                    SoundManager.I.SetEffectAudio(potClip);
                })
                .AppendInterval(0.5f)
                .AppendCallback(() =>
                {
                    enterDoppelCountPan.SetActive(true);
                    SoundManager.I.SetEffectAudio(potClip);
                })
                .AppendInterval(0.5f)
                .AppendCallback(() =>
                {
                    deadResidentCountPan.SetActive(true);
                    SoundManager.I.SetEffectAudio(potClip);
                })
                .AppendInterval(1f)
                .AppendCallback(() =>
                {
                    rankPan.SetActive(true);
                    SoundManager.I.SetEffectAudio(potClip);
                })
                .AppendInterval(1f)
                .AppendCallback(() => StartMarkAnim())
                .AppendCallback(() => SetActiveButton(true));
    }

    void StartMarkAnim()
    {
        mark.gameObject.SetActive(true);
        mark.GetComponent<RectTransform>().localScale = Vector3.one * 2f;
        mark.transform.DOScale(Vector3.one, 1f).SetEase(Ease.InSine).OnComplete(() => SoundManager.I.SetEffectAudio(stampClip));
    }

    [SerializeField] Button btn_Reload;
    [SerializeField] Button btn_ToLobby;

    public void SetActiveButton(bool on)
    {
        btn_Reload.enabled = on;
        btn_ToLobby.enabled = on;
    }

    public void OnClickReloadButton()
    {
        Record.Reset();
        LoadingController.LoadScene("Scn1.InGame");
    }

    public void OnClickLobbyButton()
    {
        Record.Reset();
        LoadingController.LoadScene("Scn0.Lobby");
    }
}
