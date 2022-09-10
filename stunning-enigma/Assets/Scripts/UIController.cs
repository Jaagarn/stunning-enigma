using UnityEngine;

public class UIController : MonoBehaviour
{
    // This is the UIController. It's manages the buttonclicks and also disables/enables the 
    // victory and loose texts on each level.
    [SerializeField]
    private GameObject victoryText;

    [SerializeField]
    private GameObject lostText;

    private void OnEnable()
    {
        EventManager.OnLevelWon += OnLevelWonHandler;
        EventManager.OnLevelLost += OnLevelLostHandler;
    }

    private void OnDisable()
    {
        EventManager.OnLevelWon -= OnLevelWonHandler;
        EventManager.OnLevelLost -= OnLevelLostHandler;
    }

    public void OnWaitButtonClick()
    {
        EventManager.RaiseOnWait();
    }
    public void OnResetButtonClick()
    {
        EventManager.RasieOnReset();
        if (lostText.activeSelf)
            lostText.SetActive(false);
        if (victoryText.activeSelf)
            victoryText.SetActive(false);
    }
    public void OnUndoButtonClick()
    {
        EventManager.RaiseOnUndo();
        if(lostText.activeSelf)
            lostText.SetActive(false);
        if(victoryText.activeSelf)
            victoryText.SetActive(false);
    }
    private void OnLevelLostHandler()
    {
        lostText.SetActive(true);
    }
    private void OnLevelWonHandler()
    {
        victoryText.SetActive(true);
    }
}
