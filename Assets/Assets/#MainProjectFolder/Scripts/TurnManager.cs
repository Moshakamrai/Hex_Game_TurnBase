using UnityEngine;

public class TurnManager : MonoBehaviour
{

    private static TurnManager _instance;

    public static TurnManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<TurnManager>();
                if (_instance == null)
                {
                    GameObject turnManagerObject = new GameObject("TurnManager");
                    _instance = turnManagerObject.AddComponent<TurnManager>();
                }
            }
            return _instance;
        }
    }

    private void OnApplicationQuit()
    {
        _instance = null;
    }


    public int currentPlayerIndex = 0;  
    public bool isGameOver = false;     
    public int playersCount = 0;
   
    public GameObject[] players;

    void Start()
    {
        
        InitializeGame();
    }


    void Update()
    {
      
        if (!isGameOver)
        {

            HandleTurns();
        }
    }


    void InitializeGame()
    {
        
    }

  
    void HandleTurns()
    {

    }

   
    void SwitchTurn()
    {
        // Increment the current player index
        currentPlayerIndex = (currentPlayerIndex + 1) % players.Length;
    }
}
