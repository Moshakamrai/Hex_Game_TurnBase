using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    // Function to load the next scene
    public void LoadNextScene()
    {
        SelectedState.storedHexcel = null;
        // Get the current scene index
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;

        // Load the next scene by incrementing the current scene index
        SceneManager.LoadScene(currentSceneIndex + 1);
    }
}
