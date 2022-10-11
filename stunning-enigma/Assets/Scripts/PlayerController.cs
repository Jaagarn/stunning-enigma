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
        if (IsWaitingForEnemy)
            return;

        Vector3 moveDirection = Vector3.zero;

        if (Input.GetKeyDown(KeyCode.A) && moveDirection.Equals(Vector3.zero))
            moveDirection = Vector3.left;

        if (Input.GetKeyDown(KeyCode.D) && moveDirection.Equals(Vector3.zero))
            moveDirection = Vector3.right;

        if (Input.GetKeyDown(KeyCode.S) && moveDirection.Equals(Vector3.zero))
            moveDirection = Vector3.back;

        if (Input.GetKeyDown(KeyCode.W) && moveDirection.Equals(Vector3.zero))
            moveDirection = Vector3.forward;

        if (currentStandingTile.CanGoDirection(moveDirection) && !moveDirection.Equals(Vector3.zero))
        {
            transform.position += moveDirection * 10;
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
