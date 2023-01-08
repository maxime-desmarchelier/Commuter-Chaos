using System.Collections.Generic;
using System.Linq;
using Script;
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

    private List<GameObject> GeneratePassengerList(List<Vector3> spawnPositionList, Quaternion passengerRotation,
        int nbPassenger, int nbFraudster){
        var passengerList = new List<GameObject>();

        for (int i = 0; i < nbPassenger; i++)
        {
            int randomIndex = Random.Range(0, spawnPositionList.Count);
            var passenger = Instantiate(passengerPrefab, spawnPositionList[randomIndex], passengerRotation);
            var passengerController = passenger.GetComponent<PassengerController>();
            passengerController.Fraudster = true;
            passengerList.Add(passenger);
        }

        return passengerList;
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

        Debug.Log(GameController.Instance.NbPassengerRemaining);

        if (GameController.Instance.NbPassengerRemaining == 0) SceneManager.LoadScene("Scoreboard");
    }
}