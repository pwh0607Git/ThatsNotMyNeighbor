public class ResidentFolderPresenter
{
    private readonly ResidentFolderView view;
    private readonly ResidentFolderModel model;

    public ResidentFolderPresenter(ResidentFolderView view)
    {
        this.view = view;
        model = new();

        view.SetFileIndex();

        InGameUIController.I.RegisterInitEvent(Init);
    }

    public void Init()
    {
        UnityEngine.Debug.Log($"Dic : {InGameManager.I.addressDic.Count}");
        model.SetApartments(InGameManager.I.addressDic);
     
        var dic = model.Apartments;
        
        foreach (var a in dic)
        {
            if (a.Value.residents == null || a.Value.residents.Count <= 0) continue;

            string address = a.Key;

            foreach (var r in a.Value.residents)
            {
                view.CreateEntry(r, address);
            }
        }

        //View 인덱스와 상태를 초기화
        view.Init();
    }

    public void OnClickOpenButton() => view.SetActive(true);
    public void OnClickCloseButton() => view.SetActive(false);
}