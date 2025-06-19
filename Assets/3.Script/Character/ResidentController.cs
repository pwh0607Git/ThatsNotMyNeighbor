using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class ResidentController : MonoBehaviour
{
    public Profile profile { get; private set; }

    RectTransform rectTransform;

    private Animator animator;

    public void SetProfile(Profile profile)
    {
        this.profile = profile;
    }

    void OnEnable()
    {
        if (rectTransform == null) rectTransform = GetComponent<RectTransform>();

        // rectTransform.anchoredPosition = startPoint;
        TryGetComponent(out animator);
    }

    #region Move
    public void Enter()
    {
        rectTransform.GetComponent<RectTransform>().position = profile.startPoint;
        rectTransform.DOAnchorPos(profile.targetPoint, 3f);
    }

    public void Exit()
    {
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