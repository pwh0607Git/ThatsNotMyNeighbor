using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class ResidentController : MonoBehaviour
{
    public Profile profile { get; private set; }

    RectTransform rectTransform;

    private Animator animator;

    void Start()
    {
        TryGetComponent(out rectTransform);
        TryGetComponent(out animator);
    }

    public void SetProfile(Profile profile)
    {
        this.profile = profile;
        
        if (rectTransform == null)
            TryGetComponent(out rectTransform);

        rectTransform.anchoredPosition = profile.startPoint;
    }

    void OnEnable()
    {
        if (profile == null) return;

        Enter();
    }

    void OnDisable()
    {

    }

    #region Move
    [SerializeField] Ease moveEase;
    public void Enter()
    {
        Sequence enterSeq = DOTween.Sequence();

        enterSeq.Append(rectTransform.DOAnchorPos(profile.targetPoint, 4f)).SetEase(moveEase)
                .AppendCallback(() => InteractionManager.I.SetCurrentResident(this))
                .AppendInterval(0.7f)
                .AppendCallback(() =>
                {
                    Talk("Greeting");
                });
    }

    public void Exit()
    {
        // InteractionManager.I.ExitResident();    
        rectTransform.DOAnchorPos(profile.endPoint, 3f).OnComplete(() => this.gameObject.SetActive(false));
    }

    public void Talk(string code)
    {
        Debug.Log("Start Talk");

        Dialog dialog = profile.dialogs.Find(c => c.code.Equals(code));

        if (dialog == null) return;
        
        InGameUIController.I.ShowTextBox(dialog);
        // animator.SetTrigger("Talk");
    }
    
    #endregion
}