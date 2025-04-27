using UnityEngine;
using DG.Tweening;
using UniRx;

public class GunBhviour : MonoBehaviour
{
    [SerializeField] SpriteRenderer gpxSprite;
    [SerializeField] SpriteRenderer gunSprite;

    [SerializeField] PlayerMovement playerMovement;

    [SerializeField] Aimer aim;
    [SerializeField] Animator animator;
    [SerializeField] Rigidbody2D rb;

    private Vector2 origScale;

    private void Start()
    {
        origScale = gunSprite.transform.localScale;
        InputManager.Inspance.OnAttackPressed.Subscribe(_ => GunScale());
    }

    private void GunScale()
    {
        gunSprite.transform.DOPunchScale(origScale * 1.2f, .1f, 10, 1);
    }
    void FixedUpdate()
    {
        Vector2 lookDir = (Vector2)aim.mousePos - (Vector2)transform.position;
        float angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg;

        transform.eulerAngles = new Vector3(0, 0, angle);

        if (playerMovement.isMoving)
        {
            if (lookDir.x < 0)
            {
                gpxSprite.flipX = true;
                if (lookDir.y < 0) //2
                {
                    if (playerMovement.isMoving) { animator.Play("frontRun"); }
                }
                if (lookDir.y > 0)
                {
                    if (playerMovement.isMoving) { animator.Play("backRun"); }//3
                    gunSprite.sortingOrder = -1;
                }
                //  }
            }
            else if (lookDir.x > 0)
            {
                gpxSprite.flipX = false;
                /*if (playerMovement.isDashing)
                {
                    animator.Play("panicDodge");
                }
                else
                {
*/
                if (lookDir.y < 0)//4
                {
                    if (playerMovement.isMoving) { animator.Play("frontRun"); }
                }
                if (lookDir.y > 0)
                {
                    if (playerMovement.isMoving) { animator.Play("backRun"); }//1
                    gunSprite.sortingOrder = 1;
                }
                // }
            }

        }
        else if (!playerMovement.isMoving)
        //&& !playerMovement.isDashing)
        {
            animator.Play("idle");
            if (lookDir.x < 0)
            {
                gpxSprite.flipX = true;
            }
            else if (lookDir.x > 0)
            {
                gpxSprite.flipX = false;
            }
        }

    }
}
