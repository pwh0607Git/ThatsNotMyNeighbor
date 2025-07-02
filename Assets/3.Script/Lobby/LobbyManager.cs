using UnityEngine;

public class LobbyManager : MonoBehaviour
{
    [SerializeField] AudioClip lobbyBgmClip;
    OptionPanelController optionPanelController;
    RecordPanelController recordPanelController;

    void Start()
    {
        TryGetComponent(out optionPanelController);
        TryGetComponent(out recordPanelController);

        InitPanel();
    }

    void OnEnable()
    {
        SoundManager.I.SetMasterAudio(lobbyBgmClip);
    }

    void InitPanel()
    {
        optionPanelController.SetActiveView(false);
        recordPanelController.SetActiveView(false);
    }

    public void OnClickOptionButton()
    {
        optionPanelController.SetActiveView(true);
    }

    public void OnClickRecrodButton()
    {
        recordPanelController.SetActiveView(true);
    }

    public void OnClickExitButton()
    {

    }
}