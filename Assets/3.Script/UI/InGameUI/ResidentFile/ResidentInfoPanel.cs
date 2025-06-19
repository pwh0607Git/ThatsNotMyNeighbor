using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ResidentInfoPanel : MonoBehaviour
{
    [SerializeField] Image residentImage;
    [SerializeField] TextMeshProUGUI firstName;
    [SerializeField] TextMeshProUGUI lastName;
    [SerializeField] TextMeshProUGUI id;
    [SerializeField] TextMeshProUGUI job;

    public void InitInformation(Profile profile)
    {
        residentImage.sprite = profile.profileImage;
        this.firstName.text = profile.firstName;
        this.lastName.text =  profile.lastName;
        this.id.text = profile.id;
        this.job.text = profile.job;
    }
}