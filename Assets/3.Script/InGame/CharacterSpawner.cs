using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
public class Resident
{
    public Profile profile;
    public string apart_Floor;
    public string apart_Number;

    public Resident(string floor, string number, Profile profile)
    {
        this.profile = profile;
        this.apart_Floor = floor;
        this.apart_Number = number;
    }
}

public class CharacterSpawner : BehaviourSingleton<CharacterSpawner>
{
    protected override bool IsDontDestroy() => false;

    [SerializeField] int level;
    [SerializeField] int maxFloor;              // F01, F02...
    [SerializeField] int maxHouse;              // 01, 02...

    public Dictionary<string, List<Profile>> addressDic;

    [SerializeField] Transform todayEntryParent;
    [SerializeField] TodayEntryComponent todayListComponent_prefab;

    private List<Profile> todayListResidents;
    private int todayListCount;

    [SerializeField] FamilyData familyDatas;

    public List<Profile> characters;          // Inspector

    [Header("Residents")]
    public List<Resident> residents;

    void Start()
    {
        todayListResidents = new();
        residents = new();

        InitLevel();

        // 주소 초기화
        InitAddress();

        InitTodayEntryList();
    }

    private void InitLevel()
    {
        todayListCount = UnityEngine.Random.Range(4, 5);
    }

    private void InitAddress()
    {
        addressDic = new();
        for (int i = 1; i <= maxFloor; i++)
        {
            for (int j = 1; j <= maxHouse; j++)
            {
                string address = $"F{i:D2}-{j:D2}";
                addressDic.Add(address, new());
            }
        }

        var addressKeys = new List<string>(addressDic.Keys);

        // 위 Keys 들을 랜덤으로 재배치 후 Mate 할당하기
        int index = 0;
        addressKeys = addressKeys.OrderBy(x => UnityEngine.Random.value).ToList();

        foreach (var mate in familyDatas.mateList)
        {
            if (index >= addressKeys.Count) break;

            string address = addressKeys[index];
            addressDic[address] = new List<Profile>(mate.mates);

            string[] split = address.Split('-');
            string floor = split[0];
            string number = split[1];

            foreach (var p in mate.mates)
            {
                CreateResident(floor, number, p);
            }

            index++;
        }
    }

    void CreateResident(string floor, string num, Profile profile)
    {
        residents.Add(new(floor, num, profile));
    }

    public void SpawnCharacters()
    {

    }

    public void InitTodayEntryList()
    {
        for (int i = 0; i < 4; i++)
        {
            TodayEntryComponent component = Instantiate(todayListComponent_prefab,todayEntryParent);

            Profile profile = null;

            // 랜덤 데이터 추출
            do
            {
                int index = UnityEngine.Random.Range(0, characters.Count);
                profile = characters[index];
                Debug.Log(profile.name);
            } while (todayListResidents.Find(v => v == profile));

            if (profile == null) continue;

            todayListResidents.Add(profile);

            //주소 찾기
            string address = "";

            address = SearchAddress(profile);

            component.InitComponent(profile.profileImage, profile.firstName, profile.lastName, address);
        }
    }

    public string SearchAddress(Profile profile)
    {
        foreach (var d in addressDic)
        {
            // d 는 List... List에 있는가...?
            if (d.Value.Find(m => m == profile))
            {
                return d.Key;
            }
        }
        return "";
    }
}