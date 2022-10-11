using System.Collections;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [SerializeField]
    private GameObject player;

    private Tile currentStandingTile;
    private Vector3 startingPosition;
    private Vector3 previousPosition;

    private int howManyMoves = 0;
    private bool moving = false;

    private void OnEnable()
    {
        EventManager.OnUndo += OnUndoHandler;
        EventManager.OnReset += OnResetHandler;
        EventManager.OnLevelWon += OnLevelWonHandler;
    }

    private void OnDisable()
    {
        EventManager.OnUndo -= OnUndoHandler;
        EventManager.OnReset -= OnResetHandler;
        EventManager.OnLevelWon -= OnLevelWonHandler;
    }


    // Keep starting position if need to reset
    private void Start()
    {
        startingPosition = previousPosition = transform.position;
    }

    //The enemy do movement in lateupdate so that it takes places after player input.
    private void LateUpdate()
    {
        if (!PlayerController.IsWaitingForEnemy)
            return;

        if (!moving)
            StartCoroutine(Move());
    }

    //The enemy moves twice, if possible. It will always attempt to move horizontally before vertically.
    private IEnumerator Move()
    {
        moving = true;
        previousPosition = transform.position;
        while (!(howManyMoves == 2))
        {
            var moveDirection = MoveDirection();

            if (moveDirection.Equals(Vector3.zero))
                break;

            if (Move(Vector3.right * Vector3.Dot(moveDirection, Vector3.right)))
                howManyMoves++;
            else if (Move(Vector3.forward * Vector3.Dot(moveDirection, Vector3.forward)))
                howManyMoves++;
            else
                //Break if the enemy cant move anymore.
                break;
            // A litte wait between the two moves, so the player can see how the enemy walks.
            // Also good for updating to the correct time when moved.
            yield return new WaitForSeconds(0.2f);
        }

        howManyMoves = 0;
        PlayerController.IsWaitingForEnemy = false;
        moving = false;
    }

    private Vector3 MoveDirection() 
    {
        var playerPosition = player.transform.position;
        var enemyPosition = transform.position;

        var differencePosition = playerPosition - enemyPosition;

        return differencePosition;
    }

    // Direction false Horizontal, true Vertical
    private bool Move(Vector3 direction)
    {
        direction.Normalize();

        if (!currentStandingTile.CanGoDirection(direction))
            return false;

        transform.position += direction*10;
        
        return true;
    }

    //Check the tile we are standing on for what directions we can go.
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Tile"))
            currentStandingTile = other.gameObject.GetComponent<Tile>();
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
    private void OnLevelWonHandler()
    {
        gameObject.GetComponent<Renderer>().enabled = false;
    }
}
