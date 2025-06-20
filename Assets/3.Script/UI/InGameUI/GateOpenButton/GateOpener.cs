using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class GateOpener : MonoBehaviour
{
    [SerializeField] Toggle toggle_Open;
    [SerializeField] Toggle toggle_Close;

    public UnityAction<bool> OnGateUpdated;

    void Start()
    {
        toggle_Open.isOn = false;
        toggle_Close.isOn = true;
    }

    public void OnClickOpenToggle()
    {
        OnGateUpdated?.Invoke(true);
    }

    public void OnClickCloseToggle()
    {
        OnGateUpdated?.Invoke(false);
    }
}