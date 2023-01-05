using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class InspectorController : MonoBehaviour
{
    private Camera _mainCamera;
    private NavMeshAgent _navMeshAgent;

    // Start is called before the first frame update
    void Start(){
    }

    private void Awake(){
        _mainCamera = Camera.main;
        _navMeshAgent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update(){
        if (Input.GetMouseButtonDown(1))
        {
            var ray = _mainCamera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out var enter, 1000))
            {
                _navMeshAgent.SetDestination(enter.point);
            }
        }
    }
}