using UniRx;
using UnityEngine;
using Ami.BroAudio;
public class PlayerCombat : MonoBehaviour
{
    public float shootCooldown = 0.3f;
    public float rayLength = 10f;
    public int damage;
    public LayerMask enemyLayer;
    public ParticleSystem partSystem;
    private float lastShootTime = -999f;
    private RaycastHit2D[] hitResults = new RaycastHit2D[5];
    private Vector2 lastAimDir = Vector2.right;
    private Vector2 currentMouseWorldPos;

    [SerializeField] Aimer aimer;
    [SerializeField] private InputManager inputManager;

    public SoundID gunSound; 
    private void OnEnable()
    {
       inputManager.OnMousePosition
            .Subscribe(UpdateMousePosition)
            .AddTo(this);
        inputManager.OnAttackPressed.Subscribe(_ =>
        {
            Debug.Log("Attack pressed at: " + currentMouseWorldPos); // attack position relative to player
            TryShoot(currentMouseWorldPos);
        }).AddTo(this);
    }
    private void UpdateMousePosition(Vector2 screenPos)
    {
        currentMouseWorldPos = Camera.main.ScreenToWorldPoint(screenPos);
    }
    public void TryShoot(Vector2 aimPos)
    {
        if (Time.time - lastShootTime < shootCooldown)
            return;
        BroAudio.Play(gunSound);
        lastShootTime = Time.time;
        //Vector2 dir = ((Vector2)aimPos - (Vector2)transform.position).normalized;
        Vector2 dir = aimer.direction.normalized;
        lastAimDir = dir;
        dir.Normalize();
        Debug.Log(dir);
        Vector2 directi = transform.position - aimer.direction;
        float angle = Mathf.Atan2(directi.y, directi.x) * Mathf.Rad2Deg; // Converts direction to angle in degrees

        ObjectPoolSystem.SpawnObject(partSystem.gameObject, transform.position, Quaternion.Euler(0,0,angle)); ;
        Debug.DrawRay(transform.position, dir * rayLength, Color.red, 0.1f);

        int hits = Physics2D.RaycastNonAlloc(transform.position, dir, hitResults, rayLength, enemyLayer);

        if (hits > 0)
        {
            for (int i = 0; i < hits; i++)
            {
                var enemy = hitResults[i].collider.GetComponent<BaseEnemyUnit>();
                if (enemy != null)
                {
                    Debug.Log("Enemy hit at position: " + hitResults[i].point); // Debug log to check hit position
                    enemy.TakeDamage(damage);
                    break;
                }
            }
        }
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, (Vector2)transform.position + lastAimDir * rayLength);

        //Gizmos.DrawLine()
    }
}