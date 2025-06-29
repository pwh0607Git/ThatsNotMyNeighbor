using UnityEngine;

public class PauseProxy : MonoBehaviour
{
    [SerializeField] GameObject pausePanel;

    void Start()
    {
        pausePanel.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (pausePanel == null) return;

            if (pausePanel.activeSelf)
            {
                pausePanel.SetActive(false);
            }
            else
            {
                pausePanel.SetActive(true);
            }
        }       
    }
}
