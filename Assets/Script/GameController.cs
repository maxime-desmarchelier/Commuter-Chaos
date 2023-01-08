using System.Collections.Generic;
using UnityEngine;

namespace Script
{
    public class GameController : MonoBehaviour
    {
        private readonly List<string> _levels = new() { "LEVEL-EASY", "LEVEL-MEDIUM", "LEVEL-HARD" };
        public static GameController Instance { get; private set; }

        public int Score { get; set; }
        public string Level { get; set; }

        public int NbPassengerRemaining { get; set; }
        public int NbFraudster { get; set; }

        private void Awake(){
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;

            DontDestroyOnLoad(gameObject);
        }

        public string GetNextLevel(){
            return _levels.IndexOf(Level) + 1 >= _levels.Count ? null : _levels[_levels.IndexOf(Level) + 1];
        }
    }
}