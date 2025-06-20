using TMPro;
using UnityEditor.PackageManager.Requests;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class CheckGroup : MonoBehaviour
{
    string groupId;

    [SerializeField] TextMeshProUGUI c_Name;

    [SerializeField] Toggle toggle_True;
    [SerializeField] Toggle toggle_False;

    public UnityAction<string, bool> OnUpdateToggle;

    void Start()
    {
        c_Name = GetComponentInChildren<TextMeshProUGUI>();
    }

    public void OnClickCheck()
    {
        if (toggle_True.isOn)
        {
            OnUpdateToggle?.Invoke(groupId, true);
        }
        else
        {
            OnUpdateToggle?.Invoke(groupId, false);
        }
    }

    public void InitCheckComponent(string id)
    {
        groupId = id;
        c_Name.text = id;

        ResetToggle();
    }

    public void ResetToggle()
    {
        toggle_True.isOn = true;
        toggle_False.isOn = false;
    }

    void OnEnable()
    {
        ResetToggle();
    }
}