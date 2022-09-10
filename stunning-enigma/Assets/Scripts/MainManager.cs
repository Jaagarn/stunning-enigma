using UnityEngine;
using UnityEngine.SceneManagement;

public class MainManager : MonoBehaviour
{
    //This is the main manager. It stays loaded during the whole lifecycle of the game. To load
    //levels and keep track on what is loaded.

    public static MainManager Instance;

    void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1) && !(SceneManager.GetActiveScene().buildIndex.Equals(0)))
            LoadLevelOne();
        if (Input.GetKeyDown(KeyCode.Alpha2) && !(SceneManager.GetActiveScene().buildIndex.Equals(1)))
            LoadLevelTwo();
        if (Input.GetKeyDown(KeyCode.Alpha3) && !(SceneManager.GetActiveScene().buildIndex.Equals(2)))
            LoadLevelThree();
    }

    public void LoadLevelOne()
    {
        SceneManager.LoadScene(0);
    }

    public void LoadLevelTwo()
    {
        SceneManager.LoadScene(1);
    }

    public void LoadLevelThree()
    {
        SceneManager.LoadScene(2);
    }

}
