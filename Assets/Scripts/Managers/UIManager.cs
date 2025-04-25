using UnityEngine;
using UniRx;
using TMPro;
using DG.Tweening;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    [SerializeField] private TextMeshProUGUI enemyKillText; // or TMPro.TMP_Text
    /*[SerializeField] private Text healthText;
    [SerializeField] private Text scoreText;*/
    [SerializeField] private GameObject[] UIgameObj;
    public Ease ease;
    public float punchAmnt;
    private void Start()
    {
        EventSubscribe();
    }

    public void EventSubscribe()
    {
        GameManager.Instance.OnPlayerDamaged
           .Subscribe(_ => UpdatePlayerHP())
           .AddTo(this);
        GameManager.Instance.OnPlayerDied.Subscribe(_ =>
        {
            ShowGameOver();
            UpdateEnemyKillHUD(GameManager.Instance.EnemyKillCount);
        }).AddTo(this);
    }

    private void UpdatePlayerHP()
    {
        Debug.Log("Reacting to player damage from GameManager!");
    }
    public void UpdateEnemyKillHUD(int killCount)
    {
        enemyKillText.text = $"Enemies Killed: {killCount}";
    }

    public void ShowGameOver()
    {
        for (int i = 0; i < UIgameObj.Length; i++)
        {
            UIgameObj[i].SetActive(true); // Do this BEFORE tweening
            UIgameObj[i].transform.DOPunchScale(Vector3.one * punchAmnt, 0.5f, 10, 1).SetEase(ease).SetUpdate(true);
        }
        Time.timeScale = 0f; // Pause the game
  /*      gameOverPanel.SetActive(true); // Show Game Over UI
        gameOverPanel.transform.localScale = Vector3.zero; // Start from 0 scale
        gameOverPanel.transform.DOScale(Vector3.one, 0.5f)
    .SetEase(Ease.OutBack); // Gives it a nice bounce feel
        playerHealthBar.SetActive(false);
        playerHealthBar.transform.localScale = Vector3.zero;*/


    }
}
