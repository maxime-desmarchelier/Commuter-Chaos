using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.Animations;
using UnityEngine;

public class LevelEasyController : MonoBehaviour
{
    [SerializeField] private GameObject passengerPrefab;

    [SerializeField] private GameObject trainPrefab;

    private List<TrainController> _trains = new();

    // Start is called before the first frame update
    void Start(){
    }

    private void Awake(){
        GameObject train = Instantiate(trainPrefab, new Vector3(-22.95f, 1.064f, -5.7f), Quaternion.identity);
        train.SetActive(true);
        var trainController = train.GetComponent<TrainController>();
        trainController.MoveToStation(new Vector3(9.55f, 1.064f, -5.7f));
        trainController.PassengerSpawnList = new List<Vector3>
        {
            new(12.87f, 0, -7.63f),
            new(6.08f, 0, -7.63f),
            new(3.2f, 0, -7.63f),
            new(-3.603f, 0, -7.63f),
            new(-6.491f, 0, -7.63f),
            new(-13.292f, 0, -7.63f)
        };

        _trains.Add(trainController);
    }

    // Update is called once per frame
    void Update(){
        foreach (var train in _trains.Where(train => train.State == "STATION"))
        {
            train.UnloadPassengers(passengerPrefab, 5);
            train.State = "UNLOADING";
        }
    }
}