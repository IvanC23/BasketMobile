using UnityEngine;

public class MenuManager : MonoBehaviour
{
    public void PlayPractice()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Practice");
    }
    public void PlayChallenge()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Challenge");
    }
}
