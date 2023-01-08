using System.Collections.Generic;
using UnityEngine;

namespace Script
{
    public class GameController : MonoBehaviour
    {
        // On définit 3 niveaux de difficulté
        private readonly List<string> _levels = new() { "LEVEL-EASY", "LEVEL-MEDIUM", "LEVEL-HARD" };
        public static GameController Instance { get; private set; }

        public float Score { get; set; }
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
            // On retourne le nom du niveau suivant en fonction du niveau actuel
            return _levels.IndexOf(Level) + 1 >= _levels.Count ? null : _levels[_levels.IndexOf(Level) + 1];
        }
    }
}