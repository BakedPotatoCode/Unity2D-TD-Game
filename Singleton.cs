using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;

public class Singleton : MonoBehaviour
{
    public static Singleton Instance;

    // Events to notify other scripts when the variables change
    public UnityEvent<int> OnVariableEnemyChange = new UnityEvent<int>();
    public UnityEvent<int> OnVariableCoinsChange = new UnityEvent<int>();
    public UnityEvent<int> OnVariableHealthChange = new UnityEvent<int>();

    private int _enemiesRemaining;
    private int coinsWealth;
    private int health;

    public int EnemiesRemaining
    {
        get { return _enemiesRemaining; }
        set
        {
            _enemiesRemaining = value;
            OnVariableEnemyChange.Invoke(value); // Notify listeners
        }
    }

    public int HealthRemaining
    {
        get { return health; }
        set
        {
            health = value;
            OnVariableHealthChange.Invoke(value); // Notify listeners
        }
    }

    public int CoinsInPocket
    {
        get { return coinsWealth; }
        set
        {
            coinsWealth = value;
            OnVariableCoinsChange.Invoke(value); // Notify listeners
        }
    }
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

}
