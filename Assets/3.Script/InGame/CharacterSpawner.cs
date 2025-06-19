using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

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

public class CharacterSpawner : MonoBehaviour
{
    private Queue<ResidentController> residentQueue;
    [SerializeField] Transform characterLayer;

    // count수만큼 캐릭터 프리팹을 생성한다.
    List<ResidentController> characterList;

    public void SetCharacters(List<Profile> todayEntryList, int count)
    {
        characterList = new();
        residentQueue = new();
        
        //일일 출입 리스트 캐릭터는 필수로 출력
        CreateTodayEntryListCharacters(todayEntryList);

        //Today 리스트에 있는 캐릭터는 필수로 생성하기.
        CreateOtherCharacters(count - todayEntryList.Count);

        SetQueue();
    }

    void CreateTodayEntryListCharacters(List<Profile> characters)
    {
        foreach (var profile in characters)
        {
            ResidentController character = Instantiate(profile.model, characterLayer).GetComponent<ResidentController>();
            character.SetProfile(profile);

            characterList.Add(character);
        }
    }

    void CreateOtherCharacters(int count)
    {
        var characters = InGameManager.I.Characters;

        for (int i = 0; i < count; i++)
        {
            int index = UnityEngine.Random.Range(0, characters.Count);

            Profile profile = characters[index];

            ResidentController character = Instantiate(profile.model, characterLayer).GetComponent<ResidentController>();
            character.SetProfile(profile);

            characterList.Add(character);
        }
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
        ResidentController resident = residentQueue.Dequeue();

        Debug.Log($"Spawn Character : [{resident.profile.firstName}]");
        resident.Enter();
    }

    public void DespawnCharacter(ResidentController resident)
    {
        
    }
}