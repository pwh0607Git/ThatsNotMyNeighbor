using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;


public enum CharacterType { Resident, Doppel, NPC }

public enum DoppelType
{
    Apearence, ID, EntryRequest, TodayEntryList, AtHome
}

public class ResidentController : MonoBehaviour
{
    public Profile profile { get; private set; }
    public CharacterType type;
    IResidentBehavior behavior;

    RectTransform rectTransform;

    private Animator animator;

    [SerializeField] Transform origin;
    [SerializeField] Transform doppel;

    void InitAppearanceRef()
    {
        origin = transform.Find("_origin");
        doppel = transform.Find("_doppel");
    }

    public void SetProperty(Profile profile, CharacterType type)
    {
        this.profile = profile;

        rectTransform ??= GetComponent<RectTransform>();
        rectTransform.anchoredPosition = profile.startPoint;

        this.type = type;

        if (type == CharacterType.NPC) return;

        behavior = CreateBehaviour(type);

        InitAppearanceRef();
        SetAppearance(type);
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
        // animator.SetTrigger("Talk");
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

    #region Setter
    private void SetAppearance(CharacterType type)
    {
        if (doppel == null || origin == null)
        {
            Debug.Log("외모 세팅 transform 이 없다.");
            return;
        }

        if (type.Equals(CharacterType.Resident))
        {
            doppel.gameObject.SetActive(false);
            origin.gameObject.SetActive(true);

            int ci = Random.Range(0, origin.childCount);

            for (int i = 0; i < origin.childCount; i++)
            {
                if (i == ci)
                    origin.GetChild(i).gameObject.SetActive(true);
                else
                    origin.GetChild(i).gameObject.SetActive(false);
            }
        }
        else if (type.Equals(CharacterType.Doppel))
        {
            origin.gameObject.SetActive(false);
            doppel.gameObject.SetActive(true);

            int ci = Random.Range(0, doppel.childCount);

            for (int i = 0; i < doppel.childCount; i++)
            {
                if (i == ci)
                    doppel.GetChild(i).gameObject.SetActive(true);
                else
                    doppel.GetChild(i).gameObject.SetActive(false);
            }
        }
    }
    #endregion
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