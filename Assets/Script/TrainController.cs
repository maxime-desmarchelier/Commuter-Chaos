using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Script
{
    public class TrainController : MonoBehaviour
    {
        public string State { get; set; }
        public List<GameObject> PassengerList { get; set; }
        public Vector3 ExitPosition { get; set; }

        private IEnumerator MoveCoroutine(Vector3 stationPosition, bool destroy){
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

        public void MoveToStation(Vector3 stationPosition){
            StartCoroutine(MoveCoroutine(stationPosition, false));
        }

        private IEnumerator SpawnPassenger(){
            foreach (var passenger in PassengerList)
            {
                passenger.SetActive(true);
                yield return new WaitForSeconds(2f);
            }

            State = "UNLOADED";
        }

        public void UnloadPassengers(){
            StartCoroutine(SpawnPassenger());
            State = "UNLOADING";
        }

        public void MoveAway(){
            StartCoroutine(MoveCoroutine(ExitPosition, true));
            State = "LEFT";
        }
    }
}