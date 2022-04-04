using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Tools.Managers;
using DG.Tweening;

public class FallingMiniGame : MiniGame
{
    private float currentRotation = -1f;

    [SerializeField] private float maxRotation;
    [SerializeField] private Animator characterAnim;
    [SerializeField] private GameObject rotatingCharacter;
    [SerializeField] private float refillAmount;
    [SerializeField] private Vector2 speedRange;

    private float currentRotationSpeed;

    private void Awake()
    {
        characterAnim.enabled = false;
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
        if(Time.frameCount%50==0)
        {
            currentRotationSpeed = Random.Range(speedRange.x, speedRange.y);
            currentRotationSpeed *= (Mathf.Max(1, CurrentDifficulty * 0.65f));
        }
        rotatingCharacter.transform.DORotate(new Vector3(0f, 0f, currentRotation), 0f);
        if (currentRotation < 0f)
            currentRotation -= Time.deltaTime * (currentRotationSpeed * (Mathf.Max(1, CurrentDifficulty * 0.8f)));
        else
            currentRotation += Time.deltaTime * (currentRotationSpeed * (Mathf.Max(1, CurrentDifficulty * 0.8f)));
        if (Input.GetKey(KeyCode.Mouse0))
        {
            SoundManager.instance.PlaySound(AudioFieldEnum.SFX03_BOUP_2);
            bool isRight = Input.mousePosition.x > Screen.width / 2;
            if (isRight)
                currentRotation -= refillAmount*Time.deltaTime * (currentRotationSpeed * (Mathf.Max(1, CurrentDifficulty * 0.8f)));
            else
                currentRotation += refillAmount*Time.deltaTime * (currentRotationSpeed * (Mathf.Max(1, CurrentDifficulty * 0.8f)));
        }
        rotatingCharacter.transform.Rotate(new Vector3(0f, 0f, currentRotation * Time.deltaTime));
        if (Mathf.Abs(currentRotation) > maxRotation)
        {
            if(currentRotation > 0)
            {
                characterAnim.enabled = true;
                characterAnim.SetTrigger("FallLeft");
            }
            else
            {
                characterAnim.enabled = true;
                characterAnim.SetTrigger("FallRight");
            }
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
        StartCoroutine(ForceDieCoroutine());
    }

    protected IEnumerator ForceDieCoroutine()
    {
        while(currentRotation < maxRotation)
        {
            currentRotation += 2f* refillAmount * Time.deltaTime * (currentRotationSpeed * (Mathf.Max(1, CurrentDifficulty * 0.8f)));
            rotatingCharacter.transform.DORotate(new Vector3(0f, 0f, currentRotation), 0f);
            yield return null;
        }
        characterAnim.enabled = true;
        characterAnim.SetTrigger("FallLeft");
        EndGame();
    }
}
