using UnityEngine;

public class RecordPanelController : MonoBehaviour
{
    [SerializeField] GameObject view;

    public void SetActiveView(bool on)
    {
        view.SetActive(on);
    }

    public void OnClickCloseButton()
    {
        SetActiveView(false);
    }
}