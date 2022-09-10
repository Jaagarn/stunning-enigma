using UnityEngine;

public class GoalSphere : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if(other.GetComponent<Renderer>().enabled)
                EventManager.RaiseOnLevelWon();
        }
    }
}
