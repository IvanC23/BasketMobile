using UnityEngine;

public class MenuManager : MonoBehaviour
{
    // Component needed to load the new scene when interacting with the buttons in the main page
    public void PlayPractice()
    {
        AudioManager.instance.PlayMusicByName("Click");
        UnityEngine.SceneManagement.SceneManager.LoadScene("Practice");
    }
    public void PlayChallenge()
    {
        AudioManager.instance.PlayMusicByName("Click");
        UnityEngine.SceneManagement.SceneManager.LoadScene("Challenge");
    }
}
