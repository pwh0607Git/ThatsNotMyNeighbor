using System.Collections.Generic;
using System.Data.Common;
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

        Debug.Log($"current Resident Set : {resident.profile}");

        this.currentResident = resident;

        if (resident is not DoppelController doppel)
        {
            Debug.Log("현재 주민은 도플 갱어가 아닙니다.");
            idCardController.SetData(resident.profile);
            entryRequestController.SetData(resident.profile);
        }
        else
        {
            
            Debug.Log("현재 주민은 도플 갱어 입니다.");
            // 위조 데이터 생성.
            if (doppel.doppelType.Equals(DoppelType.ForgedID))
            {
                idCardController.SetForgedData(resident.profile);
                entryRequestController.SetData(resident.profile);
            }
            else if (doppel.doppelType.Equals(DoppelType.ForgedEntryRequest))
            {
                idCardController.SetData(resident.profile);
                entryRequestController.SetForgedData(resident.profile);
            }
            else
            {
                idCardController.SetData(resident.profile);
                entryRequestController.SetData(resident.profile);
            }
        }


        DOVirtual.DelayedCall(0.2f, () =>
        {
            ShowProps();
        });
    }

    private void ShowProps()
    {
        int num = Random.Range(0, 4);
        
        bool hasIDCard = currentResident.HasIDCard();
        bool hasEntryRequest = currentResident.HasEntryRequest();

        Debug.Log($"Props Number : {num}");
        switch (num)
        {
            case 0:
                {
                    if(hasIDCard) idCardController.SetActiveButton(true);
                    entryRequestController.SetActiveButton(true);
                    break;
                }
            case 1:
                {
                    if(hasIDCard) idCardController.SetActiveButton(true);
                    break;
                }
            case 2:
                {
                    if(hasEntryRequest) entryRequestController.SetActiveButton(true);
                    break;
                }
            case 3:
                {
                    if(hasIDCard) idCardController.SetActiveButton(true);
                    if(hasEntryRequest) entryRequestController.SetActiveButton(true);
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
        LogManager.I.WriteLog(currentResident.type, DespawnType.Death);

        if (currentResident is DoppelController doppel && doppel.doppelType.Equals(DoppelType.Appearance))
        {
            //도플 갱어를 정상적으로 잡았다면...
            Debug.Log("도플갱어 기록 성공.");

            if (Log.IsExisting(doppel.information)) return;
            Log.CaptureDoppel(doppel.information, doppel.profile);
        }

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

            if (dialogCode == "") return;

            currentResident.TalkByCode(dialogCode);
        }
    }

    private string GetDialogCode(QuestionType type)
    {
        bool hasIDCard = currentResident.HasIDCard();
        bool hasEntryRequest = currentResident.HasEntryRequest();

        if (type.Equals(QuestionType.IDCard))
        {
            if (idCardController.GetActiveButton())
            {
                //이미 제출한 상태
                return "IDCard_Exist";
            }
            else
            {
                if (hasIDCard)
                {
                    idCardController.SetActiveButton(true);
                    return "IDCard_Mistake";
                }
                else
                {
                    if (currentResident is DoppelController doppel && doppel.doppelType.Equals(DoppelType.NonePaper))
                    {
                        doppel.Reveal("Reveal_Props");
                        return "";
                    }
                }
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
                if (hasEntryRequest)
                {
                    entryRequestController.SetActiveButton(true);
                    return "EntryRequest_Mistake";
                }
                else
                {
                    if (currentResident is DoppelController doppel && doppel.doppelType.Equals(DoppelType.NonePaper))
                    {
                        doppel.Reveal("Reveal_Props");
                        return "";
                    }
                }
            }
        }
        else if (type.Equals(QuestionType.Appearance))
        {
            if (currentResident is DoppelController doppel && doppel.doppelType.Equals(DoppelType.Appearance))
            {
                //걸렸을 때. => 20프로 확률로 Reveal 호출.
                float per = Random.value;

                if (per <= 0.5f)
                {
                    doppel.Reveal("Reveal_Appearance");
                    return "";
                }
                else
                {
                    return "Question_Appearance";
                }
            }
            return "Question_Appearance";
        }
        else if (type.Equals(QuestionType.TodayEntryList))
        {
            
            Debug.Log("Question Start : TodayEntryList");
            if (currentResident is DoppelController doppel && doppel.doppelType.Equals(DoppelType.TodayEntryList))
            {
                float per = Random.value;
                if (per >= 0.2f)
                {
                    Debug.Log("Question Start : TodayEntryList");
                    doppel.Reveal("Reveal_TodayEntryList");
                    return "";
                }
                else
                {
                    return "Question_TodayEntryList_None";
                }
            }
            else
            {
                //주민인 경우.
                return "Question_TodayEntryList_None";
            }
        }
        return "";
    }

    private bool IsDoppel() => currentResident.type.Equals(CharacterType.Doppel);
}