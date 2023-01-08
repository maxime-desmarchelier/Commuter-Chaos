using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Script
{
    public class TrainController : MonoBehaviour
    {
        public string State { get; set; }
        public List<GameObject> PassengerList { get; set; }
        public Vector3 ExitPosition { get; set; }

        private IEnumerator MoveCoroutine(Vector3 stationPosition, bool destroy, float delay){
            yield return new WaitForSeconds(delay);
            State = "MOVING";
            while (Vector3.Distance(transform.position, stationPosition) > 0.1f)
            {
                transform.position = Vector3.MoveTowards(transform.position, stationPosition, 0.01f);
                yield return null;
            }

            if (destroy)
                Destroy(gameObject);
            else
                State = "STATION";
        }

        public void MoveToStation(Vector3 stationPosition, float delay = 0f){
            StartCoroutine(MoveCoroutine(stationPosition, false, delay));
        }


        private IEnumerator SpawnPassenger(){
            var spawnPositions = FindGameObjectsWithTagInChildren(gameObject, "DoorPosition");
            foreach (var passenger in PassengerList)
            {
                var rand = Random.Range(0, spawnPositions.Count);
                passenger.transform.position = spawnPositions[rand].position;
                passenger.transform.rotation = spawnPositions[rand].rotation;

                passenger.SetActive(true);
                yield return new WaitForSeconds(2f);
            }

            State = "UNLOADED";
        }

        private List<(Vector3 position, Quaternion rotation)> FindGameObjectsWithTagInChildren(GameObject go,
            string stringTag){
            return (from Transform child in go.transform
                    where child.gameObject.CompareTag(stringTag)
                    select (child.position, child.rotation))
                .ToList();
        }

        public void UnloadPassengers(){
            StartCoroutine(SpawnPassenger());
            State = "UNLOADING";
        }

        public void MoveAway(){
            StartCoroutine(MoveCoroutine(ExitPosition, true, 0));
            State = "LEFT";
        }
    }
}