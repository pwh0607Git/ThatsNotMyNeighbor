using UnityEngine;
using UnityEngine.UI;

public class EntryRequestController : MonoBehaviour
{

    [SerializeField] Button entryRequestButton;

    [SerializeField] EntryRequestView view;

    EntryRequestPresenter presenter;

    void Start()
    {
        entryRequestButton.gameObject.SetActive(false);               // 처음에는 꺼두기
        view.gameObject.SetActive(false);

        presenter = new(view);
    }

    public void SetData(Profile profile)
    {
        string address = InGameManager.I.SearchAddress(profile);   
        presenter.SetEntryRequestModel(profile, address, false);
    }

    public void SetForgedData(Profile profile)
    {
        string address = InGameManager.I.SearchAddress(profile); 
        presenter.SetEntryRequestModel(profile, address, true);
    }

    public void SetActiveButton(bool on)
    {
        entryRequestButton.gameObject.SetActive(on);
    }

    public bool GetActiveButton()
    {
        return entryRequestButton.gameObject.activeSelf;
    }



    public void OnClickOpenButton() => presenter.OnClickOpenButton();
    public void OnClickExitButton() => presenter.OnClickCloseButton();
}
