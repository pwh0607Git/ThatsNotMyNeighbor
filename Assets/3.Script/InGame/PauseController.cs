using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseController : MonoBehaviour
{
    public void OnClickRestart()
    {
        string currentScn = SceneManager.GetActiveScene().name;
        LoadingController.LoadScene(currentScn);
    }

    public void OnClickToLobby()
    {
        LoadingController.LoadScene("Scn0.Lobby");
    }
}