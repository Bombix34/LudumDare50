using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Tools.Managers;

public class SleepMiniGame : MiniGame
{
    [SerializeField] private GameObject leftPaupiere, rightPaupiere;
    private Vector2 paupiereBasePosition;
    private float leftDuration, rightDuration = 0f;
    [SerializeField] private float speed;
    [SerializeField] private float clickPower = 1f;
    [SerializeField] private float criticState = 20f;
    [SerializeField] private float deadState = 40f;

    private void Start()
    {
        paupiereBasePosition = leftPaupiere.transform.position;
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
        leftDuration += (Time.deltaTime * (speed * (Mathf.Max(1, CurrentDifficulty * 0.8f))));
        rightDuration += (Time.deltaTime * (speed * (Mathf.Max(1, CurrentDifficulty * 0.8f))));

        leftPaupiere.transform.position = new Vector2(leftPaupiere.transform.position.x, paupiereBasePosition.y - leftDuration);
        rightPaupiere.transform.position = new Vector2(rightPaupiere.transform.position.x, paupiereBasePosition.y - rightDuration);
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            SoundManager.instance.PlaySound(AudioFieldEnum.SFX03_BOUP_2);
            bool isRight = Input.mousePosition.x > Screen.width / 2;
            if(isRight)
                rightDuration = Mathf.Max(0f, rightDuration - clickPower);
            else
                leftDuration = Mathf.Max(0f, leftDuration - clickPower);

        }
        if (leftDuration >= criticState || rightDuration >= criticState)
        {
        }
        if (leftDuration >= deadState || rightDuration >= deadState)
        {
            Lose();
        }
    }

    protected override void EndGame()
    {
        base.EndGame();
    }

    protected override void Lose()
    {
        currentGameState = GameState.LOSE;
        EndGame();
    }

    protected override void Win()
    {
        currentGameState = GameState.WIN;
        StartCoroutine(ForceSleepCoroutine());
    }

    protected IEnumerator ForceSleepCoroutine()
    {
        while(leftDuration < deadState || rightDuration < deadState)
        {
            if(leftDuration < deadState)
            {
                leftDuration += (Time.deltaTime * (speed )*2f);
                leftPaupiere.transform.position = new Vector2(leftPaupiere.transform.position.x, paupiereBasePosition.y - leftDuration);
            }
            if(rightDuration < deadState)
            {
                rightDuration += (Time.deltaTime * (speed )*2f);
                rightPaupiere.transform.position = new Vector2(rightPaupiere.transform.position.x, paupiereBasePosition.y - rightDuration);
            }
            yield return null;
        }
        EndGame();
    }
}
