using UnityEngine;

public class ScoreTrigger : MonoBehaviour
{
    private bool hasScored = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (hasScored) return;

        if (collision.GetComponent<PlayerController>() != null)
        {
            hasScored = true;
            GameManager.Instance.AddScore();
        }
    }
}
