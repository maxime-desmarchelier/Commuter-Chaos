using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

namespace Script
{
    public class LevelMediumController : MonoBehaviour
    {
        [SerializeField] private GameObject passengerPrefab;

        [SerializeField] private GameObject greenTrainPrefab;
        [SerializeField] private GameObject yellowTrainPrefab;

        private readonly List<TrainController> _trains = new();

        private void Awake(){
            GameController.Instance.Score = 0;
            GameController.Instance.Level = "LEVEL-MEDIUM";
            GameController.Instance.NbPassengerRemaining = 10;
            GameController.Instance.NbFraudster = 5;

            // On récupère les sorties
            var objList = GameObject.FindGameObjectsWithTag("Exit");
            var exitPositions = objList.Select(obj => obj.transform.position).ToList();

            // On créer les trains
            var greenTrain = Instantiate(greenTrainPrefab, new Vector3(40.3f, 1.30f, 36.24f), Quaternion.identity);
            var greenTrainController = greenTrain.GetComponent<TrainController>();
            var yellowTrain = Instantiate(yellowTrainPrefab, new Vector3(-29.6f, 1.30f, -4.335f), Quaternion.identity);
            var yellowTrainController = yellowTrain.GetComponent<TrainController>();

            // On génère la liste des passagers
            var passengerList = GeneratePassengerList(exitPositions, Quaternion.identity,
                GameController.Instance.NbPassengerRemaining, GameController.Instance.NbFraudster);

            greenTrain.SetActive(true);
            greenTrainController.MoveToStation(new Vector3(2.98f, 1.30f, 36.24f), Random.Range(0f, 1.5f));

            yellowTrain.SetActive(true);
            yellowTrainController.MoveToStation(new Vector3(7.9f, 1.30f, -4.335f), Random.Range(0f, 1.5f));

            // On ajoute les passagers au train
            var rand = Random.Range(0, passengerList.Count);
            greenTrainController.PassengerList = passengerList.GetRange(0, rand);
            passengerList.RemoveRange(0, rand);
            yellowTrainController.PassengerList = passengerList;

            greenTrainController.ExitPosition = new Vector3(-37f, 1.30f, 36.24f);
            yellowTrainController.ExitPosition = new Vector3(48.73f, 1.30f, -4.335f);
            _trains.Add(greenTrainController);
            _trains.Add(yellowTrainController);
        }

        // Update is called once per frame
        private void Update(){
            foreach (var train in _trains.Where(train => train.State == "STATION")) train.UnloadPassengers();

            foreach (var train in _trains.Where(train => train.State == "UNLOADED")) train.MoveAway();

            Debug.Log(GameController.Instance.NbPassengerRemaining);

            if (GameController.Instance.NbPassengerRemaining == 0) SceneManager.LoadScene("Scoreboard");
        }

        private List<GameObject> GeneratePassengerList(
            IReadOnlyList<Vector3> exitPositionList,
            Quaternion passengerRotation,
            int nbPassenger, int nbFraudster){
            var passengerList = new List<GameObject>();

            for (var i = 0; i < nbPassenger; i++)
            {
                var passenger = Instantiate(passengerPrefab,
                    new Vector3(0, 0, 0), passengerRotation);
                var passengerController = passenger.GetComponent<PassengerController>();
                passengerController.Fraudster = i < nbFraudster;
                passengerController.Exit = exitPositionList[Random.Range(0, exitPositionList.Count)];
                Debug.Log(passengerController.Exit);
                passengerList.Add(passenger);
            }

            passengerList = passengerList.OrderBy(_ => Guid.NewGuid()).ToList();

            return passengerList;
        }
    }
}