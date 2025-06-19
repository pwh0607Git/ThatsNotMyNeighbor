using System.Collections;
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

    public void SetIDCard(Profile profile, List<ForgedType> types = null)
    {
        this.firstName.text = profile.firstName;
        this.lastName.text = profile.lastName;
        this.id.text = profile.id;
        this.exp.text = profile.expiration;
        this.profileImage.sprite = profile.profileImage;

        //위조 데이터 생성
        if (types == null || types.Count <= 0) return;

        foreach (var type in types)
        {
            switch (type)
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

                        break;
                }

            }
        }
    }

    public void SetActive(bool on)
    {
        this.gameObject.SetActive(on);
    }
}
