using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    // Start is called before the first frame update
    private void Start(){
    }

    // Update is called once per frame
    private void Update(){
    }

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