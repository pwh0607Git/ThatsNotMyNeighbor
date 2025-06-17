using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TodayEntryComponent : MonoBehaviour
{
    [SerializeField] Image profileImage;
    [SerializeField] TextMeshProUGUI TMP_firstName;
    [SerializeField] TextMeshProUGUI TMP_lastName;
    [SerializeField] TextMeshProUGUI TMP_address;

    public void InitComponent(Sprite profileSprite, string firstName, string lastName, string address)
    {
        profileImage.sprite = profileSprite;
        this.TMP_firstName.text = firstName;
        this.TMP_lastName.text = lastName;
        this.TMP_address.text = address;
    }
}