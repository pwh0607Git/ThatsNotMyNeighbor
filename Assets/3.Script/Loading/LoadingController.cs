using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadingController : MonoBehaviour
{
    private static string nextScene;
    [SerializeField] float loadingTime;

    float elapsed = 0f; 
    void Start()
    {
        StartCoroutine(LoadSceneAsync());
    }

    public static void LoadScene(string sceneName)
    {
        Debug.Log("로딩씬으로 이동...");
        nextScene = sceneName;
        SceneManager.LoadScene("Scn0.Loading");
    }

    IEnumerator LoadSceneAsync()
    {
        yield return null;

        Debug.Log("Loading Scene...");
        AsyncOperation operation = SceneManager.LoadSceneAsync(nextScene);
        operation.allowSceneActivation = false;

        yield return new WaitForSeconds(2f);

        operation.allowSceneActivation = true;

        elapsed = 0f; 
    }
}
