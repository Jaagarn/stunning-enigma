using UnityEngine;

public class EventManager : MonoBehaviour
{
    public delegate void Reset();
    public static event Reset OnReset;

    public delegate void Undo();
    public static event Undo OnUndo;

    public delegate void LevelWon();
    public static event LevelWon OnLevelWon;

    public delegate void LevelLost();
    public static event LevelLost OnLevelLost;

    public delegate void Wait();
    public static event Wait OnWait;

    public static void RasieOnReset()
    {
        OnReset?.Invoke();
    }

    public static void RaiseOnUndo()
    {
        OnUndo?.Invoke();
    }

    public static void RaiseOnLevelWon()
    {
        OnLevelWon?.Invoke();
    }

    public static void RaiseOnLevelLost()
    {
        OnLevelLost?.Invoke();
    }

    public static void RaiseOnWait()
    {
        OnWait?.Invoke();
    }
}
