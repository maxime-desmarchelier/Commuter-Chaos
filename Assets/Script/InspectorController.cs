using System.Collections;
using UnityEngine;
using UnityEngine.AI;

namespace Script
{
    public class InspectorController : MonoBehaviour
    {
        private static readonly int Walking = Animator.StringToHash("Walking");
        private Animator _animator;

        private bool _controlInProcess;
        private Camera _mainCamera;
        private NavMeshAgent _navMeshAgent;

        private void Awake(){
            _mainCamera = Camera.main;
            _navMeshAgent = GetComponent<NavMeshAgent>();
            _animator = GetComponent<Animator>();
        }

        private void Update(){
            // Si un contrôle est en cours, on ne fait rien 
            if (_controlInProcess) return;

            _animator.SetBool(Walking, _navMeshAgent.hasPath);

            if (Input.GetMouseButtonDown(1))
            {
                // On déplace le contrôleur vers le point cliqué
                var ray = _mainCamera.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out var enter, 1000)) _navMeshAgent.SetDestination(enter.point);
            }
            else if (Input.GetMouseButtonDown(0))
            {
                // On déplace le contrôleur vers le point cliqué
                var ray = _mainCamera.ScreenPointToRay(Input.mousePosition);
                if (!Physics.Raycast(ray, out var hit)) return;
                if (!hit.transform.CompareTag("Passenger")) return;
                // Si on a cliqué sur un passager, on le contrôle
                if (Vector3.Distance(transform.position, hit.point) < 3f)
                {
                    var passenger = hit.transform.gameObject;
                    StartCoroutine(ControlCoroutine(passenger));
                }
                else
                {
                    _navMeshAgent.SetDestination(hit.point);
                }
            }
        }

        private IEnumerator ControlCoroutine(GameObject passenger){
            _controlInProcess = true;
            var position = transform.position;
            _navMeshAgent.SetDestination(position);
            _animator.SetBool(Walking, false);

            var passengerController = passenger.GetComponent<PassengerController>();
            passengerController.Control(position);
            // On tourne le controleur vers le passager
            StartCoroutine(LookAt(passenger.transform.position));
            // On simule une durée de contrôle
            yield return new WaitForSeconds(1f);
            // On libère le passager
            passengerController.Release();
            _controlInProcess = false;
        }

        private IEnumerator LookAt(Vector3 controllerPosition){
            while (Quaternion.Angle(transform.rotation,
                       Quaternion.LookRotation(controllerPosition - transform.position)) > 10f)
            {
                var transform1 = transform;
                transform.rotation = Quaternion.RotateTowards(transform1.rotation,
                    Quaternion.LookRotation((controllerPosition - transform1.position).normalized),
                    360f * Time.deltaTime);
                yield return null;
            }
        }
    }
}