using System;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum GameMode
{
    Arcade, Endless,
}
public static class LevelData
{
    public static int level = 0;
    public static GameMode mode = GameMode.Arcade;
    public static void ResetLevel(){
        level = 0;
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

    [Header("GameMode")]
    [SerializeField] GameMode mode;

    [Header("Controller")]
    [SerializeField] WarningCallController warningCall;

    [Header("Level Data")]
    [SerializeField] int maxFloor;              // F01, F02...
    [SerializeField] int maxHouse;              // 01, 02...

    [Header("CharacterDatas")]
    [SerializeField] List<Profile> characters;          // Inspector
    public List<Profile> Characters => characters;
    public Profile profile_DDD;
    public List<string> telephoneNumbers;
    [SerializeField] FamilyData familyDatas;

    public List<Profile> TodayEntryList { get; private set; }
    public Dictionary<string, Apartment> addressDic { get; private set; }

    [Header("Spawner")]
    private CharacterSpawner spawner;

    [Header("InGameSource")]
    [SerializeField] AudioClip inGameBgm;
    [SerializeField] GameObject gameOverPan;
    [SerializeField] AudioClip gameOverMasterSource;
    [SerializeField] AudioClip gameOverEffectSource;
    
    void ResetGame()
    {
        InitInGameSource();

        InitAddress();

        InitSpawner();

        DOVirtual.DelayedCall(1.5f, () => StartGame());
    }

    void InitInGameSource()
    {
        addressDic = new();
        SoundManager.I.SetMasterAudio(inGameBgm);
        gameOverPan?.SetActive(false);

        //TodayEntrList 생성.
        if (TodayEntryList != null) TodayEntryList.Clear();

        int todayEntryListcount = 4 + (LevelData.level / 3);
        TodayEntryList = GetRandomList(Characters, todayEntryListcount);

        InGameUIController.I.InitControllers();
    }
 
    void InitSpawner()
    {
        spawner = new(characterLayer);

        spawner.OnCompleteSpawn += InitAtHome;

        int spawnCount = 8 + (LevelData.level / 2);
        spawner.SetCharacters(TodayEntryList, spawnCount);

        InteractionManager.I.OnExitResident += ResidentExitHandler;
        spawner.OnEmptyCharacterQueue += EndGame;
    }

    public int life;

    void ResidentExitHandler(Profile profile)
    {
        string ad = SearchAddress(profile);
        addressDic[ad].UpdateCheckAtHome(profile, true);

        if (LevelManager.I.data.enterDoppelCount >= life)
        {
            Debug.Log("도플갱어가 들어간 수가 3개를 넘어 갔습니다.");
            GameOver();
            return;
        }

        //후처리로 캐릭터 스폰하기
        DOVirtual.DelayedCall(5f, () => spawner.SpawnCharacter());
    }

    void InitAtHome(List<ResidentController> list)
    {
        Debug.Log("[InGameManager] InitAtHome...");
        List<ResidentController> residents = new();

        foreach (var r in list)
        {
            if (r.type.Equals(CharacterType.NPC)) continue;

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

            if (r.type.Equals(CharacterType.NPC)) continue;
            addressDic[ad].UpdateCheckAtHome(r.profile, false);
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

    private ResidentController npc_DDD;
    [SerializeField] Transform characterLayer;

    public void StartGame()
    {
        if (mode.Equals(GameMode.Arcade)) PerformAracade();
        else if (mode.Equals(GameMode.Endless)) PerformEndless();
    }

    void PerformAracade()
    {
        Sequence startSeq = DOTween.Sequence();

        npc_DDD = Instantiate(profile_DDD.model, characterLayer).GetComponent<ResidentController>();
        npc_DDD.SetProperty(profile_DDD, CharacterType.NPC, BehaviourFactory.CreateResidentBehaviour());

        (npc_DDD as DDDController).OnCompleteBehaviour += SpawnResident;

        warningCall.Init(npc_DDD);

        startSeq.AppendInterval(1f)
            .AppendCallback(() => InGameUIController.I.MoveShutDownDoor(800f))
            .AppendInterval(1f)
            .AppendCallback(() =>
            {
                // 대사 출력
                npc_DDD.TalkByCode("Greeting");
            });
    }

    void PerformEndless()
    {
        Sequence startSeq = DOTween.Sequence();

        npc_DDD = Instantiate(profile_DDD.model, characterLayer).GetComponent<ResidentController>();
        npc_DDD.SetProperty(profile_DDD, CharacterType.NPC, BehaviourFactory.CreateResidentBehaviour());

        (npc_DDD as DDDController).OnCompleteBehaviour += SpawnResident;

        warningCall.Init(npc_DDD);

        startSeq.AppendInterval(1f)
            .AppendCallback(() => InGameUIController.I.MoveShutDownDoor(800f))
            .AppendInterval(1f)
            .AppendCallback(() =>
            {
                npc_DDD.TalkByCode("Greeting_Endless");
            });
    }

    void SpawnResident()
    {
        spawner.SpawnCharacter();
    }
    
    void EndGame()
    {
        Sequence endSeq = DOTween.Sequence();

        if (mode.Equals(GameMode.Arcade))
        {
            endSeq.AppendInterval(1.0f)
                .AppendCallback(() => InGameUIController.I.MoveShutDownDoor(0f))
                .AppendInterval(1f)
                .AppendCallback(() => LoadToResultScene());
        } else if (mode.Equals(GameMode.Endless)) {
            endSeq.AppendInterval(1.0f)
                .AppendCallback(() => InGameUIController.I.MoveShutDownDoor(0f))
                .AppendInterval(1f)
                .AppendCallback(() => LoadingController.LoadScene("Scn1.InGame_Endless"));
        }

    }

    public void GameOver()
    {
        Sequence overSeq = DOTween.Sequence();

        overSeq.AppendInterval(2f)
                .AppendCallback(()=>SoundManager.I.SetEffectAudio(gameOverMasterSource))
                .AppendInterval(5f)
                .AppendCallback(() => gameOverPan.SetActive(true))
                .AppendInterval(4f)
                .AppendCallback(() => SoundManager.I.SetEffectAudio(gameOverEffectSource))
                .AppendInterval(1f)
                .AppendCallback(() => LoadToResultScene());

        LevelData.ResetLevel();
    }


    public List<Profile> GetRandomList(List<Profile> chararcters, int count)
    {        
        List<Profile> selected = new();
        while (selected.Count < count)
        {
            var c = chararcters[UnityEngine.Random.Range(0, chararcters.Count)];
            if (!selected.Contains(c)) selected.Add(c);
        }

        return selected;
    }


    private void LoadToResultScene()
    {
        LoadingController.LoadScene("Scn2.Result");
    }

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log("[InGameManager] : Scene Loaded => Game Reset");
        if (this.mode.Equals(GameMode.Endless))
        {
            LevelData.level++;
            LevelManager.I.AccumulateData();
            LevelManager.I.data.ResetData();
        }
        ResetGame();
    }
}