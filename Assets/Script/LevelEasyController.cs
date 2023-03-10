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
            // On réinitialise les variables de la scène précédente
            GameController.Instance.Score = 0;
            GameController.Instance.Level = "LEVEL-EASY";

            // On définit le nombre de passager et de fraudeur
            GameController.Instance.NbPassengerRemaining = 10;
            GameController.Instance.NbFraudster = 5;

            // On créee le train
            var train = Instantiate(trainPrefab, new Vector3(-22.95f, 1.064f, 3.86f), Quaternion.identity);
            train.SetActive(true);
            var trainController = train.GetComponent<TrainController>();

            // On fait bouger le train vers la station
            trainController.MoveToStation(new Vector3(5f, 1.064f, 3.86f));

            var objList = GameObject.FindGameObjectsWithTag("Exit");
            var exitPositions = objList.Select(obj => obj.transform.position).ToList();

            trainController.PassengerList = GeneratePassengerList(exitPositions,
                GameController.Instance.NbPassengerRemaining,
                GameController.Instance.NbFraudster);

            trainController.ExitPosition = new Vector3(44f, 1.064f, 3.86f);
            _trains.Add(trainController);
        }

        // Update is called once per frame
        private void Update(){
            foreach (var train in _trains.Where(train => train.State == "STATION")) train.UnloadPassengers();

            foreach (var train in _trains.Where(train => train.State == "UNLOADED")) train.MoveAway();

            if (GameController.Instance.NbPassengerRemaining == 0) SceneManager.LoadScene("Scoreboard");
        }


        private List<GameObject> GeneratePassengerList(
            IReadOnlyList<Vector3> exitPositionList,
            int nbPassenger, int nbFraudster){
            var passengerList = new List<GameObject>();

            for (var i = 0; i < nbPassenger; i++)
            {
                var passenger = Instantiate(passengerPrefab,
                    new Vector3(0, 0, 0), Quaternion.identity);
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