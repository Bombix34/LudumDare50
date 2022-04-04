using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;
using DG.Tweening;

public class MiniGameTitleUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI titleText;
    public static Action OnEndDisplayTitleText;

    private int displayedDifficulty = 1;

    private void Start()
    {
        titleText.rectTransform.DOAnchorPosY(Screen.height, 0f);
    }

    private void OnEnable()
    {
        GameManager.OnLaunchMiniGame += LaunchTitle;
        GameManager.OnGameOver += GameOver;
    }

    private void OnDisable()
    {
        GameManager.OnLaunchMiniGame -= LaunchTitle;
        GameManager.OnGameOver -= GameOver;
    }

    private void LaunchTitle(MiniGame miniGame)
    {
        if(miniGame.CurrentDifficulty > displayedDifficulty)
        {
            titleText.text = miniGame.gameName + "\n" + "<size=40%>Difficulty Up!";
            displayedDifficulty = miniGame.CurrentDifficulty;
        }
        else
            titleText.text = miniGame.gameName;
        StartCoroutine(LaunchTitleCoroutine());
    }

    private IEnumerator LaunchTitleCoroutine()
    {
        titleText.rectTransform.DOAnchorPosY(0f, 0.5f);
        yield return new WaitForSeconds(2f);
        titleText.rectTransform.DOAnchorPosY(Screen.height, 0.5f);
        yield return new WaitForSeconds(0.5f);
        OnEndDisplayTitleText?.Invoke();
    }

    private void GameOver(int score)
    {
        titleText.text = "GAME OVER" + "\n" + "<size=120%>"+score.ToString();
        titleText.rectTransform.DOAnchorPosY(0f, 1.5f).SetEase(Ease.OutBack);
    }
}
