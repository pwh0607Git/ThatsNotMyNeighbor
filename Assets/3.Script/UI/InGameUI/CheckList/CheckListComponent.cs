using TMPro;
using UnityEditor.PackageManager.Requests;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class CheckListComponent : MonoBehaviour
{
    QuestionType groupId;

    [SerializeField] TextMeshProUGUI c_Name;

    [SerializeField] Toggle toggle;

    public UnityAction<QuestionType, bool> OnUpdateToggle;

    void Start()
    {
        c_Name = GetComponentInChildren<TextMeshProUGUI>();
    }

    public void OnClickCheck()
    {
        OnUpdateToggle?.Invoke(groupId, toggle.isOn);
    }

    public void InitCheckComponent(QuestionType type)
    {
        groupId = type;
        c_Name.text = type.ToString();

        ResetToggle();
    }

    public void ResetToggle()
    {
        toggle.isOn = false;
    }

    void OnEnable()
    {
        ResetToggle();
    }
}