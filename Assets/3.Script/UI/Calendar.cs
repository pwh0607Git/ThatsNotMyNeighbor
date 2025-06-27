using System.Collections;
using UnityEngine;

public class Calendar : MonoBehaviour
{
    [SerializeField] Transform markParent;

    IEnumerator Start()
    {
        yield return new WaitUntil(() => InGameManager.I != null);
        InitMark();
    }

    void InitMark()
    {
        for (int i = 0; i < markParent.childCount; i++)
        {
            markParent.GetChild(i).gameObject.SetActive(false);
        }

        int level = LevelData.level;

        for (int i = 0; i < level; i++)
        {
            GameObject o = markParent.GetChild(i).gameObject;
            o.SetActive(true);
            float angleZ = Random.Range(-90f, 90f);
            o.GetComponent<RectTransform>().localEulerAngles = new Vector3(0, 0, angleZ);
        }
    }
}
