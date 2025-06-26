using DG.Tweening;
using UnityEngine.Events;

public class DDDController : ResidentController
{
    public UnityAction OnCompleteBehaviour;

    void OnEnable()
    {
        InGameUIController.I.RegisterTextBoxAction(OnCompletePrintDialogHandler);
    }

    void OnDisable()
    {
        InGameUIController.I.UnregisterTextBoxAction(OnCompletePrintDialogHandler);
    }

    void OnCompletePrintDialogHandler()
    {
        // 텍스트 출력후 퇴장
        Sequence endSeq = DOTween.Sequence();

        endSeq.AppendInterval(0.5f)
            .AppendCallback(() => Exit())
            .AppendInterval(4f)
            .OnComplete(() =>
            {
                OnCompleteBehaviour.Invoke();
            });
    }
}