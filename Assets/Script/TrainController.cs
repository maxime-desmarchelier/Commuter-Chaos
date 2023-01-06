using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrainController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start(){
    }

    // Update is called once per frame
    void Update(){
    }

    IEnumerator MoveCoroutine(Vector3 stationPosition, bool destroy){
        State = "MOVING";
        while (Vector3.Distance(transform.position, stationPosition) > 0.1f)
        {
            transform.position = Vector3.MoveTowards(transform.position, stationPosition, 0.01f);
            yield return null;
        }

        if (destroy)
        {
            Destroy(gameObject);
        }
        else
        {
            State = "STATION";
        }
    }

    public string State { get; set; }
    public List<Vector3> PassengerSpawnList { get; set; }

    public void MoveToStation(Vector3 stationPosition){
        StartCoroutine(MoveCoroutine(stationPosition, false));
    }

    IEnumerator SpawnPassenger(GameObject passengerPrefab, int number){
        while (number > 0)
        {
            number--;
            yield return new WaitForSeconds(2f);
            // Get random element from PassengerSpawnList
            Vector3 spawnPosition = PassengerSpawnList[Random.Range(0, PassengerSpawnList.Count)];
            var passenger = Instantiate(passengerPrefab, spawnPosition, Quaternion.identity);
            passenger.SetActive(true);
        }

        State = "UNLOADED";
    }

    public void UnloadPassengers(GameObject passengerPrefab, int passengerNumber){
        StartCoroutine(SpawnPassenger(passengerPrefab, passengerNumber));
    }
}