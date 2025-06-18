using System;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;

[Serializable]
public class Resident
{
    public Profile profile;
    bool atHome;

    public Resident(Profile profile, bool atHome)
    {
        this.profile = profile;
        this.atHome = atHome;
    }
}

[Serializable]
public class Apartment
{
    public List<Profile> residents;
    public string apart_Floor;
    public string apart_Number;

    // 추후에 추가 예정
    public string telephoneNum;                             // 집 전화 번호
    public Dictionary<Profile, bool> checkAtHome;           // 집에 주민이 있는지...

    public Apartment(string apart_Floor, string apart_Number, string telephoneNum)
    {
        this.apart_Floor = apart_Floor;
        this.apart_Number = apart_Number;
        this.telephoneNum = telephoneNum;

        checkAtHome = new();
    }

    public void SetResident(List<Profile> residents)
    {
        this.residents = new(residents);
    }
}

public class InGameManager : BehaviourSingleton<InGameManager>
{
    protected override bool IsDontDestroy() => false;

    [Header("Level Data")]
    [SerializeField] int level;
    [SerializeField] int maxFloor;              // F01, F02...
    [SerializeField] int maxHouse;              // 01, 02...

    [Header("CharacterDatas")]
    public List<Profile> characters;          // Inspector
    public List<Profile> npcs;

    [SerializeField] FamilyData familyDatas;

    public Dictionary<string, Apartment> addressDic { get; private set; }

    [Header("Dialog code")]
    [SerializeField] List<string> dialogCodes;

    void Start()
    {
        addressDic = new();

        InitAddress();

        StartTutorial();
    }

    void InitAddress()
    {
        //주소만 할당
        for (int i = 1; i <= maxFloor; i++)
        {
            for (int j = 1; j <= maxHouse; j++)
            {
                string fullAddress = $"F{i:D2}" + "-" + $"{j:D2}";
                Apartment apt = new($"F{i:D2}", $"{j:D2}", "0000");

                addressDic.Add(fullAddress, apt);
            }
        }

        var addressKeys = new List<string>(addressDic.Keys);

        // 위 Keys 들을 랜덤으로 재배치 후 Mate 할당하기
        int index = 0;
        addressKeys = addressKeys.OrderBy(x => UnityEngine.Random.value).ToList();

        foreach (var mateList in familyDatas.mateList)
        {
            if (index >= addressKeys.Count) break;
            string address = addressKeys[index];
            addressDic[address].SetResident(mateList.mates);

            index++;
        }

        InGameUIController.I.InitUI();
    }


    [SerializeField] PersonController npc_DDD;
    [SerializeField] Transform characterLayer;

    public void StartTutorial()
    {
        Sequence startSeq = DOTween.Sequence();

        Profile profile = npcs.Find(p => p.id.Equals("000000000"));               // DDD 직원 id

        npc_DDD = Instantiate(profile.model, characterLayer).GetComponent<PersonController>();
        npc_DDD.SetProfile(profile);

        startSeq.AppendInterval(1f)
            .AppendCallback(() => InGameUIController.I.MoveShutDownDoor(800f))
            .AppendInterval(1f)
            .AppendCallback(() =>
            {
                // 대사 출력
                npc_DDD.GetComponent<PersonController>().Talk("Tutorial");
            });
    }

    public void EndTutorial()
    {
        Sequence endSeq = DOTween.Sequence();

        endSeq.AppendInterval(0.5f)
            .AppendCallback(() => npc_DDD.Exit())
            .AppendInterval(1f)
            .OnComplete(() => Debug.Log("캐릭터 스폰 로직 수행!!"));
    }
}