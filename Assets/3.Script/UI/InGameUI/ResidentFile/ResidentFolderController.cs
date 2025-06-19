using UnityEngine;

public class ResidentFolderController : MonoBehaviour
{
    [SerializeField] ResidentFolderView view;
    ResidentFolderPresenter presenter;

    void Awake()
    {
        presenter = new(view);
    }

    public void OnClickOpenButton() => presenter.OnClickOpenButton();
    public void OnClickExitButton() => presenter.OnClickCloseButton();
}
