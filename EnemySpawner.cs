using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class EnemySpawner : MonoBehaviour
{
    public Transform spawnPoint;
    public Text roundText;
    public Button roundPlayer;
    public GameObject winScreen;
    public Text coinsText;

    private int currentRound = 0;
    private int enemiesRemaining;

    //Enemies References
    public GameObject enemyType1;
    public GameObject enemyType2;
    public GameObject enemyType3;
    private List<int> numberOfEnemiesList = new List<int> { 0, 15, 20, 25, 25, 30, 20, 40, 45, 50 };

    private List<List<int>> roundNumbers = new List<List<int>> {
        new List<int> { 1, 1, 1, 1, 1, 1, 1, 2, 2, 2, 2, 1, 1, 1, 1 },
        new List<int> { 1, 1, 1, 1, 1, 1, 1, 2, 2, 2, 2, 1, 1, 1, 2, 2, 2, 2, 2, 2 },
        new List<int> { 2, 2, 1, 3, 3, 1, 1, 2, 2, 2, 2, 1, 1, 1, 3, 3, 3, 3, 3, 3, 2, 2, 2, 2, 3 }
    };


    private void Start()
    {
        if (Singleton.Instance != null)
        {
            Singleton.Instance.OnVariableEnemyChange.AddListener(OnVariableEnemyChange);
            Singleton.Instance.CoinsInPocket = 200;
        }
    }

    private void Update()
    {
        if (coinsText != null)
        {
            coinsText.text = "Coins: " + Singleton.Instance.CoinsInPocket.ToString();
        }
    }

    private void OnVariableEnemyChange(int newValue)
    {
        enemiesRemaining = newValue;

        if (enemiesRemaining <= 0)
        {
            CheckRoundCompletion();
        }
    }

    public void HandleEnemyDeath(GameObject source)
    {
        Singleton.Instance.EnemiesRemaining--;
    }

    private void CheckRoundCompletion()
    {
        if (currentRound < 3)
        {
            roundPlayer.interactable = true;
            Singleton.Instance.CoinsInPocket += 100;
        }
        else
        {
            winScreen.SetActive(true);
        }
    }

    private IEnumerator SpawnEnemies(int numberOfEnemies)
    {
        Singleton.Instance.EnemiesRemaining = numberOfEnemies;

        List<int> currentRoundNumbers = roundNumbers[currentRound];

        for (int i = 0; i < numberOfEnemies; i++)
        {
            int enemyType = currentRoundNumbers[i];
            GameObject enemy = InstantiateEnemy(enemyType);

            if (enemy == null)
            {
                Debug.LogError($"[SpawnEnemies] enemy is null for enemyType {enemyType} at iteration {i}");
            }
            else
            {
                Enemy enemyScript = enemy.GetComponent<Enemy>();

                if (enemyScript == null)
                {
                    Debug.LogError($"[SpawnEnemies] enemyScript is null for enemyType {enemyType} at iteration {i}");
                }
                else
                {
                    enemyScript.OnEnemyDeath += HandleEnemyDeath;
                }
            }

            float spawnDelay = (currentRound == 1) ? 0.8f : 1f;
            yield return new WaitForSeconds(spawnDelay);
        }
    }

    private GameObject InstantiateEnemy(int enemyType)
    {
        switch (enemyType)
        {
            case 1:
                return Instantiate(enemyType1, spawnPoint.position, Quaternion.identity);
            case 2:
                return Instantiate(enemyType2, spawnPoint.position, Quaternion.identity);
            case 3:
                return Instantiate(enemyType3, spawnPoint.position, Quaternion.identity);
            default:
                Debug.LogWarning($"Unknown enemyType: {enemyType}. Instantiating a default enemy.");
                return Instantiate(enemyType1, spawnPoint.position, Quaternion.identity);
        }
    }

    public void StartNextRound()
    {
        currentRound++;
        int numberOfEnemies = numberOfEnemiesList[currentRound];
        StartCoroutine(SpawnEnemies(numberOfEnemies));

        if (roundText != null)
        {
            roundText.text = "Round: " + currentRound.ToString();
        }

        roundPlayer.interactable = false;
    }
}
