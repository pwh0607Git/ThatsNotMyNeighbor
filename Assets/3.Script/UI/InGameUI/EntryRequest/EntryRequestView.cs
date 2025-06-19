using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EntryRequestView : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI firstName;
    [SerializeField] TextMeshProUGUI lastName;
    [SerializeField] TextMeshProUGUI address;
    [SerializeField] TextMeshProUGUI reason;

    [SerializeField] Image profileImage;

    public void SetData(Profile profile, string address, string reason)
    {
        this.firstName.text = profile.firstName;
        this.lastName.text = profile.lastName;
        this.address.text = address;
        this.reason.text = reason;
        this.profileImage.sprite = profile.profileImage;
    }

    public void SetActive(bool on)
    {
        this.gameObject.SetActive(on);
    }
}
