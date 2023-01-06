using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public static GameController Instance { get; private set; }

    public int Score { get; set; }
    public string Level { get; set; }

    public int NbPassengerRemaining { get; set; }

    private List<string> _levels = new List<string> { "LEVEL-EASY", "LEVEL-MEDIUM", "LEVEL-HARD" };

    public string GetNextLevel(){
        return _levels.IndexOf(Level) + 1 >= _levels.Count ? null : _levels[_levels.IndexOf(Level) + 1];
    }

    private void Awake(){
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        DontDestroyOnLoad(gameObject);

        // Initialisation du Game Manager...
    }
}