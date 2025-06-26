using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

[Serializable]
public class TutorialData
{
    public Image mark;
    public string msg;
}


public class TutorialManager : MonoBehaviour
{
    public UnityAction<ResidentController> OnCompleteTutorial;
    [SerializeField] TutorialData tutorialData;
  
    [SerializeField] Profile profile_DDD;
    [SerializeField] Transform characterLayer;

    private ResidentController ddd;

    [SerializeField] Button skipButton;
    [SerializeField] Button performButton;

    void InitNPC()
    {
        ddd = Instantiate(profile_DDD.model, characterLayer).GetComponent<ResidentController>();
        ddd.SetProperty(profile_DDD, CharacterType.NPC, BehaviourFactory.CreateResidentBehaviour());
    
        Sequence startSeq = DOTween.Sequence();

        startSeq.AppendInterval(1f)
            .AppendCallback(() => InGameUIController.I.MoveShutDownDoor(800f))
            .AppendInterval(1f)
            .AppendCallback(() =>
            {
                // 대사 출력
                ddd.GetComponent<ResidentController>().TalkByCode("Tutorial");
            });
    }

    void StartTutorial()
    {
    }

    void EndTutorial()
    {
        OnCompleteTutorial?.Invoke(ddd);
    }

    void ShowButtons()
    {

    }

    // 튜토리얼 스킵
    public void OnClickSkipButton()
    {
        OnCompleteTutorial?.Invoke(ddd);
    }

    // 튜토리얼 수행
    public void OnPerformButton()
    {
        StartTutorial();
    }
}
