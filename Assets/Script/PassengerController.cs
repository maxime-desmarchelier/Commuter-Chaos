using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class PassengerController : MonoBehaviour
{
    private static readonly int Walking = Animator.StringToHash("Walking");
    private static readonly int Badge = Animator.StringToHash("Badge");
    private static readonly int Jump = Animator.StringToHash("Jump");
    private Animator _animator;

    private readonly Vector3 _Exit = new(0.12f, 2.75f, 18f);

    private readonly bool _isFraudster = true;
    private NavMeshAgent _navMeshAgent;
    private static readonly int Arrested = Animator.StringToHash("Arrested");

    private void Awake(){
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _navMeshAgent.SetDestination(_Exit);
        _animator = GetComponent<Animator>();
    }

    // Start is called before the first frame update
    private void Start(){
    }

    // Update is called once per frame
    private void Update(){
        _animator.SetBool(Walking, _navMeshAgent.hasPath);

        if (!(transform.position.z > _Exit.z - 0.5f)) return;
        Destroy(gameObject);
        GameController.Instance.NbPassengerRemaining--;
    }

    private void OnTriggerEnter(Collider other){
        if (other.CompareTag("Tourniquet"))
        {
            _navMeshAgent.speed = 1.5f;
            _animator.SetTrigger(!_isFraudster ? Badge : Jump);
        }
    }

    private void OnTriggerExit(Collider other){
        _navMeshAgent.speed = 3.5f;
    }

    public void Control(Vector3 controller){
        _navMeshAgent.SetDestination(transform.position);
        StartCoroutine(LookAt(controller));
        _animator.SetBool(Walking, false);
        if (_isFraudster)
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
        if (_isFraudster)
        {
            GameController.Instance.NbPassengerRemaining--;
            Destroy(gameObject);
        }
        else
        {
            _navMeshAgent.SetDestination(_Exit);
            _animator.SetBool(Walking, true);
        }
    }
}