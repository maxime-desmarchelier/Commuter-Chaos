using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelEasyController : MonoBehaviour
{
    [SerializeField] private GameObject passengerPrefab;

    [SerializeField] private GameObject trainPrefab;

    private readonly List<TrainController> _trains = new();

    private void Awake(){
        GameController.Instance.Score = 0;
        GameController.Instance.Level = "LEVEL-EASY";
        GameController.Instance.NbPassengerRemaining = 5;

        var train = Instantiate(trainPrefab, new Vector3(-22.95f, 1.064f, -5.7f), Quaternion.identity);
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

        trainController.ExitPosition = new Vector3(44f, 1.064f, -5.7f);
        _trains.Add(trainController);
    }

    // Start is called before the first frame update
    private void Start(){
    }

    // Update is called once per frame
    private void Update(){
        foreach (var train in _trains.Where(train => train.State == "STATION"))
        {
            train.UnloadPassengers(passengerPrefab, 5);
            train.State = "UNLOADING";
        }

        foreach (var train in _trains.Where(train => train.State == "UNLOADED"))
        {
            train.MoveAway();
            train.State = "LEFT";
        }

        if (_trains.Any(train => train.PassengerCount != 0))
        {
            return;
        }

        if (GameController.Instance.NbPassengerRemaining == 0)
        {
            SceneManager.LoadScene("Scoreboard");
        }
    }
}