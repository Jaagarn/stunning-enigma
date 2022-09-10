using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public static bool IsWaitingForEnemy = false;
    private Vector3 startingPosition;
    private Vector3 previousPosition;
    private Tile currentStandingTile;

    private void OnEnable()
    {
        EventManager.OnUndo += OnUndoHandler;
        EventManager.OnReset += OnResetHandler;
        EventManager.OnWait += OnWaitHandler;
    }

    private void OnDisable()
    {
        EventManager.OnUndo -= OnUndoHandler;
        EventManager.OnReset -= OnResetHandler;
        EventManager.OnWait -= OnWaitHandler;
    }

    // Keep starting position if need to reset
    private void Start()
    {
        startingPosition = previousPosition = transform.position;
    }

    private void Update()
    {
        // Up is positive z, down is negative z, right is positve x, and left is negative x.
        // I change 10 instead of one because it seemed to be easier to handle larger gameobjects.
        // But that is an aftereffect of trying to play around with rigidbody and collision detection.

        float newXValue = 0;

        // The tile the player is standing on has to have the bool for the direction you want to go enabled for it to work.
        if (Input.GetKeyDown(KeyCode.A) && currentStandingTile.canGoMinusX)
            newXValue = -10;
        else if(Input.GetKeyDown(KeyCode.D) && currentStandingTile.canGoPlusX)
            newXValue = 10;

        float newZValue = 0;

        if (Input.GetKeyDown(KeyCode.S) && currentStandingTile.canGoMinusZ)
            newZValue = -10;
        else if (Input.GetKeyDown(KeyCode.W) && currentStandingTile.canGoPlusZ)
            newZValue = 10;

        // The bool waitingfor enemy is true if it is the enemys turn to move.
        if (newXValue != 0 && !IsWaitingForEnemy)
        {
            //Keep previous position in case of undo click
            previousPosition = transform.position;
            var newPosition = previousPosition;

            newPosition.x += newXValue;

            transform.position = newPosition;
            IsWaitingForEnemy = true;
        }

        // Same as above, but for z. Horizontal movment comes before vertical because it made sense with the other rules of the game.
        if (newZValue != 0 && !IsWaitingForEnemy)
        {
            previousPosition = transform.position;
            var newPosition = previousPosition;

            newPosition.z += newZValue;

            transform.position = newPosition;
            IsWaitingForEnemy = true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // If the player touches the enemy, the player looses.
        // If the enemys mesh is not rendered, it counts as a defeted enemy.
        if(other.CompareTag("Enemy"))
        {
            if(!other.gameObject.GetComponent<Renderer>().enabled)
                return;
            EventManager.RaiseOnLevelLost();
            gameObject.GetComponent<Renderer>().enabled = false;
        }

        // Check the tile the player is standing on. It contains the information in what direction we can move.
        if(other.CompareTag("Tile"))
        {
            currentStandingTile = other.gameObject.GetComponent<Tile>();
        }
    }

    private void OnResetHandler()
    {
        transform.position = startingPosition;
        if (!gameObject.GetComponent<Renderer>().enabled)
            gameObject.GetComponent<Renderer>().enabled = true;
    }

    private void OnUndoHandler()
    {
        transform.position = previousPosition;
        if (!gameObject.GetComponent<Renderer>().enabled)
            gameObject.GetComponent<Renderer>().enabled = true;
    }

    //If the player press wait, the player skips a turn and let the enemy take one.
    private void OnWaitHandler()
    {
        IsWaitingForEnemy = true;
    }

}
