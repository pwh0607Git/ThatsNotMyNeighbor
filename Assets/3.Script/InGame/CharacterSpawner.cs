using System;
using System.Collections.Generic;
using System.Linq;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Events;

[Serializable]
public class Resident
{
    public Profile profile;
    bool isDoppel;

    public Resident(Profile profile, bool isDoppel)
    {
        this.profile = profile;
        this.isDoppel = isDoppel;
    }
}

public class CharacterSpawner
{
    private Queue<ResidentController> residentQueue;
    Transform characterLayer;

    // count수만큼 캐릭터 프리팹을 생성한다.
    List<ResidentController> characterList;

    public UnityAction<List<ResidentController>> OnCompleteSpawn;
    public UnityAction OnEmptyCharacterQueue;

    public CharacterSpawner(Transform characterLayer) {
        this.characterLayer = characterLayer;        
    }

    public void SetCharacters(List<Profile> todayEntryList, int count)
    {
        ResetCharacterList();
     
        characterList = new();
        residentQueue = new();

        //일일 출입 리스트 캐릭터는 필수로 출력
        CreateTodayEntryListCharacters(todayEntryList);

        //Today 리스트에 있는 캐릭터는 필수로 생성하기.
        CreateOtherCharacters(count - todayEntryList.Count);

        OnCompleteSpawn?.Invoke(characterList);

        SetQueue();
    }

    void ResetCharacterList()
    {
        if (characterList == null || characterList.Count <= 0) return;

        foreach (var character in characterList)
        {
            GameObject.Destroy(character);
        }

        characterList.Clear();
    }

    void CreateTodayEntryListCharacters(List<Profile> characters)
    {
        foreach (var profile in characters)
        {
            CreateResident(profile);
        }
    }

    void CreateOtherCharacters(int count)
    {
        var characters = InGameManager.I.Characters;

        for (int i = 0; i < count; i++)
        {
            int index = UnityEngine.Random.Range(0, characters.Count);

            float rnd = UnityEngine.Random.value;
            CharacterType type = rnd < 0.2f ? CharacterType.Resident : CharacterType.Doppel;

            Profile profile = characters[index];

            ResidentController character = null;

            if (type.Equals(CharacterType.Resident))
            {
                do
                {
                    index = UnityEngine.Random.Range(0, characters.Count);
                    profile = characters[index];
                } while (characterList.Find(p => p.profile.Equals(profile)));

                character = GameObject.Instantiate(profile.model, characterLayer).GetComponent<ResidentController>();
                character.SetProperty(profile, CharacterType.Resident, BehaviourFactory.CreateResidentBehaviour());
            }
            else if (type.Equals(CharacterType.Doppel))
            {
                character = CreateDoppelganger(profile);
            }

            if (character == null)
            {
                Debug.LogWarning("character is null");
                return;
            }

            characterList.Add(character);
            character.gameObject.SetActive(false);
        }
    }

    void CreateResident(Profile profile)
    {
        ResidentController character = GameObject.Instantiate(profile.model, characterLayer).GetComponent<ResidentController>();
        ICharacterBehaviour behaviour = BehaviourFactory.CreateResidentBehaviour();

        characterList.Add(character);
        character.SetProperty(profile, CharacterType.Resident, behaviour);

        character.gameObject.SetActive(false);
    }

    DoppelController CreateDoppelganger(Profile profile)
    {
        // 1. 타입 결정
        DoppelType doppelType = GetRandomDoppelTypeExceptNone();
        DoppelApearanceType appearanceType = DoppelApearanceType.None;

        DoppelController doppel = null;

        DoppelInform doppelInfo = null;

        // 2. info에서 외모 이상한 모델 추출하기
        if (doppelType.Equals(DoppelType.Appearance))
        {
            var filtered = profile.doppelData.models
            .Where(info => info.type != DoppelApearanceType.None)
            .ToList();

            int rndI = UnityEngine.Random.Range(0, filtered.Count);

            doppelInfo = filtered[rndI];                                                          //DoppelInfo
            doppel = GameObject.Instantiate(doppelInfo.model, characterLayer).GetComponent<DoppelController>();
            appearanceType = doppelInfo.type;
        }
        else
        {
            //외모 문제 none 추출하기
            doppelInfo = profile.doppelData.models.Find(m => m.type.Equals(DoppelApearanceType.None));

            if (doppelInfo == null)
            {
                Debug.LogError("doppel Info is null...");
                return null;
            }

            // doppelType
            doppel = GameObject.Instantiate(doppelInfo.model, characterLayer).GetComponent<DoppelController>();
            appearanceType = DoppelApearanceType.None;
        }

        doppel.SetProperty(profile, CharacterType.Doppel, BehaviourFactory.CreateDoppelBehaviour(doppelType, appearanceType));
        doppel.SetType(appearanceType, doppelType, doppelInfo);              //외모 문제는 있지만 타입이 없음.

        return doppel;
    }

    //Utility
    DoppelType GetRandomDoppelTypeExceptNone()
    {
        var values = Enum.GetValues(typeof(DoppelType))
                         .Cast<DoppelType>()
                         .Where(t => t != DoppelType.None)
                         .ToList();

        return values[UnityEngine.Random.Range(0, values.Count)];
    }

    void SetQueue()
    {
        var tempList = characterList.OrderBy(obj => UnityEngine.Random.value).ToList();

        foreach (var o in tempList)
        {
            residentQueue.Enqueue(o);
        }
    }

    public void SpawnCharacter()
    {
        if (residentQueue.Count <= 0)
        {
            OnEmptyCharacterQueue?.Invoke();
        }

        ResidentController resident = residentQueue.Dequeue();

        Debug.Log($"Spawn Character : [{resident.profile.firstName}]");
        resident.gameObject.SetActive(true);
    }
}

public static class BehaviourFactory
{
    public static ICharacterBehaviour CreateDoppelBehaviour(DoppelType doppelType, DoppelApearanceType apearanceType)
    {
        return new DoppelgangerBehavior(doppelType, apearanceType);
    }

    public static ICharacterBehaviour CreateResidentBehaviour()
    {
        return new ResidentBehaviour();
    }
}


public interface ICharacterBehaviour
{
    public Dialog GetDialog(ResidentController resident, string code);
    public void Talk(ResidentController resident, Dialog dialog);

    public bool HasIDCard();
    public bool HasEntryRequest();
}

public class ResidentBehaviour : ICharacterBehaviour
{
    public Dialog GetDialog(ResidentController resident, string code)
    {
        Debug.Log("Start Talk");

        Dialog dialog = resident.profile.dialogs.Find(c => c.code.Equals(code));

        if (dialog == null)
            return null;

        return dialog;
    }

    public void Talk(ResidentController resident, Dialog dialog)
    {
        if (dialog == null) return;
        InGameUIController.I.ShowTextBox(resident, dialog);
    }
   
    public bool HasIDCard() => true;
    public bool HasEntryRequest() => true;
}

public class DoppelgangerBehavior : ICharacterBehaviour
{
    private bool hasIDCard;
    private bool hasEntryRequest;

    private readonly DoppelType doppelType;
    private readonly DoppelApearanceType appearanceType;

    public DoppelgangerBehavior(DoppelType doppelType, DoppelApearanceType appearanceType)
    {
        this.doppelType = doppelType;
        this.appearanceType = appearanceType;

        hasIDCard = true;
        hasEntryRequest = true;

        if (doppelType.Equals(DoppelType.NonePaper))
        {
            float per = UnityEngine.Random.value;

            if (per <= 0.5f)
            {
                hasIDCard = false;
            }
            else
            {
                hasEntryRequest = false;
            }
        }
    }

    public Dialog GetDialog(ResidentController resident, string code)
    {
        if (resident?.profile == null || resident.profile.dialogs == null)
        {
            Debug.LogWarning("resident or profile or dialogs is null.");
            return null;
        }

        Dialog dialog = resident.profile.dialogs.Find(c => c.code.Equals(code));
        if (dialog == null)
        {
            Debug.LogWarning($"Dialog with code '{code}' not found.");
            return null;
        }
        
        return dialog;
    }

    public Dialog GetRevealDialog(ResidentController doppel, string code)
    {
        Dialog dialog = doppel.profile.doppelData.revealDialog.Find(c => c.code.Equals(code));

        if (dialog == null)
        {
            Debug.LogWarning($"Dialog with code '{code}' not found.");
            return null;
        }
        return dialog;
    }

    public void Talk(ResidentController resident, Dialog dialog)
    {
        if (resident is not DoppelController) return;

        DoppelController doppel = resident as DoppelController;

        if (dialog == null) return;

        if (dialog.code.Contains("Reveal"))
        {
            InGameUIController.I.ShowTextBox_Noise(resident, dialog);
        }
        else
        {
            InGameUIController.I.ShowTextBox(resident, dialog);
        }
    }

    public void Reveal(ResidentController resident, string code)
    {
        Dialog dialog = GetRevealDialog(resident, code);
        Talk(resident, dialog);
    }

    public bool IsAppearanceSuspicious() => doppelType == DoppelType.Appearance;
    public bool HasIDCard() => hasIDCard;
    public bool HasEntryRequest() => hasEntryRequest;
}