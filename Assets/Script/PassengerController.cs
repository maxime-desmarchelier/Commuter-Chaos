using System.Collections;
using UnityEngine;
using UnityEngine.AI;

namespace Script
{
    public class PassengerController : MonoBehaviour
    {
        private static readonly int Walking = Animator.StringToHash("Walking");
        private static readonly int Badge = Animator.StringToHash("Badge");
        private static readonly int Jump = Animator.StringToHash("Jump");
        private static readonly int Arrested = Animator.StringToHash("Arrested");

        private Animator _animator;

        private NavMeshAgent _navMeshAgent;
        public bool Fraudster { get; set; }

        public Vector3 Exit { get; set; }

        public float Speed { get; set; } = 3.5f;

        private void Awake(){
            _navMeshAgent = GetComponent<NavMeshAgent>();
            _navMeshAgent.SetDestination(Exit);
            _navMeshAgent.speed = Speed;
            _animator = GetComponent<Animator>();
        }

        private void Update(){
            _animator.SetBool(Walking, _navMeshAgent.hasPath);
        }

        private void OnTriggerEnter(Collider other){
            if (other.CompareTag("Tourniquet"))
            {
                _navMeshAgent.speed = 1.5f;
                _animator.SetTrigger(!Fraudster ? Badge : Jump);
            }
            else if (other.CompareTag("Exit"))
            {
                Destroy(gameObject);
                GameController.Instance.NbPassengerRemaining--;
            }
        }

        private void OnTriggerExit(Collider other){
            _navMeshAgent.speed = 3.5f;
        }

        public void Control(Vector3 controller){
            _navMeshAgent.SetDestination(transform.position);
            StartCoroutine(LookAt(controller));
            _animator.SetBool(Walking, false);
            if (Fraudster)
                _animator.SetTrigger(Arrested);
        }

        private IEnumerator LookAt(Vector3 controllerPosition){
            while (Quaternion.Angle(transform.rotation,
                       Quaternion.LookRotation(controllerPosition - transform.position)) != 0)
            {
                var transformTemp = transform;
                transform.rotation = Quaternion.RotateTowards(transformTemp.rotation,
                    Quaternion.LookRotation((controllerPosition - transformTemp.position).normalized),
                    360f * Time.deltaTime);
                yield return null;
            }
        }

        public void Release(){
            if (Fraudster)
            {
                GameController.Instance.NbPassengerRemaining--;
                GameController.Instance.Score += 1;
                Destroy(gameObject);
            }
            else
            {
                GameController.Instance.Score -= 0.2f;
                _navMeshAgent.SetDestination(Exit);
                _animator.SetBool(Walking, true);
            }
        }
    }
}