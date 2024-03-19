using UnityEngine;
public class GroundSound : MonoBehaviour
{
    // Responsible for playing a sound when the ball hits the ground
    void OnCollisionEnter(Collision collision)
    {
        // Check if the other collider is a ball
        if (collision.gameObject.tag == "Ball")
        {
            AudioManager.instance.PlayMusicByName("Ground");
        }
    }
}
