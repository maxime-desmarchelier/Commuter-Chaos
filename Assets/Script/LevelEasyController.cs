using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

namespace Script
{
    public class LevelEasyController : MonoBehaviour
    {
        [SerializeField] private GameObject passengerPrefab;

        [SerializeField] private GameObject trainPrefab;

        private readonly List<TrainController> _trains = new();

        private void Awake(){
            GameController.Instance.Score = 0;
            GameController.Instance.Level = "LEVEL-EASY";
            GameController.Instance.NbPassengerRemaining = 10;
            GameController.Instance.NbFraudster = 5;

            var train = Instantiate(trainPrefab, new Vector3(-22.95f, 1.064f, 3.86f), Quaternion.identity);
            train.SetActive(true);
            var trainController = train.GetComponent<TrainController>();
            trainController.MoveToStation(new Vector3(5f, 1.064f, 3.86f));

            trainController.PassengerList = GeneratePassengerList(new List<Vector3>
            {
                new(12.87f, 0, -7.63f),
                new(6.08f, 0, -7.63f),
                new(3.2f, 0, -7.63f),
                new(-3.603f, 0, -7.63f),
                new(-6.491f, 0, -7.63f),
                new(-13.292f, 0, -7.63f)
            }, Quaternion.identity, GameController.Instance.NbPassengerRemaining, GameController.Instance.NbFraudster);

            trainController.ExitPosition = new Vector3(44f, 1.064f, 3.86f);
            _trains.Add(trainController);
        }

        // Start is called before the first frame update
        private void Start(){
        }

        // Update is called once per frame
        private void Update(){
            foreach (var train in _trains.Where(train => train.State == "STATION"))
            {
                train.UnloadPassengers(passengerPrefab, GameController.Instance.NbPassengerRemaining,
                    GameController.Instance.NbFraudster);
                train.State = "UNLOADING";
            }

            foreach (var train in _trains.Where(train => train.State == "UNLOADED"))
            {
                train.MoveAway();
                train.State = "LEFT";
            }

            if (GameController.Instance.NbPassengerRemaining == 0) SceneManager.LoadScene("Scoreboard");
        }

        private List<GameObject> GeneratePassengerList(IReadOnlyList<Vector3> spawnPositionList,
            Quaternion passengerRotation,
            int nbPassenger, int nbFraudster){
            var passengerList = new List<GameObject>();

            for (var i = 0; i < nbPassenger; i++)
            {
                var randomIndex = Random.Range(0, spawnPositionList.Count);
                var passenger = Instantiate(passengerPrefab, spawnPositionList[randomIndex], passengerRotation);
                var passengerController = passenger.GetComponent<PassengerController>();
                passengerController.Fraudster = i < nbFraudster;
                passengerList.Add(passenger);
            }

            passengerList = passengerList.OrderBy(i => Guid.NewGuid()).ToList();

            return passengerList;
        }
    }
}