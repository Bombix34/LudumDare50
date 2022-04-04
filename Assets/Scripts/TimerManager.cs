using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;
using DG.Tweening;

public class TimerManager : MonoBehaviour
{
    public float BASE_TIMER_DURATION;
    private float currentTimer;

    public static Action OnEndTimer;

    [SerializeField] private GameManager manager;

    [SerializeField] private RectTransform chronoPanel;
    [SerializeField] private Image chronoUI;

    private bool isRunning = false;

    private void Start()
    {
        currentTimer = BASE_TIMER_DURATION;
        OnEndTimer += ResetTimer;
    }

    private void OnEnable()
    {
        MiniGameTitleUI.OnEndDisplayTitleText += LaunchMiniGameChrono;
        GameManager.OnEndMiniGame += ResetOnEndMiniGame;
    }

    private void OnDisable()
    {
        MiniGameTitleUI.OnEndDisplayTitleText -= LaunchMiniGameChrono;
        GameManager.OnEndMiniGame -= ResetOnEndMiniGame;
    }

    private void Update()
    {
        if (!isRunning || !manager.IsCurrentMiniGameRunning)
            return;
        currentTimer -= Time.deltaTime;
        chronoUI.fillAmount = currentTimer / BASE_TIMER_DURATION;
        if(currentTimer<=0f)
        {
            OnEndTimer?.Invoke();
        }
    }

    private void ResetTimer()
    {
        isRunning = false;
        currentTimer = BASE_TIMER_DURATION;
        chronoPanel.DOScale(0f, 0.3f);
    }

    private void ResetOnEndMiniGame(MiniGame miniGame)
    {
        ResetTimer();
    }


    private void LaunchMiniGameChrono()
    {
        isRunning = true;
        chronoPanel.DOScale(1f, 0.3f); 
    }
}
