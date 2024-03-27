using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class LevelManager : MonoBehaviour
{
    // Function to load the next scene
    public void LoadNextScene()
    {
        OnActiveState.storedEnemyCell = null;
        OnActiveState.activeCell = null;
        SelectedState.storedHexcel = null;
        // Get the current scene index
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;

        // Load the next scene by incrementing the current scene index
        SceneManager.LoadScene(currentSceneIndex + 1);
    }
}
