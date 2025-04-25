using UnityEngine;
using UniRx;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public int EnemyKillCount { get; private set; } = 0;

    public Subject<Unit> OnEnemyDied = new Subject<Unit>();
    public Subject<int> OnPlayerDamaged = new Subject<int>();
    public Subject<Unit> OnPlayerDied = new Subject<Unit>();
    [SerializeField] private InputManager inputManager;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        OnEnemyDied.Subscribe(_ =>
        {
            EnemyKillCount++;
            UIManager.Instance.UpdateEnemyKillHUD(EnemyKillCount);
        }).AddTo(this);
    }
    public void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    public void OnApplicationQuit()
    {
        Application.Quit();
    }
}
