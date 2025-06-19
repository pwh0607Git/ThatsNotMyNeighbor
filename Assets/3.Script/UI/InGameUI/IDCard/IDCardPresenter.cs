public class IDCardPresenter
{
    private readonly IDCardView view;
    private readonly IDCardModel model;

    public IDCardPresenter(IDCardView view)
    {
        this.view = view;
        model = new();
    }

    public void SetIDCardModel(Profile profile, bool isDoppel = false)
    {
        model.SetProfile(profile, isDoppel);

        //view 갱신하기
        if (!view.gameObject.activeSelf)
        {
            UnityEngine.Debug.Log("Id 카드 뷰가 비활성화 되어있습니다.");
        }
        view.SetIDCard(profile);
    }
            
    public void OnClickOpenButton() => view.SetActive(true);
    public void OnClickCloseButton() => view.SetActive(false);
}