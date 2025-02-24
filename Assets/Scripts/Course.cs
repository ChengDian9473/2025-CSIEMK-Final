using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using TMPro;

public class Course : MonoBehaviour
{
    public int index = -1;

    [HideInInspector]
    public int isSelected = -1;

    GameManager gameManager;
    private bool isFocused = false;
    private Vector2 originPos;
    private Vector2 originSize;
    private RectTransform rt;
    private Image info;
    private Image mask;
    private TextMeshProUGUI banned;
    private Canvas canvas;

    private Color[] colors =
    {
        new Color(0, 0, 0, 0.86f),
        new Color(1, 0.8272578f, 0, 0.4745098f),
        new Color(0, 0.2732866f, 1, 0.6196078f),
        new Color(1, 0, 0.03027153f, 0.4901961f),
        new Color(0, 1, 0.088377f, 0.4392157f)
    };

    void Start()
    {
        if (index == -1)
        {
            Debug.LogError("Index not initialized");
        }
        gameManager = GameManager.Instance;

        rt = GetComponent<RectTransform>();
        originPos = rt.anchoredPosition;
        originSize = rt.sizeDelta;
        info = transform.Find("Info").GetComponent<Image>();
        canvas = GetComponent<Canvas>();
        mask = transform.Find("Mask").GetComponent<Image>();
        banned = transform.GetComponentInChildren<TextMeshProUGUI>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (isFocused)
            {
                DOTween.Clear(destroy: true);
                isFocused = false;
                rt.anchoredPosition = originPos;
                rt.sizeDelta = originSize;
                info.color = new Color(1, 1, 1, 0);
                canvas.sortingOrder = 1;
                gameManager.StartBlink();
                banned.enabled = false;
                SetMask(isSelected);
            }
        }
    }

    public void SetMask(int index)
    {
        mask.color = colors[index];
        mask.enabled = true;
    }

    public void RemoveMask()
    {
        mask.enabled = false;
    }

    public void OnSelected()
    {
        if (isFocused)
            return;

        isFocused = true;
        isSelected = gameManager.GetCurrentPlayer();
        gameManager.StopBlink();

        RemoveMask();
        Vector2 targetPos = new Vector2(950, -530);
        Vector2 targetSize = new Vector2(1900, 1060);

        canvas.sortingOrder = 2;
        Sequence seq = DOTween.Sequence();

        if (isSelected != 0)
            gameManager.PlaySelectedSFX();

        seq.Append(rt.DOAnchorPos(targetPos, 0.9f))
            .Join(rt.DOSizeDelta(targetSize, 0.9f))
            .Append(info.DOFade(1, 0.5f));

        if (isSelected == 0)
        {
            // Ban
            RectTransform maskRt = mask.GetComponent<RectTransform>();

            seq.AppendInterval(0.15f);
            seq.AppendCallback(() =>
            {
                maskRt.anchoredPosition = new Vector2(0, 1060);
                SetMask(0);
                banned.enabled = true;
                gameManager.PlayBannedSFX();
            });
            seq.Append(maskRt.DOAnchorPos(new Vector2(0, 0), 0.85f).SetEase(Ease.OutBounce));
        }
    }
}
