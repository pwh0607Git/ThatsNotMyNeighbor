using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EntryRequestView : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI firstName;
    [SerializeField] TextMeshProUGUI lastName;
    [SerializeField] TextMeshProUGUI address;
    [SerializeField] TextMeshProUGUI reason;
    [SerializeField] Image mark;
    [SerializeField] Image profileImage;

    public void SetData(Profile profile, string address, string reason, ForgedType forgedType)
    {
        this.firstName.text = profile.firstName;
        this.lastName.text = profile.lastName;
        this.address.text = address;
        this.reason.text = reason;
        this.profileImage.sprite = profile.profileImage;

        switch (forgedType)
        {
            case ForgedType.Mark:
                {
                    mark.enabled = false;
                    break;
                }
            default:
                {
                    mark.enabled = true;
                    break;
                }
        }
    }

    public void SetActive(bool on)
    {
        this.gameObject.SetActive(on);
    }
}