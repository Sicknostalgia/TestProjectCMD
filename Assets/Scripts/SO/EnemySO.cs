using UnityEngine;

[CreateAssetMenu(fileName = "New Enemy Data", menuName = "Enemy/Enemy Data")]
public class EnemySO : ScriptableObject
{
    public float moveSpeed = 2f;
    public int attakDmg = 1;
    public float attackRange = 1.5f;
    public float fovAngle = 90f;
    public float health = 100f;
}
