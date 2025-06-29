using UnityEngine;
using UnityEngine.SceneManagement;

public class GameModePanelController : MonoBehaviour
{
    [SerializeField] GameObject panel;

    public void OnClickOpenButton()
    {
        panel.SetActive(true);
    }

    public void OnClickCloseButton()
    {
        panel.SetActive(false);
    }

    public void OnClickArcadeModeButton()
    {
        LoadingController.LoadScene("Scn1.InGame");
    }

    public void OnClickEndlessModeButton()
    {
        LevelData.mode = GameMode.Endless;
        LoadingController.LoadScene("Scn1.InGame_Endless");
    }
    private void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
}
