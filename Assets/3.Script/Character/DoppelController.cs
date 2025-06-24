using UnityEngine;

public class DoppelController : ResidentController
{
    [SerializeField] GameObject eye;
    [SerializeField] GameObject reveal_Eye;

    [SerializeField] GameObject mouth;
    [SerializeField] GameObject reveal_mouth;

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

    public void SetType(DoppelApearanceType appearanceType, DoppelType doppelType)
    {
        this.appearanceType = appearanceType;
        this.doppelType = doppelType;
    }

    public void Reveal(string code)
    {
        DoppelgangerBehavior doppel_Behavior = this.behavior as DoppelgangerBehavior;

        Debug.Log("Reveal...");
        eye?.SetActive(false);
        reveal_Eye?.SetActive(true);
        doppel_Behavior?.Reveal(this, code);
    }
}