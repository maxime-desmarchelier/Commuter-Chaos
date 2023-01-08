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

            var objList = GameObject.FindGameObjectsWithTag("SpawnPosition");
            var spawnPositions = objList.Select(obj => obj.transform.position).ToList();

            objList = GameObject.FindGameObjectsWithTag("Exit");
            var exitPositions = objList.Select(obj => obj.transform.position).ToList();

            trainController.PassengerList = GeneratePassengerList(spawnPositions,
                exitPositions, Quaternion.identity, GameController.Instance.NbPassengerRemaining,
                GameController.Instance.NbFraudster);

            trainController.ExitPosition = new Vector3(44f, 1.064f, 3.86f);
            _trains.Add(trainController);
        }

        // Update is called once per frame
        private void Update(){
            foreach (var train in _trains.Where(train => train.State == "STATION"))
            {
                train.UnloadPassengers();
            }

            foreach (var train in _trains.Where(train => train.State == "UNLOADED"))
            {
                train.MoveAway();
            }

            if (GameController.Instance.NbPassengerRemaining == 0) SceneManager.LoadScene("Scoreboard");
        }

        private List<GameObject> GeneratePassengerList(IReadOnlyList<Vector3> spawnPositionList,
            IReadOnlyList<Vector3> exitPositionList,
            Quaternion passengerRotation,
            int nbPassenger, int nbFraudster){
            var passengerList = new List<GameObject>();

            for (var i = 0; i < nbPassenger; i++)
            {
                var passenger = Instantiate(passengerPrefab,
                    spawnPositionList[Random.Range(0, spawnPositionList.Count)], passengerRotation);
                var passengerController = passenger.GetComponent<PassengerController>();
                passengerController.Fraudster = i < nbFraudster;
                passengerController.Exit = exitPositionList[Random.Range(0, exitPositionList.Count)];
                passengerList.Add(passenger);
            }

            passengerList = passengerList.OrderBy(_ => Guid.NewGuid()).ToList();

            return passengerList;
        }
    }
}