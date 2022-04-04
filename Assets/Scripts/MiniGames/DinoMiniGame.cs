using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DinoMiniGame : MiniGame
{
    [SerializeField] private GameObject dinoObject;
    [SerializeField] private SpriteRenderer dinoShadow;
    [SerializeField] private float dinoSpeed;

    [SerializeField] private GameObject meteoritePrefab;

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
        if (!dinoObject.activeInHierarchy)
            Lose();
        UpdateMeteoriteSpawn();
        UpdateDino(Camera.main.ScreenToWorldPoint(Input.mousePosition));
    }

    private void UpdateMeteoriteSpawn()
    {
        if(Time.frameCount % (79-(15*CurrentDifficulty-1)) == 0)
        {
            GameObject newMeteorite = Instantiate(meteoritePrefab, this.transform);
            //newMeteorite.transform.position = GetRandomPositionInsideScreen;
            newMeteorite.transform.position = dinoObject.transform.position + (Vector3.right * Random.Range(-1f, 1f)) + (Vector3.up * Random.Range(-1f, 1f));
            newMeteorite.GetComponent<DinoMeteorite>().player = dinoObject;
        }
        else if(Time.frameCount % (199 - (30 * CurrentDifficulty - 1)) == 0)
        {
            GameObject newMeteorite = Instantiate(meteoritePrefab, this.transform);
            newMeteorite.transform.position = GetRandomPositionInsideScreen;
            //newMeteorite.transform.position = dinoObject.transform.position + (Vector3.right * Random.Range(-1f, 1f)) + (Vector3.up * Random.Range(-1f, 1f));
            newMeteorite.GetComponent<DinoMeteorite>().player = dinoObject;

        }
    }

    private void UpdateDino(Vector2 mousePosition)
    {
        if(!IsGameRunning)
        {
            dinoObject.GetComponent<Animator>().SetBool("Walking", false);
            return;
        }
        if (Vector2.Distance(dinoObject.transform.position, mousePosition) > 0.2f)
        {
            Vector2 dirVector = (mousePosition - (Vector2)dinoObject.transform.position).normalized;
            if (dirVector.x < 0)
            {
                dinoObject.GetComponentInChildren<SpriteRenderer>().flipX = true;
                dinoShadow.flipX = false;

            }
            else
            {
                dinoObject.GetComponentInChildren<SpriteRenderer>().flipX = false;
                dinoShadow.flipX = true;
            }
            dinoObject.transform.Translate(dirVector * Time.deltaTime * dinoSpeed);
            if (!dinoObject.GetComponent<Animator>().GetBool("Walking"))
                dinoObject.GetComponent<Animator>().SetBool("Walking", true);
        }
        else
        {
            dinoObject.GetComponent<Animator>().SetBool("Walking", false);
        }
    }

    private Vector2 GetRandomPositionInsideScreen
    {
        get
        {
            Vector2 screenSize = new Vector2(Screen.width*0.9f, Screen.height*0.8f);
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
        currentGameState = GameState.LOSE;
        EndGame();
    }

    protected override void Win()
    {
        currentGameState = GameState.WIN;
        dinoObject.GetComponent<Animator>().SetBool("Walking", false);
        StartCoroutine(ForceDieCoroutine());
    }
    private IEnumerator ForceDieCoroutine()
    {
        GameObject newMeteorite = Instantiate(meteoritePrefab, this.transform);
        newMeteorite.transform.position = dinoObject.transform.position;
        newMeteorite.GetComponent<DinoMeteorite>().player = dinoObject;
        newMeteorite.GetComponent<DinoMeteorite>().MeteoriteSpeed /= 3f; 
        while (dinoObject.activeInHierarchy)
        {
            yield return null;
        }
        EndGame();
    }
}
