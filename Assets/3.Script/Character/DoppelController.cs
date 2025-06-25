using UnityEngine;

public class DoppelController : ResidentController
{
    [SerializeField] GameObject eye;
    [SerializeField] GameObject reveal_Eye;

    [SerializeField] GameObject mouth;
    [SerializeField] GameObject reveal_mouth;

    public DoppelInform information;

    public DoppelApearanceType appearanceType;// { get; private set; }
    public DoppelType doppelType;// { get; private set; }

    void Start()
    {
        Transform face = transform.Find("Face");

        eye = face?.Find("Eye")?.gameObject;
        reveal_Eye = face.transform.Find("Reveal_Eye")?.gameObject;
        mouth = face.transform.Find("Mouth")?.gameObject;
        reveal_mouth = face.transform.Find("Reveal_Mouth")?.gameObject;

        reveal_Eye?.SetActive(false);
        reveal_mouth?.SetActive(false);
    }

    public void SetType(DoppelApearanceType appearanceType, DoppelType doppelType, DoppelInform information)
    {
        this.appearanceType = appearanceType;
        this.doppelType = doppelType;
        this.information = information;
    }

    public override void TalkByCode(string code)
    {
        if (behavior == null) return;

        code = CheckType(appearanceType) == "None" ? code : CheckType(appearanceType);

        Dialog dialog = behavior.GetDialog(this, code);
        behavior.Talk(this, dialog);
    }
    
    private string CheckType(DoppelApearanceType type) => type switch {
            DoppelApearanceType.NoneLanguage => "NoneLanguage",
            DoppelApearanceType.NoneMouth => "NoneMouth",
            _ => "None",
        };

    public void Reveal(string code)
    {
        DoppelgangerBehavior doppel_Behavior = this.behavior as DoppelgangerBehavior;

        Debug.Log("Reveal...");

        // 눈을 따로 가지고 있는 경우만 호출.
        if (reveal_Eye != null)
        {
            eye?.SetActive(false);
            reveal_Eye?.SetActive(true);
        }
        doppel_Behavior?.Reveal(this, code);
    }
}