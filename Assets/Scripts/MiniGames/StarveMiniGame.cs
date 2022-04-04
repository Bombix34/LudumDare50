using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Tools.Managers;

public class StarveMiniGame : MiniGame
{
    [SerializeField] private GameObject wilsonObject;
    [SerializeField] private SpriteRenderer wilsonShadow;
    [SerializeField] private List<GameObject> carrot;
    [SerializeField] private GameObject rabbit;
    [SerializeField] private GameObject rabbitShadow;
    [SerializeField] private float wilsonSpeed;

    [SerializeField] private float maxHunger = 2f;
    private float currentHunger = 0f;

    private void Start()
    {
        currentHunger = maxHunger;
        foreach (var curCarrot in carrot)
        {
            curCarrot.transform.position = GetRandomPositionInsideScreen;
        }
        wilsonSpeed -= (0.15f * (CurrentDifficulty - 1));
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
        UpdateWilson(Camera.main.ScreenToWorldPoint(Input.mousePosition));
        currentHunger -= Time.deltaTime;
        if (currentHunger <= 0)
        {
            Lose();
            return;
        }
        if (Time.frameCount % 24 == 0)
        {
            foreach (var curCarrot in carrot)
            {
                if (!curCarrot.activeInHierarchy)
                    continue;
                if (Vector2.Distance(curCarrot.transform.position, wilsonObject.transform.position) < 0.5f)
                {
                    SoundManager.instance.PlaySound(AudioFieldEnum.SFX01_BOUP);
                    curCarrot.transform.DOMove(wilsonObject.transform.position, 0.2f);
                    curCarrot.transform.DOScale(0f, 0.2f)
                        .OnComplete(() => curCarrot.SetActive(false));
                    currentHunger = maxHunger;
                    break;
                }
            }
        }
    }

    private void UpdateWilson(Vector2 mousePosition)
    {
        if (!IsGameRunning)
        {
            wilsonObject.GetComponent<Animator>().SetBool("Walking", false);
            return;
        }
        if (Vector2.Distance(wilsonObject.transform.position, mousePosition) > 0.2f)
        {
            Vector2 dirVector = (mousePosition - (Vector2)wilsonObject.transform.position).normalized;
            if (dirVector.x < 0)
            {
                wilsonObject.GetComponentInChildren<SpriteRenderer>().flipX = true;
                wilsonShadow.flipX = false;
            }
            else
            {
                wilsonObject.GetComponentInChildren<SpriteRenderer>().flipX = false;
                wilsonShadow.flipX = true;
            }
            wilsonObject.transform.Translate(dirVector * Time.deltaTime * wilsonSpeed);
            if (!wilsonObject.GetComponent<Animator>().GetBool("Walking"))
                wilsonObject.GetComponent<Animator>().SetBool("Walking", true);
        }
        else
        {
            wilsonObject.GetComponent<Animator>().SetBool("Walking", false);
        }
    }

    private Vector2 GetRandomPositionInsideScreen
    {
        get
        {
            Vector2 screenSize = new Vector2(Screen.width * 0.8f, Screen.height * 0.8f);
            Vector2 worldSize = Camera.main.ScreenToWorldPoint(screenSize);
            return new Vector2(Random.Range(-worldSize.x, worldSize.x), Random.Range(-worldSize.y, worldSize.y));
        }
    }

    protected override void EndGame()
    {
        base.EndGame();
    }

    protected override void Lose()
    {
        if (!IsGameRunning)
            return;
        currentGameState = GameState.LOSE;
        StartCoroutine(ForceDieCoroutine());
    }

    protected override void Win()
    {
        if (!IsGameRunning)
            return;
        currentGameState = GameState.WIN;
        StartCoroutine(ForceDieCoroutine());
    }

    private IEnumerator ForceDieCoroutine()
    {
        wilsonObject.GetComponent<Animator>().SetBool("Walking", false);
        Vector2 currentWilsonPosition = wilsonObject.transform.position;
        rabbitShadow.SetActive(false);
        rabbit.transform.position = new Vector2(wilsonObject.transform.position.x, 15f);
        rabbit.transform.DOMoveY(wilsonObject.transform.position.y, 1f).SetEase(Ease.InCirc);
        while (Vector2.Distance(wilsonObject.transform.position, rabbit.transform.position) > 0.2f)
        {
            wilsonObject.transform.localPosition = currentWilsonPosition + (Vector2.right * Random.Range(-0.05f, 0.05f));
            yield return null;
        }
        wilsonObject.transform.DOScaleY(0.1f, 0.1f);
        rabbit.transform.DOPunchScale(Vector3.one * 0.2f, 0.3f);
        rabbitShadow.SetActive(true);
        EndGame();
    }
}
