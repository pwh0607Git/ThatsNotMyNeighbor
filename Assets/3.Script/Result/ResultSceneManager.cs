using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
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

    void OnEnable()
    {
        SetTMP();
        ShowResult();
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

        showSeq.AppendCallback(() => capturedDoppelCountPan.SetActive(true))
                .AppendInterval(0.5f)
                .AppendCallback(() => enterDoppelCountPan.SetActive(true))
                .AppendInterval(0.5f)
                .AppendCallback(() => deadResidentCountPan.SetActive(true))
                .AppendInterval(1f)
                .AppendCallback(() => rankPan.SetActive(true))
                .AppendInterval(0.3f)
                .AppendCallback(() => SetActiveButton(true));
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
        SceneManager.LoadScene("Scn1.InGame");
    }  
    
    public void OnClickLobbyButton()
    {
        SceneManager.LoadScene("Scn0.Lobby");
    }
}
