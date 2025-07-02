using UnityEngine;

public class OptionPanelController : MonoBehaviour
{
    [SerializeField] OptionPanel view;

    private float prevVolumeMaster = 0.5f;
    private float prevVolumeEffect = 0.5f;

    public void SetActiveView(bool on)
    {
        view.gameObject.SetActive(on);
    }

    void OnEnable()
    {
        view.OnChangedMasterVolume += MasterVolumeChangedHandler;
        view.OnChangedEffectVolume += EffectVolumeChangedHandler;
    }

    void MasterVolumeChangedHandler(float value)
    {
        SoundManager.I.SetVolume(value, prevVolumeEffect);
    }

    void EffectVolumeChangedHandler(float value)
    {
        SoundManager.I.SetVolume(prevVolumeMaster, value);       
    }

    public void OnClickExitButton()
    {
        ResetVolume();
        SetActiveView(false);
    }

    public void OnClickCompleteButton()
    {
        // 새로운 값을 SoundManager에게 전달하기
        SoundManager.I.SetVolume(view.amountMaster, view.amountEffect);
        SetPrevVolume(view.amountMaster, view.amountEffect);
        SetActiveView(false);
    }

    private void SetPrevVolume(float prevVolumeMaster, float prevVolumeEffect)
    {
        this.prevVolumeMaster = prevVolumeMaster;
        this.prevVolumeEffect = prevVolumeEffect;
    }

    private void ResetVolume()
    {
        SoundManager.I.SetVolume(prevVolumeMaster, prevVolumeEffect);       
        view.SetSlideBar(prevVolumeMaster, prevVolumeEffect);
    }
}