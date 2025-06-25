using System.Collections;
using System.Collections.Generic;
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
        LoadScene("Scn1.InGame");
    }

    public void OnClickEndlessModeButton()
    {
        LoadScene("Scn1.InGame_EndlessMode");
    }

    private void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
}
