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
    }

    void SetTMP()
    {
        capturedDoppelCount.text = Log.capturedDoppelCount.ToString();
        enterDoppelCount.text = Log.enterDoppelCount.ToString();
        deadResidentCount.text = Log.deadResidentCount.ToString();

        rank.text = Log.GetRank();

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
        LogManager.I.ResetLog();
        LoadingController.LoadScene("Scn1.InGame");
    }

    public void OnClickLobbyButton()
    {
        LogManager.I.ResetLog();
        LoadingController.LoadScene("Scn0.Lobby");
    }
}
