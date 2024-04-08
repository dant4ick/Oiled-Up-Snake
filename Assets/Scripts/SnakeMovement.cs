using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnakeMovement : MonoBehaviour
{
    private float speed = 0.45f;
    private Vector2 direction = Vector2.up;
    private Vector2 selectedDirection = Vector2.up;
    private List<GameObject> body = new List<GameObject>();
    [SerializeField] private Sprite bodySprite;
    [SerializeField] private Sprite bodyTurnRightSprite;
    [SerializeField] private Sprite bodyTurnLeftSprite;
    [SerializeField] private Sprite tailSprite;
    private GameObject bodyGameObject;
    private GameObject tailGameObject;
    private bool ateFood = false;
    
    public List<GameObject> GetBody()
    {
        return body;
    }

    void Start()
    {
        // Add Rigidbody2D and set it to kinematic
        Rigidbody2D rb = gameObject.AddComponent<Rigidbody2D>();
        rb.isKinematic = true;

        // Create initial body and tail
        bodyGameObject = new GameObject("BodyPart");
        bodyGameObject.AddComponent<SpriteRenderer>().sprite = bodySprite;
        bodyGameObject.transform.position = new Vector2(transform.position.x, transform.position.y) - direction;
        
        tailGameObject = new GameObject("Tail");
        tailGameObject.AddComponent<SpriteRenderer>().sprite = tailSprite;
        tailGameObject.transform.position = new Vector2(transform.position.x, transform.position.y) - direction * 2;
        
        // Add body and tail to the body list
        body.Add(gameObject);
        body.Add(bodyGameObject);
        body.Add(tailGameObject);
        
        // Iterate through the body list to add box colliders and set their sizes
        for (int i = 0; i < body.Count; i++)
        {
            BoxCollider2D collider = body[i].AddComponent<BoxCollider2D>();
            collider.size = new Vector2(0.8f, 0.8f);
            collider.isTrigger = true;
        }

        ApplySpriteDirection(direction);
    }

    void MoveSnake()
    {
        direction = selectedDirection;
        // Move the snake by iterating through the body list that includes the head, body, and tail
        Vector2 previousPosition = new Vector2(transform.position.x, transform.position.y);
        transform.position = new Vector2(transform.position.x + direction.x, transform.position.y + direction.y);
        if (ateFood)
        {
            GameObject newBodyGameObject = Instantiate(bodyGameObject, previousPosition, Quaternion.identity);
            body.Insert(1, newBodyGameObject);
            ateFood = false;
        }
        else
        {
            for (int i = 1; i < body.Count; i++)
            {
                Vector2 temp = new Vector2(body[i].transform.position.x, body[i].transform.position.y);
                body[i].transform.position = previousPosition;
                previousPosition = temp;
            }
        }
        
        ApplySpriteDirection(direction);
    }

    private void ApplySpriteDirection(Vector2 direction)
    {
        ApplyRotation(gameObject, direction);
    
        for (int i = 1; i < body.Count - 1; i++)
        {
            Vector2 previousDirection = (body[i - 1].transform.position - body[i].transform.position).normalized;
            Vector2 nextDirection = (body[i + 1].transform.position - body[i].transform.position).normalized;
        
            // Determine the sprite based on the current and next directions
            Sprite spriteToApply = bodySprite;
            if (nextDirection == Vector2.up && previousDirection == Vector2.right)
                spriteToApply = bodyTurnRightSprite;
            else if (nextDirection == Vector2.up && previousDirection == Vector2.left)
                spriteToApply = bodyTurnLeftSprite;
            else if (nextDirection == Vector2.right && previousDirection == Vector2.up)
                spriteToApply = bodyTurnLeftSprite;
            else if (nextDirection == Vector2.right && previousDirection == Vector2.down)
                spriteToApply = bodyTurnRightSprite;
            else if (nextDirection == Vector2.down && previousDirection == Vector2.right)
                spriteToApply = bodyTurnLeftSprite;
            else if (nextDirection == Vector2.down && previousDirection == Vector2.left)
                spriteToApply = bodyTurnRightSprite;
            else if (nextDirection == Vector2.left && previousDirection == Vector2.up)
                spriteToApply = bodyTurnRightSprite;
            else if (nextDirection == Vector2.left && previousDirection == Vector2.down)
                spriteToApply = bodyTurnLeftSprite;
        
            body[i].GetComponent<SpriteRenderer>().sprite = spriteToApply;
            ApplyRotation(body[i], nextDirection);
        }
        
        // flip tail x axis half the time
        if (Random.Range(0, 2) == 0)
            tailGameObject.GetComponent<SpriteRenderer>().flipX = true;
        else
            tailGameObject.GetComponent<SpriteRenderer>().flipX = false;
        ApplyRotation(body[body.Count - 1], (body[body.Count - 2].transform.position - body[body.Count - 1].transform.position).normalized);
    }
    
    
    private void ApplyRotation(GameObject bodyPart, Vector2 direction)
    {
        if (direction == Vector2.right)
            bodyPart.transform.rotation = Quaternion.Euler(0, 0, 270);
        else if (direction == -Vector2.up)
            bodyPart.transform.rotation = Quaternion.Euler(0, 0, 180);
        else if (direction == -Vector2.right)
            bodyPart.transform.rotation = Quaternion.Euler(0, 0, 90);
        else if (direction == Vector2.up)
            bodyPart.transform.rotation = Quaternion.Euler(0, 0, 0);
    }
    
    private float elapsedTime = 0f;
    void Update()
    {
        elapsedTime += Time.deltaTime;

        if (elapsedTime >= speed)
        {
            elapsedTime = 0f;
            MoveSnake();
            AudioManager.instance.PlaySnakeMoveSound();
        }

        if ((Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D)) && direction != -Vector2.right)
            selectedDirection = Vector2.right;
        else if ((Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S)) && direction != Vector2.up)
            selectedDirection = -Vector2.up;    // means 'down'
        else if ((Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A)) && direction != Vector2.right)
            selectedDirection = -Vector2.right; // means 'left'
        else if ((Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W)) && direction != -Vector2.up)
            selectedDirection = Vector2.up;
    }
    
    void OnTriggerEnter2D(Collider2D coll)
    {
        // Check if it's a part of the snake's body
        if (coll.name.StartsWith("BodyPart") || coll.name.StartsWith("Tail"))
        {
            // Trigger game over in GameManager
            FindObjectOfType<GameManager>().GameOver();
        }
        // Else if it's food
        else if (coll.name.StartsWith("Food"))
        {
            // The snake ate the food, so destroy this food GameObject
            Destroy(coll.gameObject);
            ateFood = true;
            speed *= 0.985f;
            AudioManager.instance.PlayFoodConsumedSound();
        }
    }
}