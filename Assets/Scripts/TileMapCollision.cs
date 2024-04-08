using UnityEngine;
using UnityEngine.SceneManagement;

public class TileMapCollision : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D coll)
    {
        Debug.Log("Collision detected with " + coll.name);
        // Check if it's the snake
        if (coll.name.StartsWith("Snake"))
        {
            // Trigger game over from the GameManager
            FindObjectOfType<GameManager>().GameOver();
        }
    }
}