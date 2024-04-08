using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class FoodSpawner : MonoBehaviour
{
    public List<Sprite> foodSprites; // Assign this in the inspector
    public Tilemap spawnArea; // Assign this in the inspector
    private GameObject currentFood;
    public SnakeMovement snake; // Assign this in the inspector
    public GameManager gameManager; // Assign this in the inspector

    void Update()
    {
        if (currentFood == null)
        {
            SpawnFood();
        }
    }

    void SpawnFood()
    {
        // Convert the snake's body positions to cell positions and store them in a HashSet
        HashSet<Vector3Int> snakeBodyCells = new HashSet<Vector3Int>();
        foreach (GameObject bodyPart in snake.GetBody())
        {
            Vector3Int bodyCellPosition = spawnArea.WorldToCell(bodyPart.transform.position);
            snakeBodyCells.Add(bodyCellPosition);
        }

        // Before do while check if there is no more space to spawn food by iterating through the spawnArea
        bool noSpace = true;
        foreach (Vector3Int cell in spawnArea.cellBounds.allPositionsWithin)
        {
            if (spawnArea.HasTile(cell) && !snakeBodyCells.Contains(cell))
            {
                noSpace = false;
                break;
            }
        }
        if (noSpace)
        {
            gameManager.CompleteLevel();
            return;
        }

        Vector3Int cellPosition;
        do
        {
            int x = Random.Range(spawnArea.cellBounds.xMin, spawnArea.cellBounds.xMax);
            int y = Random.Range(spawnArea.cellBounds.yMin, spawnArea.cellBounds.yMax);
            cellPosition = new Vector3Int(x, y, 0);
        } while (!spawnArea.HasTile(cellPosition) || snakeBodyCells.Contains(cellPosition));

        Vector2 spawnPosition = spawnArea.GetCellCenterWorld(cellPosition);
        currentFood = new GameObject("Food");
        SpriteRenderer renderer = currentFood.AddComponent<SpriteRenderer>();
        BoxCollider2D collider = currentFood.AddComponent<BoxCollider2D>();
        renderer.sprite = foodSprites[Random.Range(0, foodSprites.Count)];
        collider.size = renderer.sprite.bounds.size;
        collider.isTrigger = true;
        currentFood.transform.position = spawnPosition;
    }
}