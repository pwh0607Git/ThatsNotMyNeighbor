using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadingController : MonoBehaviour
{
    float elapsedTime;

    void OnEnable()
    {
        elapsedTime = 0f;
        Debug.Log("Loading...");
        StartCoroutine(LoadSceneAsync());
    }

    void Update()
    {
        elapsedTime += Time.deltaTime;
    }

    public float minimumLoadingTime = 1f;

    IEnumerator LoadSceneAsync()
    {
        string nextSceneName = SceneLoader.nextSceneName;

        AsyncOperation operation = SceneManager.LoadSceneAsync(nextSceneName);
        operation.allowSceneActivation = false;

        while (operation.progress < 0.9f)
        {
            yield return null;
        }

        // 최소 로딩 시간 대기
        while (elapsedTime <= minimumLoadingTime)
        {
            yield return null;
        }

        operation.allowSceneActivation = true;
    }
}
