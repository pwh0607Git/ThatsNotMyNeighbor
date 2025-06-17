using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class TextBoxController : MonoBehaviour, IPointerDownHandler
{
    [Header("Test Inspector")]
    [SerializeField] List<string> testTexts;

    // List를 받아서 queue에 넣기
    // Queue에 있는 데이터들 모두 순차적으로 출력
    // Queue 가 비어있으면 text box 클릭시 종료.

    [SerializeField] TextMeshProUGUI TMP_lineText;

    private Sequence typingSequence;
    private string currentShowText;
    private Queue<string> textQueue;

    void Start()
    {
        SetTextQueue(testTexts);
        ShowText();
    }

    public void SetTextQueue(List<string> msgs)
    {
        textQueue = new();
        foreach (string m in msgs)
        {
            textQueue.Enqueue(m);
        }
    }

    public void ShowText()
    {
        if (textQueue.Count <= 0)
        {
            Debug.Log("출력할 대사가 없습니다.");
            CloseTextBox();
            return;
        }

        string text = textQueue.Dequeue();

        TMP_lineText.text = "";
        currentShowText = text;

        typingSequence = DOTween.Sequence();

        for (int i = 0; i < text.Length; i++)
        {
            int index = i;
            typingSequence.AppendCallback(() => TMP_lineText.text += text[index])
            .AppendInterval(0.1f);
        }
    }

    public void ShowTotalText()
    {
        TMP_lineText.text = currentShowText;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        // Dotween 시퀀스를 Kill하고 그냥 전체 텍스트 보여주기
        Debug.Log("TextBox Click!!");

        // 만약 타이핑 중이라면.. 죽이고 대사 전체 추출하기
        if (typingSequence != null && typingSequence.IsActive())
        {
            Debug.Log("전체 대사 출력!");
            typingSequence.Kill();
            ShowTotalText();
        }
        else
        {
            Debug.Log("대사 출력이 완료 되었습니다 다음 대사 출력하겠습니다.");
            ShowText();
        }
    }

    public void CloseTextBox()
    {
        this.gameObject.SetActive(false);
    }
}