using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PolluteMiniGame : MiniGame
{
    [SerializeField] private GameObject powerplantPrefab, treePrefab;
    [SerializeField, Range(0f,1f)] private float treeProbability;

    private List<PowerPlant> spawnedPowerplant;
    private List<PolluteTree> spawnedTrees;

    private void Awake()
    {
        spawnedPowerplant = new List<PowerPlant>();
        spawnedTrees = new List<PolluteTree>();
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
        UpdateSpawn();
        if (IsPowerPlantPollution || IsDeadTree)
            Lose();
    }

    private void UpdateSpawn()
    {
        if (Time.frameCount % (199 - (30 * CurrentDifficulty - 1)) == 0)
        {
            float rand = Random.Range(0f, 1f);
            if (rand < treeProbability)
            {
                GameObject newTree = Instantiate(treePrefab, this.transform);
                newTree.transform.position = GetRandomPositionInsideScreen;
                spawnedTrees.Add(newTree.GetComponent<PolluteTree>());
            }
            else
            {
                SpawnPowerPlant();
            }
        }
    }

    private void SpawnPowerPlant()
    {
        GameObject newPowerplant = Instantiate(powerplantPrefab, this.transform);
        newPowerplant.transform.position = GetRandomPositionInsideScreen;
        spawnedPowerplant.Add(newPowerplant.GetComponent<PowerPlant>());
    }

    private bool IsPowerPlantPollution
    {
        get
        {
            return spawnedPowerplant.FindAll(x => x.IsPolluting  && !x.IsDead ).Count != 0; 
        }
    }

    private bool IsDeadTree
    {
        get
        {
            return spawnedTrees.FindAll(x => x.IsDead).Count != 0;
        }
    }

    protected override void EndGame()
    {
        StartCoroutine(EndGameCoroutine());
    }

    private IEnumerator EndGameCoroutine()
    {
        for (int i = 0; i < 20; ++i)
        {
            SpawnPowerPlant();
            yield return new WaitForSeconds(0.05f);
        }
        foreach (var power in spawnedPowerplant)
        {
            power.GetComponent<ClickableElement>().isActive = false;
            if (!power.IsDead)
                power.IsPolluting = true;
        }
        yield return new WaitForSeconds(1f);
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
        EndGame();
    }



    private Vector2 GetRandomPositionInsideScreen
    {
        get
        {
            Vector2 screenSize = new Vector2(Screen.width * 0.9f, Screen.height * 0.8f);
            Vector2 worldSize = Camera.main.ScreenToWorldPoint(screenSize);
            return new Vector2(Random.Range(-worldSize.x, worldSize.x), Random.Range(-worldSize.y, worldSize.y));
        }
    }
}
