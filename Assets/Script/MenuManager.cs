using UnityEngine;
using UnityEngine.SceneManagement;

namespace Script
{
    public class MenuManager : MonoBehaviour
    {
        public void PlayEasy(){
            SceneManager.LoadScene("LEVEL-EASY");
        }

        public void PlayMedium(){
            SceneManager.LoadScene("LEVEL-MEDIUM");
        }

        public void PlayHard(){
            SceneManager.LoadScene("LEVEL-HARD");
        }
    }
}