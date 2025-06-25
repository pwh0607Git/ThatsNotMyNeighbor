using UnityEngine;
using UnityEngine.SceneManagement;

public class LobbyManager : MonoBehaviour
{
    [SerializeField] AudioClip lobbyBgmClip;
    OptionPanelController optionPanelController;

    void Start()
    {
        TryGetComponent(out optionPanelController);

        InitPanel();
    }

    void OnEnable()
    {
        SoundManager.I.SetMasterAudio(lobbyBgmClip);
    }

    void InitPanel()
    {
        optionPanelController.SetActivePanel(false);
    }

    public void OnClickOptionButton()
    {
        optionPanelController.SetActivePanel(true);
    }

    public void OnClickGameButton()
    {
        SceneManager.LoadScene("Scn1.InGame");
    }
}
