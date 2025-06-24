using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public enum CharacterType { Resident, Doppel, NPC }

public class ResidentController : MonoBehaviour
{
    private Queue<string> textQueue = new();
    public Profile profile { get; private set; }
    public CharacterType type;
    protected ICharacterBehaviour behavior;

    RectTransform rectTransform;

    public Animator animator;

    public void SetProperty(Profile profile, CharacterType type, ICharacterBehaviour behavior)
    {
        this.profile = profile;
        this.type = type;
        this.behavior = behavior;

        animator = GetComponentInChildren<Animator>();

        rectTransform ??= GetComponent<RectTransform>();
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
    public void Enter()
    {
        Sequence enterSeq = DOTween.Sequence();

        enterSeq.Append(rectTransform.DOAnchorPos(profile.targetPoint, 4f)).SetEase(Ease.Linear)
                .AppendCallback(() =>
                {
                    InteractionManager.I.SetCurrentResident(this);
                    if (InteractionManager.I.isOpen)
                    {
                        enterSeq.Kill();
                        Exit();
                    }
                })
                .AppendInterval(0.7f)
                .AppendCallback(() =>
                {
                    TalkByCode("Greeting");
                });
    }

    public void Exit()
    {
        InteractionManager.I.ExitResident();
        rectTransform.DOAnchorPos(profile.endPoint, 3f).OnComplete(() => this.gameObject.SetActive(false));
    }

    public void TalkByCode(string code)
    {
        Debug.Log("Greeting=>=>=>");
        if (behavior == null)
        {
            Debug.Log("behaviour is null..");
            return;
        }
        Dialog dialog = behavior.GetDialog(this, code);
        behavior.Talk(this, dialog);
    }
    #endregion

    public bool HasIDCard() => behavior?.HasIDCard() ?? false;
    public bool HasEntryRequest() => behavior?.HasEntryRequest() ?? false;
}