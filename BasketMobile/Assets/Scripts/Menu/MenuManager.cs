using UnityEngine;

public class MenuManager : MonoBehaviour
{
    // Component needed to call the new scene when interacting with the buttons in the main page
    public void PlayPractice()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Practice");
    }
    public void PlayChallenge()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Challenge");
    }
}
