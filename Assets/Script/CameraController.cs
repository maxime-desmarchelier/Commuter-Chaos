using UnityEngine;

namespace Script
{
    public class CameraController : MonoBehaviour
    {
        private Camera _mainCamera;

        private void Awake(){
            _mainCamera = Camera.main;
        }

        // Start is called before the first frame update
        private void Start(){
        }

        // Update is called once per frame
        private void Update(){
            if (Input.mousePosition.x >= Screen.width - 1)
                _mainCamera.transform.Translate(Vector3.right * (Time.deltaTime * 10));
            else if (Input.mousePosition.x <= 0)
                _mainCamera.transform.Translate(Vector3.left * (Time.deltaTime * 10));
            else if (Input.mousePosition.y >= Screen.height - 1)
                _mainCamera.transform.Translate(-_mainCamera.transform.forward * (Time.deltaTime * 10));
            else if (Input.mousePosition.y <= 0)
                _mainCamera.transform.Translate(_mainCamera.transform.forward * (Time.deltaTime * 10));
        }
    }
}