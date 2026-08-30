using UnityEngine;

public class PlayerHit : MonoBehaviour
{
    // Automatically triggered by Unity's 2D physics engine when the bird hits a pipe or the ground
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Global Communication: Reaches out directly to the GameManager singleton
        // and safely triggers the public GameOver routine. 
        // The bird doesn't need to know HOW the game ends, only that it crashed.
        GameManager.Instance.GameOver();
    }
}