using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Tools.Managers;

public class DogMiniGame : MiniGame
{
    [SerializeField] private GameObject playerArm;
    [SerializeField] private SpriteRenderer dogHead;
    [SerializeField] private Sprite dogHappyHead;
    private Vector2 playerArmBasePosition;

    private float duration = 0f;
    [SerializeField] private float speed;
    [SerializeField] private float clickPower = 1f;
    [SerializeField] private float criticState = 20f;
    [SerializeField] private float deadState = 40f;

    private void Start()
    {
        playerArmBasePosition = playerArm.transform.position;
        playerArm.GetComponentInChildren<ClickableElement>().OnClickElement += OnClick;
    }

    private void OnEnable()
    {
        TimerManager.OnEndTimer += Win;
    }

    private void OnDisable()
    {
        TimerManager.OnEndTimer -= Win;
    }

    private void Update()
    {
        if (!IsGameRunning)
            return;
        duration += (Time.deltaTime * (speed * (Mathf.Max(1, CurrentDifficulty * 0.6f))));
        //HAND HERE
        playerArm.transform.position = new Vector2(playerArmBasePosition.x, playerArmBasePosition.y + duration);
        if (duration >= criticState)
        {
            // balloonEyes.transform.localScale += Vector3.one * Time.deltaTime;
        }
        if (duration >= deadState)
        {
            Lose();
        }
    }

    private void OnClick()
    {
        SoundManager.instance.PlaySound(AudioFieldEnum.SFX03_BOUP_2);
        duration = Mathf.Max(0f, duration - clickPower);
    }

    protected override void EndGame()
    {
        StartCoroutine(EndGameCoroutine());
        base.EndGame();
    }

    private IEnumerator EndGameCoroutine()
    {
        playerArm.GetComponent<Animator>().SetBool("Pet", true);
        dogHead.sprite = dogHappyHead;
        yield return new WaitForSeconds(2f);
        playerArm.transform.DOMoveY(playerArmBasePosition.y, 1f);
    }

    protected override void Lose()
    {
        currentGameState = GameState.LOSE;
        EndGame();
    }

    protected override void Win()
    {
        currentGameState = GameState.WIN;
        StartCoroutine(ForcePet());
    }

    private IEnumerator ForcePet()
    {
        while (duration < deadState)
        {
            duration += (Time.deltaTime * (speed * CurrentDifficulty)) * 2f;
            playerArm.transform.position = new Vector2(playerArmBasePosition.x, playerArmBasePosition.y + duration);
            yield return null;
        }
        EndGame();
    }
}
