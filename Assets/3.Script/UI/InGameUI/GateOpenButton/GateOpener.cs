using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class GateOpener : MonoBehaviour
{
    [SerializeField] Toggle toggle_Open;
    [SerializeField] Toggle toggle_Close;
    [SerializeField] AudioClip openClip;
    [SerializeField] AudioClip closeClip;

    public UnityAction<bool> OnGateUpdated;

    void Start()
    {
        toggle_Open.isOn = false;
        toggle_Close.isOn = true;
    }

    public void OnClickOpenToggle()
    {
        SoundManager.I.SetEffectAudio(openClip);
        OnGateUpdated?.Invoke(true);
    }

    public void OnClickCloseToggle()
    {
        SoundManager.I.SetEffectAudio(closeClip);
        OnGateUpdated?.Invoke(false);
    }
}