using UnityEngine;
using UnityEngine.SceneManagement;

public sealed class DebugCommands : MonoBehaviour
{
    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    void Update()
    {
        #region RESET CURRENT SCENE
        if (Input.GetKeyDown(KeyCode.R))
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        #endregion
    }

    [RuntimeInitializeOnLoadMethod]
    static void AutoCreate()
    {
        new GameObject("DebugCommands").AddComponent<DebugCommands>();
    }
}
