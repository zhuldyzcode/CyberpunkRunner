using UnityEngine;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    // Define UnityEvents for each game state
    [System.Serializable] public class GameStateEvent : UnityEvent { }

    public GameStateEvent OnStartMenu;
    public GameStateEvent OnPlay;
    public GameStateEvent OnPause;
    public GameStateEvent OnResume;
    public GameStateEvent OnLose;

    private GameState currentState;

    private void Awake()
    {
        // Singleton pattern to ensure there's only one GameManager
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        ChangeState(GameState.StartMenu);
    }

    public void ChangeState(GameState newState)
    {
        currentState = newState;
        switch (newState)
        {
            case GameState.StartMenu:
                OnStartMenu.Invoke();
                break;
            case GameState.Play:
                OnPlay.Invoke();
                break;
            case GameState.Pause:
                OnPause.Invoke();
                break;
            case GameState.Resume:
                OnResume.Invoke();
                break;
            case GameState.Lose:
                OnLose.Invoke();
                break;
        }
    }
    public void ChangeStateToPause()
    {
        ChangeState(GameState.Pause);
    }
    public void ChangeStateToPlay()
    {
        ChangeState(GameState.Play);
    }
    public void ChangeStateToStartMenu()
    {
        ChangeState(GameState.StartMenu);
    }
    public void ChangeStateToLose()
    {
        ChangeState(GameState.Lose);
    }
    public void ChangeStateToResume()
    {
        ChangeState(GameState.Resume);
    }

    public GameState GetCurrentState()
    {
        return currentState;
    }
}