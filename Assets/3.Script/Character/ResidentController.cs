using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public enum CharacterType { Resident, Doppel, NPC }
public enum DespawnType
{
    Enter,
    Death,
}

public class ResidentController : MonoBehaviour
{
    public Profile profile;
    public CharacterType type;
    public DespawnType despawnType = DespawnType.Enter;
    protected ICharacterBehaviour behavior;

    private AudioSource audioSource;

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

    public void SetDespawnType(DespawnType type)
    {
        Debug.Log("DespawnType Change");
        this.despawnType = type;
    }

    #region Move
    public void Enter()
    {
        animator.SetFloat("Movement", 1f);

        DOVirtual.DelayedCall(0.7f, () => SoundManager.I.SetCharacterAudio(profile.walkClip));

        Sequence enterSeq = DOTween.Sequence();

        enterSeq.Append(rectTransform.DOAnchorPos(profile.targetPoint, 3f)).SetEase(Ease.Linear)
                .AppendCallback(() =>
                {
                    InteractionManager.I.SetCurrentResident(this);
                    if (InteractionManager.I.isOpen)
                    {
                        enterSeq.Kill();
                        Exit();
                    }
                    animator.SetFloat("Movement", 0f);
                })
                .AppendInterval(0.7f)
                .AppendCallback(() =>
                {
                    TalkByCode("Greeting");
                });
    }

    public void Exit()
    {
        Debug.Log($"Resident Exit : {gameObject.name}");
        SoundManager.I.SetCharacterAudio(profile.walkClip);
        animator.SetFloat("Movement", 1);
        
        LevelManager.I.WriteLog(type, DespawnType.Enter);
        InteractionManager.I.ExitResident();

        rectTransform.DOAnchorPos(profile.endPoint, 3f).OnComplete(() => {
            this.gameObject.SetActive(false);
            });
    }

    public virtual void TalkByCode(string code)
    {
        Debug.Log($"Talk : {code}");

        if (behavior == null) return;

        Dialog dialog = behavior.GetDialog(this, code);
        behavior.Talk(this, dialog);
    }
    #endregion

    public bool HasIDCard() => behavior?.HasIDCard() ?? false;
    public bool HasEntryRequest() => behavior?.HasEntryRequest() ?? false;
}