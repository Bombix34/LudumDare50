using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using DG.Tweening;
using Tools.Managers;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    private int levelCount = 0;
    private int difficulty = 1;
    [SerializeField] private int countToDifficultyUp = 5;
    [SerializeField] private TextMeshProUGUI scoreText;
    public static Action<MiniGame> OnLaunchMiniGame;
    public static Action<MiniGame> OnEndMiniGame;
    public static Action<int> OnGameOver;

    private bool isGameOver = false;

    [SerializeField] private List<GameObject> miniGamesPrefabs;
    [SerializeField] private TransitionUI transitionUI;
    private List<GameObject> currentAvailableMiniGames;
    private MiniGame currentMiniGame = null;

    public int playerHealth = 3;

    [SerializeField] private List<Image> heartContainers;

    private void Awake()
    {
        currentAvailableMiniGames = new List<GameObject>();
        ResetMiniGameList();
    }

    private void Start()
    {
        StartCoroutine(TempoStartCoroutine());
    }

    private IEnumerator TempoStartCoroutine()
    {
        yield return new WaitForSeconds(1f);
        foreach (var heart in heartContainers)
        {
            heart.transform.DOScale(1f, 0.3f).SetEase(Ease.InOutBounce);
            SoundManager.instance.PlaySound(AudioFieldEnum.SFX03_BOUP_2);
            yield return new WaitForSeconds(0.3f);
        }
        scoreText.transform.DOScale(1f, 0.3f);
        transitionUI.DisplayTransitionPanel();
        yield return new WaitForSeconds(1f);
        CreateMiniGame();
    }

    private void OnEnable()
    {
        OnEndMiniGame += EndMiniGame;
    }

    private void OnDisable()
    {
        OnEndMiniGame -= EndMiniGame;
    }

    private void CreateMiniGame()
    {
        if (currentMiniGame != null)
            Destroy(currentMiniGame.gameObject);
        if (currentAvailableMiniGames.Count == 0)
            ResetMiniGameList();
        GameObject miniGame = currentAvailableMiniGames[UnityEngine.Random.Range(0, currentAvailableMiniGames.Count)];
        currentAvailableMiniGames.Remove(miniGame);
        GameObject newMiniGame = Instantiate(miniGame, this.transform);
        currentMiniGame = newMiniGame.GetComponent<MiniGame>();
        currentMiniGame.CurrentDifficulty = difficulty;
        LaunchMiniGameTitle();
    }

    private void ResetMiniGameList()
    {
        currentAvailableMiniGames.Clear();
        foreach (var miniGames in miniGamesPrefabs)
            currentAvailableMiniGames.Add(miniGames);
    }

    private void LaunchMiniGameTitle()
    {
        if (currentMiniGame == null)
            return;
        MiniGameTitleUI.OnEndDisplayTitleText += LaunchMiniGame;
        OnLaunchMiniGame?.Invoke(currentMiniGame);
    }

    private void LaunchMiniGame()
    {
        currentMiniGame?.Launch();
        MiniGameTitleUI.OnEndDisplayTitleText -= LaunchMiniGame;
    }

    private void LoseHealth()
    {
        GameObject currentHeart = heartContainers[playerHealth - 1].gameObject;
        currentHeart.transform.DORotate(new Vector3(0, 0, 3600), 3f);
        currentHeart.transform.DOMoveY(-Screen.height, 2f);
        playerHealth--;
        if(playerHealth <= 0)
        {
            //GAME OVER
            SoundManager.instance.PlaySound(AudioFieldEnum.SFX02_GAME_OVER);
            SoundManager.instance.FinalFade();
            isGameOver = true;
            scoreText.transform.DOScale(0f, 0.3f);
            OnGameOver?.Invoke(levelCount);
        }
    }

    private void EndMiniGame(MiniGame miniGameEnded)
    {
        if(miniGameEnded.IsWinning)
        {
            SoundManager.instance.PlaySound(AudioFieldEnum.SFX05_WIN);
            SoundManager.instance.FadeMusicForJingle();

        }
        else
        {
            SoundManager.instance.FadeMusicForJingle();
            Camera.main.GetComponent<CameraShaker>().Shake(0.3f);
            LoseHealth();
            if(!isGameOver)
                SoundManager.instance.PlaySound(AudioFieldEnum.SFX04_ERROR);
        }
        if (!isGameOver)
            StartCoroutine(EndMiniGameCoroutine());
        else
            StartCoroutine(GameOverCoroutine());
    }

    private IEnumerator EndMiniGameCoroutine()
    {
        levelCount++;
        UpdateScoreText();
        if (levelCount % countToDifficultyUp == 0)
            difficulty++;
        yield return new WaitForSeconds(2f);
        transitionUI.DisplayTransitionPanel();
        yield return new WaitForSeconds(1f);
        CreateMiniGame();
    }

    private IEnumerator GameOverCoroutine()
    {
        yield return new WaitForSeconds(5f);
        SceneManager.LoadScene("MenuScene");
    }

    private void UpdateScoreText()
    {
        scoreText.text = levelCount.ToString();
        scoreText.transform.DOPunchScale(Vector3.one * 0.3f, 0.2f);
    }

    public bool IsCurrentMiniGameRunning
    {
        get => currentMiniGame != null && currentMiniGame.IsGameRunning;
    }
}
