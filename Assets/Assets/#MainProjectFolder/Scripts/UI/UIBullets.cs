using DG.Tweening;
using UnityEngine;

public class UIBullets : MonoBehaviour
{
    // Singleton instance
    private static UIBullets instance;
    private float rotate;
    [SerializeField] private GameObject[] _bullets;
    int count = 0;
    // Public property to access the singleton instance
    public static UIBullets Instance
    {
        get { return instance; }
    }

    // Ensure the instance is set when the script is loaded
    private void Awake()
    {
        // If the instance doesn't exist yet, set it to this object
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // Don't destroy this object when loading new scenes
        }
        else
        {
            // If an instance already exists and it's not this one, destroy this duplicate
            if (instance != this)
            {
                Destroy(gameObject);
            }
        }
    }

    public void FiredBullet()
    {
        rotate += 60f;
        transform.DORotate(new Vector3(0f, 0f, rotate), 0.2f);
        if (_bullets[count] != null)
        {
            _bullets[count].gameObject.SetActive(true);
        }
        count++;
    }
}
