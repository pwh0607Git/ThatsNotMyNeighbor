using System;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;

[Serializable]
public class Apartment
{
    public List<Profile> residents;
    public string apart_Floor;
    public string apart_Number;

    // 추후에 추가 예정
    public string telephoneNum;                             // 집 전화 번호
    public Dictionary<Profile, bool> checkAtHome;           // 집에 주민이 있는지...

    public Apartment(string apart_Floor, string apart_Number)
    {
        this.apart_Floor = apart_Floor;
        this.apart_Number = apart_Number;
        this.telephoneNum = "";
        residents = new();
        checkAtHome = new();
    }

    public void SetResident(List<Profile> residents)
    {
        this.residents = new(residents);
        InitCheckAtHome();
    }

    public void SetTelephone(string number)
    {
        this.telephoneNum = number;
    }

    private void InitCheckAtHome()
    {
        if (residents == null || residents.Count <= 0) return;

        foreach (var p in residents)
        {
            checkAtHome[p] = true;
        }
    }

    public void UpdateCheckAtHome(Profile profile, bool atHome)
    {
        checkAtHome[profile] = atHome;
    }
}

public class InGameManager : BehaviourSingleton<InGameManager>
{
    protected override bool IsDontDestroy() => false;

    [Header("Controller")]
    CharacterSpawner characterSpawner;
    [SerializeField] WarningCallController warningCall;

    [Header("Level Data")]
    [SerializeField] int level;
    [SerializeField] int maxFloor;              // F01, F02...
    [SerializeField] int maxHouse;              // 01, 02...

    [Header("CharacterDatas")]
    [SerializeField] List<Profile> characters;          // Inspector
    public List<Profile> Characters => characters;
    public List<Profile> npcs;
    public List<string> telephoneNumbers;
    [SerializeField] FamilyData familyDatas;

    public Dictionary<string, Apartment> addressDic { get; private set; }

    [Header("Dialog code")]
    [SerializeField] List<string> dialogCodes;
    [SerializeField] CharacterSpawner spawner;

    void Start()
    {
        addressDic = new();

        TryGetComponent(out spawner);
        TryGetComponent(out characterSpawner);

        InitControllers();

        InitAddress();

        InitSpawner();

        StartTutorial();
    }

    [Header("SpawnCount")]
    [SerializeField] int fullCount;

    void InitSpawner()
    {
        if (todayEntryListController == null) return;
        spawner.OnCompleteSpawn += InitAtHome;

        spawner.SetCharacters(todayEntryListController.TodayEntryList, fullCount);

        InteractionManager.I.OnExitResident += ResidentExitHandler;
        spawner.OnEmptyCharacterQueue += EndGame;
    }

    void ResidentExitHandler(Profile profile)
    {
        string ad = SearchAddress(profile);
        addressDic[ad].UpdateCheckAtHome(profile, true);

        //후처리로 캐릭터 스폰하기
        DOVirtual.DelayedCall(5f, () => spawner.SpawnCharacter());
    }

    void InitAtHome(List<ResidentController> list)
    {
        Debug.Log("[InGameManager] InitAtHome...");
        List<ResidentController> residents = new();

        foreach (var r in list)
        {
            if (r.type.Equals(CharacterType.Resident))
            {
                //도플갱어인 경우 AtHome을 true로 설정하기
                residents.Add(r);
            }
        }

        // 아파트에 없다는 정보 갱신
        foreach (var r in residents)
        {
            string ad = SearchAddress(r.profile);
            addressDic[ad].UpdateCheckAtHome(r.profile, false);

            Debug.Log($"{r.profile}은 현재 외출중...");
        }
    }

    void InitAddress()
    {
        //주소만 할당
        for (int i = 1; i <= maxFloor; i++)
        {
            for (int j = 1; j <= maxHouse; j++)
            {
                string fullAddress = $"F{i:D2}" + "-" + $"{j:D2}";
                Apartment apt = new($"F{i:D2}", $"{j:D2}");

                addressDic.Add(fullAddress, apt);
            }
        }

        var addressKeys = new List<string>(addressDic.Keys);

        // 위 Keys 들을 랜덤으로 재배치 후 Mate 할당하기
        int index = 0;
        addressKeys = addressKeys.OrderBy(x => UnityEngine.Random.value).ToList();
        telephoneNumbers.OrderBy(i => UnityEngine.Random.value);

        foreach (var mateList in familyDatas.mateList)
        {
            if (index >= addressKeys.Count) break;

            string address = addressKeys[index];
            addressDic[address].SetResident(mateList.mates);
            addressDic[address].SetTelephone(telephoneNumbers[index]);
            index++;
        }

        InGameUIController.I.InitUI();
    }

    public Apartment SearchApartment(string phoneNumber)
    {
        foreach (var apt in addressDic)
        {
            if (apt.Value.telephoneNum.Equals(phoneNumber)) return apt.Value;
        }

        return null;
    }

    public string SearchAddress(Profile profile)
    {
        Debug.Log($"Dic Count : {addressDic.Count}");
        //해당 주민에 대한 주소 찾기
        foreach (var add in addressDic)
        {
            var apartment = add.Value;
            if (apartment.residents == null)
            {
                Debug.LogWarning("residents is null.");
                return "";
            }

            bool check = add.Value.residents.Find(r => r.Equals(profile));
            if (check)
            {
                return add.Key;
            }
        }
        return "";
    }

    #region tutorial
    private ResidentController npc_DDD;
    [SerializeField] Transform characterLayer;

    public void StartTutorial()
    {
        Sequence startSeq = DOTween.Sequence();

        Profile profile = npcs.Find(p => p.id.Equals("000000000"));               // DDD 직원 id

        npc_DDD = Instantiate(profile.model, characterLayer).GetComponent<ResidentController>();
        npc_DDD.SetProperty(profile, CharacterType.NPC, BehaviourFactory.CreateResidentBehaviour());
        warningCall.Init(npc_DDD);

        startSeq.AppendInterval(1f)
            .AppendCallback(() => InGameUIController.I.MoveShutDownDoor(800f))
            .AppendInterval(1f)
            .AppendCallback(() =>
            {
                // 대사 출력
                npc_DDD.GetComponent<ResidentController>().TalkByCode("Tutorial");
            });
    }

    public void EndDDDBehaviour()
    {
        Sequence endSeq = DOTween.Sequence();

        endSeq.AppendInterval(0.5f)
            .AppendCallback(() => npc_DDD.Exit())
            .AppendInterval(4f)
            .OnComplete(() =>
            {
                Debug.Log("캐릭터 스폰 로직 수행!!");
                characterSpawner.SpawnCharacter();
            });
    }
    #endregion


    [Header("Controller")]
    ResidentFolderController residentFileController;
    TodayEntryListController todayEntryListController;

    void InitControllers()
    {
        TryGetComponent(out todayEntryListController);
        TryGetComponent(out residentFileController);

        InGameUIController.I.InitControllers(residentFileController, todayEntryListController);
    }

    void EndGame()
    {
        Sequence endSeq = DOTween.Sequence();

        endSeq.AppendInterval(1.0f)
            .AppendCallback(() => Debug.Log("End Game"))
            .AppendCallback(() => InGameUIController.I.MoveShutDownDoor(0f))
            .AppendInterval(1f)
            .AppendCallback(() => LoadToResultScene());
    }

    private void LoadToResultScene()
    {
        SceneManager.LoadScene("Scn2.Result");
    }
}