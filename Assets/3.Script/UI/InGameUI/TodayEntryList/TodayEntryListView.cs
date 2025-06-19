using UnityEngine;

public class TodayEntryListView : MonoBehaviour
{
    [SerializeField] Transform parent;
    [SerializeField] TodayEntryComponent prefab;

    public void Clear()
    {
        foreach (Transform c in parent)
        {
            Destroy(c.gameObject);
        }
    }

    // 여기서의 Entry는 항목을 의미한다.
    public void CreateEntry(Profile profile, string address)
    {
        TodayEntryComponent comp = Instantiate(prefab, parent);
        comp.InitComponent(profile.profileImage, profile.firstName, profile.lastName, address);
    }

    public void SetActive(bool on) => gameObject.SetActive(on);
}