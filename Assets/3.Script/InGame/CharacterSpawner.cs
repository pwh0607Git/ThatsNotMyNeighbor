using System.Collections.Generic;
using System.Linq;
using UnityEngine;

//특이 사항
public class Resident
{
    public CharacterType type;              // 진짜 주민인지 도플 갱어 인지.
    public Profile profile;
    public List<SignificantType> significants;
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

    void Start()
    {
        todayListResidents = new();

        InitLevel();

        // 주소 초기화
        InitAddress();

        InitTodayEntryList();
    }

    private void InitLevel()
    {
        todayListCount = Random.Range(4, 5);
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
            if (index >= addressKeys.Count) break; // 주소 부족하면 중단

            string addr = addressKeys[index];
            addressDic[addr] = new List<Profile>(mate.mates); // 주소에 가족 할당
            index++;
        }
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
                int index = Random.Range(0, characters.Count);
                profile = characters[index];
                Debug.Log(profile.name);
            } while (todayListResidents.Find(v => v == profile));

            if (profile == null) continue;

            todayListResidents.Add(profile);

            //주소 찾기
            string address = "";

            address = SearchAddress(profile);

            component.InitComponent(profile.profileImage, profile.firstName, profile.secondName, address);
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


public class ResidentFactory
{
    public void CreateResident(Profile profile, CharacterType type)
    {
        //빈 객체 생성
        Resident resident = new Resident
        {
            profile = profile,
            type = type,
            significants = new()
        };

        //생성
        if (type.Equals(CharacterType.DOPPELGANGER))
        {
            //위조항목 추가하기
            
        }
        else
        {

        }
    }
}