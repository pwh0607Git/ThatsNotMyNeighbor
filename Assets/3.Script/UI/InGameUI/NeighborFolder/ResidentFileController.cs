using UnityEngine;
using System.Collections;

public class ResidentFileController : MonoBehaviour
{
    [SerializeField] GameObject popUp;
    [SerializeField] ResidentInfoPanel panelPrefab;

    [SerializeField] FilesTabController filesTab;

    public void InitFileDatas()
    {
        filesTab.InitFileDatas();
    }

    public void OnClickOpenButton()
    {
        popUp.SetActive(true);
    }
    
    public void OnClickExitButton()
    {
        popUp.SetActive(false);
    }
}
