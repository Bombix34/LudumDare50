using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class TransitionUI : MonoBehaviour
{
    [SerializeField] private RectTransform panel;

    private void Start()
    {
        //panel.DOAnchorPosY(Screen.height, 1f);
    }

    private void OnEnable()
    {
        GameManager.OnGameOver += GameOver;
    }

    private void OnDisable()
    {
        GameManager.OnGameOver -= GameOver;
    }

    public void DisplayTransitionPanel()
    {
        StartCoroutine(TransitionCoroutine());
    }

    private IEnumerator TransitionCoroutine()
    {
        panel.DOAnchorPosY(0f, 1f);
        yield return new WaitForSeconds(2f);
        panel.DOAnchorPosY(Screen.height, 1f);
    }

    public void GameOver(int score)
    {
        panel.DOAnchorPosY(0f, 1f).SetEase(Ease.InOutElastic);
    }
}
