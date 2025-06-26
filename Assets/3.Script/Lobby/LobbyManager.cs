using UnityEngine;
using UnityEngine.SceneManagement;

public static class SceneLoader
{
    public static string nextSceneName;
    public static void LoadLoadingScene()
    {
        SceneManager.LoadScene("Scn0.Loading");
    }
}
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

    public void OnClickArcadeModeButton()
    {
        SceneLoader.nextSceneName = "Scn1.InGame";
        SceneLoader.LoadLoadingScene();
    }

    public void OnClickEndlessModeButton()
    {
        SceneLoader.nextSceneName = "Scn1.InGame_Endless";
        SceneLoader.LoadLoadingScene();
    }
}
