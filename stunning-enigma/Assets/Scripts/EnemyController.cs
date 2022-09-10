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

        if(!moving)
            StartCoroutine(Move());
    }

    //The enemy moves twice, if possible. It will always attempt to move horizontally before vertically.
    private IEnumerator Move()
    {
        moving = true;
        previousPosition = transform.position;
        while (!(howManyMoves == 2))
        {
            if (MoveHorizontally())
                howManyMoves++;
            else if (MoveVertically())
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

    //Horizontal movment one tile. Check if we should go left or right depending on where the player is.
    //If the enemy has the same x value, the enemy dooes not move. If the enemy is standing on a tile that does not have the
    //correct direction the enemy wants to go, the enemy does not move.
    private bool MoveHorizontally() 
    {
        var playerPosition = player.transform.position;

        var playerXPosition = playerPosition.x;
        var enemyXPosition = transform.position.x;

        var differenceX = playerXPosition - enemyXPosition;

        if (differenceX == 0)
            return false;

        var newXValue = differenceX > 0 ? 10 : -10;

        if (newXValue < 0 && !currentStandingTile.canGoMinusX)
            return false;

        if (newXValue > 0 && !currentStandingTile.canGoPlusX)
            return false;

        var newPosition = transform.position;
        newPosition.x += newXValue;

        transform.position = newPosition;
        return true;
    }

    //Same as horizontall, but for up and down. Happens only the horizontal returns false twice.
    private bool MoveVertically()
    {
        var playerPosition = player.transform.position;

        var playerZPosition = playerPosition.z;
        var enemyZPosition = transform.position.z;

        var differenceZ = playerZPosition - enemyZPosition;

        if (differenceZ == 0)
            return false;

        var newZValue = differenceZ > 0 ? 10 : -10;

        if (newZValue < 0 && !currentStandingTile.canGoMinusZ)
            return false;

        if (newZValue > 0 && !currentStandingTile.canGoPlusZ)
            return false;

        var newPosition = transform.position;
        newPosition.z += newZValue;

        transform.position = newPosition;
        return true;
    }

    //Check the tile we are standing on for what directions we can go.
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Tile"))
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
    private void OnLevelWonHandler()
    {
        gameObject.GetComponent<Renderer>().enabled = false;
    }
}
