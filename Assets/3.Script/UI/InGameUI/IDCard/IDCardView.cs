using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class IDCardView : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI firstName;
    [SerializeField] TextMeshProUGUI lastName;
    [SerializeField] TextMeshProUGUI id;
    [SerializeField] TextMeshProUGUI exp;
    [SerializeField] Image profileImage;

    [SerializeField] Image mark;

    public void SetData(Profile profile, ForgedType forgedType)
    {
        this.firstName.text = profile.firstName;
        this.lastName.text = profile.lastName;
        this.id.text = profile.id;
        this.exp.text = profile.expiration;
        this.profileImage.sprite = profile.profileImage;
        mark.enabled = true;
        
        switch (forgedType)
        {
            case ForgedType.Mark:
                {
                    mark.enabled = false;
                    break;
                }
            case ForgedType.Expiration:
                {
                    exp.text = "";
                    break;
                }
            case ForgedType.Id:
                {
                    id.text = MakeForgedId(profile.id);
                    break;
                }
        }
    }

    private string MakeForgedId(string id)
    {
        int index = Random.Range(0, id.Length);
        char originalChar = id[index];
        char newChar;
        do
        {
            newChar = (char)('0' + Random.Range(0, 10));
        } while (newChar == originalChar);

        char[] chars = id.ToCharArray();
        chars[index] = newChar;

        return new string(chars);
    }

    public void SetActive(bool on)
    {
        this.gameObject.SetActive(on);
    }
}
