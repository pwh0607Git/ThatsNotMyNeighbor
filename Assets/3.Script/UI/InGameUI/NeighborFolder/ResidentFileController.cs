using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ResidentFileController : MonoBehaviour
{
    [SerializeField] GameObject popUp;
    [SerializeField] ResidentInfoPanel pan;

    [SerializeField] FilesTabController filesTab;

    public void InitFileDatas()
    {
        StartCoroutine(testco());
    }

    IEnumerator testco()
    {
        yield return new WaitForSeconds(0.5f);
        
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
