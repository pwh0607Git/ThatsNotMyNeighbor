using DG.Tweening;
using UnityEngine;

public enum CharacterType { Resident, Doppel, NPC }

public class ResidentController : MonoBehaviour
{
    public Profile profile { get; private set; }
    public CharacterType type;
    IResidentBehavior behavior;

    RectTransform rectTransform;

    private Animator animator;

    public void SetProperty(Profile profile, CharacterType type)
    {
        animator = GetComponentInChildren<Animator>();
        this.profile = profile;

        rectTransform ??= GetComponent<RectTransform>();
        rectTransform.anchoredPosition = profile.startPoint;

        this.type = type;

        if (type == CharacterType.NPC) return;

        behavior = CreateBehaviour(type);
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
                .AppendCallback(() =>
                {
                    InteractionManager.I.SetCurrentResident(this);
                    if (InteractionManager.I.isOpen)
                    {
                        // 시퀀스 중단하고 Exit 실행
                        enterSeq.Kill(); // 시퀀스 종료
                        Exit();
                    }
                })
                .AppendInterval(0.7f)
                .AppendCallback(() =>
                {
                    Talk("Greeting");
                });
    }

    public void Exit()
    {
        InteractionManager.I.ExitResident();
        rectTransform.DOAnchorPos(profile.endPoint, 3f).OnComplete(() => this.gameObject.SetActive(false));
    }

    public void Talk(string code)
    {
        Debug.Log("Start Talk");

        Dialog dialog = profile.dialogs.Find(c => c.code.Equals(code));

        if (dialog == null) return;

        InGameUIController.I.ShowTextBox(dialog);

        if (animator == null) return;

        animator.SetTrigger("Talk");
    }
    #endregion


    #region Behaviour
    private IResidentBehavior CreateBehaviour(CharacterType type)
    {
        return type switch
        {
            CharacterType.Resident => new ResidentBehaviour(),
            CharacterType.Doppel => new DoppelgangerBehavior(),
            _ => null,
        };
    }
    #endregion

    public void RevealDoppel()
    {
        GameObject revealFace = transform.Find("_RevealFace").gameObject;

        //대사만 출력.
        if (revealFace == null) return;

        revealFace.SetActive(true);
        animator.SetTrigger("Reveal");
    }
}


public interface IResidentBehavior
{
    void Talk(ResidentController resident, string code);
}

public class ResidentBehaviour : IResidentBehavior
{
    public void Talk(ResidentController resident, string code)
    {

    }
}

public class DoppelgangerBehavior : IResidentBehavior
{
    public void Talk(ResidentController resident, string code)  
    {

    }
}