using UnityEngine;

public class OptionPanelController : MonoBehaviour
{
    [SerializeField] OptionPanel panel;

    private float prevVolumeMaster = 0.5f;
    private float prevVolumeEffect = 0.5f;

    public void SetActivePanel(bool on)
    {
        panel.gameObject.SetActive(on);
    }

    void OnEnable()
    {
        panel.OnChangedMasterVolume += MasterVolumeChangedHandler;
        panel.OnChangedEffectVolume += EffectVolumeChangedHandler;
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
        SetActivePanel(false);
    }

    public void OnClickCompleteButton()
    {
        // 새로운 값을 SoundManager에게 전달하기
        SoundManager.I.SetVolume(panel.amountMaster, panel.amountEffect);
        SetPrevVolume(panel.amountMaster, panel.amountEffect);
        SetActivePanel(false);
    }

    private void SetPrevVolume(float prevVolumeMaster, float prevVolumeEffect)
    {
        this.prevVolumeMaster = prevVolumeMaster;
        this.prevVolumeEffect = prevVolumeEffect;
    }

    private void ResetVolume()
    {
        SoundManager.I.SetVolume(prevVolumeMaster, prevVolumeEffect);       
        panel.SetSlideBar(prevVolumeMaster, prevVolumeEffect);
    }
}