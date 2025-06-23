using System.Collections.Generic;
using System.ComponentModel;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;

public class InteractionManager : BehaviourSingleton<InteractionManager>
{
    protected override bool IsDontDestroy() => false;

    [SerializeField] GateOpener gateOpener;

    [SerializeField] private ResidentController currentResident;

    public IDCardController idCardController;
    public EntryRequestController entryRequestController;

    public bool isOpen { get; private set; }

    void Start()
    {
        TryGetComponent(out idCardController);
        TryGetComponent(out entryRequestController);

        gateOpener.OnGateUpdated += UpdateGate;

        isOpen = false;
    }

    public void SetCurrentResident(ResidentController resident)
    {
        if (resident is DDDController) return;

        this.currentResident = resident;

        idCardController.SetData(resident.profile);
        entryRequestController.SetData(resident.profile);

        DOVirtual.DelayedCall(0.2f, () =>
        {
            ShowProps();
        });
    }

    private void ShowProps()
    {
        int num = Random.Range(0, 4);

        Debug.Log($"Props Number : {num}");
        switch (num)
        {
            case 0:
                {
                    idCardController.SetActiveButton(true);
                    entryRequestController.SetActiveButton(true);
                    break;
                }
            case 1:
                {
                    idCardController.SetActiveButton(true);
                    break;
                }
            case 2:
                {
                    entryRequestController.SetActiveButton(true);
                    break;
                }
            case 3:
                {
                    idCardController.SetActiveButton(true);
                    entryRequestController.SetActiveButton(true);
                    break;
                }
        }
    }

    public UnityAction<Profile> OnExitResident;

    public void ExitResident()
    {
        if (currentResident == null) return;

        idCardController?.SetActiveButton(false);
        entryRequestController?.SetActiveButton(false);

        Profile profile = currentResident.profile;
        OnExitResident?.Invoke(profile);

        currentResident = null;
    }

    public void UpdateGate(bool on)
    {
        isOpen = on;

        if (currentResident == null) return;

        currentResident.Exit();
        ExitResident();
    }

    public void CleanDoppel()
    {
        currentResident.gameObject.SetActive(false);
        currentResident = null;

        idCardController.SetActiveButton(false);
        entryRequestController.SetActiveButton(false);
    }

    public void StartQuestion(Dictionary<QuestionType, bool> questions)
    {
        // 리스트에 있는 질의들 하기.
        foreach (var q in questions)
        {
            if (!q.Value) continue;

            string dialogCode = GetDialogCode(q.Key);
            currentResident.Talk(dialogCode);
        }
    }

    private string GetDialogCode(QuestionType type)
    {
        if (type.Equals(QuestionType.IDCard))
        {
            if (idCardController.GetActiveButton())
            {
                //이미 제출한 상태
                return "IDCard_Exist";
            }
            else
            {
                idCardController.SetActiveButton(true);
                return "IDCard_Mistake";
            }
        }
        else if (type.Equals(QuestionType.EntryRequest))
        {
            if (entryRequestController.GetActiveButton())
            {
                //이미 제출한 상태
                return "EntryRequest_Exist";
            }
            else
            {
                entryRequestController.SetActiveButton(true);
                return "EntryRequest_Mistake";
            }
        }
        else if (type.Equals(QuestionType.Appearance))
        {
            return "Question_Appearance";
        }
        else if (type.Equals(QuestionType.TodayEntryList))
        {
            if (IsDoppel())
            {
                // 만약 도플갱어라면 확률 적으로 정체가 밝혀진 것을 체크한다.
                float rndVal = Random.Range(0, 1);

                // if (rndVal <= 0.3f)
                // {
                //     currentResident.RevealDoppel();
                //     return "Reveal_TodayEntryList";
                // }
                // else
                // {
                //     return "Question_TodayEntryList_None";
                // }
                return "Question_TodayEntryList_None";
            }
            else
            {
                return "Question_TodayEntryList_None";
            }
        }
        return "";
    }

    private bool IsDoppel() => currentResident.type.Equals(CharacterType.Doppel);
}