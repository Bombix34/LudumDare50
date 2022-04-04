using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Tools.Managers;

public class PerceMiniGame : MiniGame
{
    [SerializeField] private GameObject balloonObject;
    [SerializeField] private GameObject balloonEyes;
    [SerializeField] private Sprite faceSpriteOpen, faceSpriteClose, faceSpriteEnd;
    private Vector3 balloonBaseScale;
    private Vector3 balloonBasePosition;
    private Vector3 balloonEyesBasePosition;
    private Vector3 balloonEyesBaseScale;
    private float duration = 0f;
    [SerializeField] private float speed;
    [SerializeField] private float clickPower = 1f;
    [SerializeField] private float criticState = 20f;
    [SerializeField] private float deadState = 40f;

    private void Start()
    {
        balloonBaseScale = balloonObject.transform.localScale;
        balloonBasePosition = balloonObject.transform.position;
        balloonEyesBaseScale = balloonEyes.transform.localScale;
        balloonEyesBasePosition = balloonEyes.transform.position;
    }

    private void OnEnable()
    {
        TimerManager.OnEndTimer += Win;
        GetComponentInChildren<ClickableElement>().OnClickElement += OnClick;
    }

    private void OnDisable()
    {
        TimerManager.OnEndTimer -= Win;
    }

    private void Update()
    {
        if (Time.frameCount % 600 == 0 && duration < criticState)
        {
            float rand = Random.Range(0f, 1f);
            if (rand < 0.7f)
                StartCoroutine(WinkCoroutine());
        }
        if (!IsGameRunning)
            return;
        duration += (Time.deltaTime*(speed*(Mathf.Max(1, CurrentDifficulty*0.8f))));
        balloonObject.transform.localScale = balloonBaseScale + ( Vector3.one * duration );
        balloonObject.transform.localPosition = balloonBasePosition + (Vector3.right * Random.Range(-0.2f, 0.2f));
        balloonEyes.transform.localPosition = balloonEyesBasePosition + (Vector3.right * Random.Range(-0.1f, 0.1f));
        balloonEyes.transform.localScale = balloonEyesBaseScale + (Vector3.one * 0.5f * duration);
        if (duration >= criticState)
        {
           // balloonEyes.transform.localScale += Vector3.one * Time.deltaTime;
            balloonEyes.GetComponent<SpriteRenderer>().sprite = faceSpriteEnd;
        }
        if(duration >= deadState)
        {
            Lose();
        }
    }

    private void OnClick()
    {
        SoundManager.instance.PlaySound(AudioFieldEnum.SFX03_BOUP_2);
        duration = Mathf.Max(0f, duration - clickPower);
    }

    protected IEnumerator WinkCoroutine()
    {
        balloonEyes.GetComponent<SpriteRenderer>().sprite = faceSpriteClose;
        yield return new WaitForSeconds(0.1f);
        balloonEyes.GetComponent<SpriteRenderer>().sprite = faceSpriteOpen;
    }

    protected override void EndGame()
    {
        balloonObject.SetActive(false);
        balloonEyes.SetActive(false);
        base.EndGame();
    }

    protected override void Lose()
    {
        currentGameState = GameState.LOSE;
        GetComponentInChildren<ClickableElement>().OnClickElement -= OnClick;
        EndGame();
    }

    protected override void Win()
    {
        currentGameState = GameState.WIN;
        StartCoroutine(ForceExplode());
    }

    private IEnumerator ForceExplode()
    {
        while(duration < deadState)
        {
            duration += (Time.deltaTime * (speed * CurrentDifficulty))*5f;
            balloonObject.transform.localScale = balloonBaseScale + (Vector3.one * duration);
            balloonObject.transform.localPosition = balloonBasePosition + (Vector3.right * Random.Range(-0.3f, 0.3f));
            balloonEyes.transform.localPosition = balloonEyesBasePosition + (Vector3.right * Random.Range(-0.1f, 0.1f));
            balloonEyes.transform.localScale = balloonEyesBaseScale  + (Vector3.one *0.5f * duration);
            if (duration >= criticState)
            {
                balloonEyes.GetComponent<SpriteRenderer>().sprite = faceSpriteEnd;
            }
            yield return null;
        }
        Camera.main.GetComponent<CameraShaker>().Shake(0.3f);
        EndGame();
    }
}
