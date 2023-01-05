using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.AI;

public class PassengerController : MonoBehaviour
{
    private NavMeshAgent _navMeshAgent;
    private Animator _animator;
    private static readonly int Walking = Animator.StringToHash("Walking");
    private static readonly int Badge = Animator.StringToHash("Badge");

    // Start is called before the first frame update
    void Start(){
    }

    private void Awake(){
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _navMeshAgent.SetDestination(new Vector3(0.12f, 2.75f, 18.78f));
        _animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update(){
        _animator.SetBool(Walking, _navMeshAgent.hasPath);

        if (transform.position.z > 18.45f)
        {
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter(Collider other){
        if (other.CompareTag("Tourniquet"))
        {
            Debug.Log("Tourniquet");
            _animator.SetTrigger(Badge);
        }
    }
}