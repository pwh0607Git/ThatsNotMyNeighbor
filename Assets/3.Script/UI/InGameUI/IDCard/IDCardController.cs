using UnityEngine;
using UnityEngine.UI;

public class IDCardController : MonoBehaviour
{
    [SerializeField] Button idCardButton;

    [SerializeField] IDCardView view;

    IDCardPresenter presenter;


    void Start()
    {
        idCardButton.gameObject.SetActive(false);               // 처음에는 꺼두기
        view.gameObject.SetActive(false);

        presenter = new(view);
    }

    public void SetData(Profile profile)
    {
        presenter.SetIDCardModel(profile);
    }

    public void SetActiveButton(bool on)
    {
        idCardButton.gameObject.SetActive(on);
    }

    public bool GetActiveButton()
    {
        return idCardButton.gameObject.activeSelf;
    }

    public void OnClickOpenButton() => presenter.OnClickOpenButton();
    public void OnClickExitButton() => presenter.OnClickCloseButton();
}