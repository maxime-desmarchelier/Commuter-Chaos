using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class InspectorController : MonoBehaviour
{
    private Camera _mainCamera;
    private NavMeshAgent _navMeshAgent;
    private Animator _animator;
    private static readonly int Walking = Animator.StringToHash("Walking");

    // Start is called before the first frame update
    void Start(){
    }

    private void Awake(){
        _mainCamera = Camera.main;
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update(){
        _animator.SetBool(Walking, _navMeshAgent.hasPath);
        if (Input.GetMouseButtonDown(1))
        {
            var ray = _mainCamera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out var enter, 1000))
            {
                _navMeshAgent.SetDestination(enter.point);
            }
        }
        else if (Input.GetMouseButtonDown(0))
        {
            var ray = _mainCamera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out var hit))
            {
                if (hit.transform.CompareTag("Passenger") && Vector3.Distance(transform.position, hit.point) < 5)
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
    }

    IEnumerator ControlCoroutine(GameObject passenger){
        var passengerController = passenger.GetComponent<PassengerController>();
        passengerController.Control(transform.position);
        yield return new WaitForSeconds(2f);
        passengerController.Release();
    }
}