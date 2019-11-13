using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomMoveCorutine : MonoBehaviour
{
    private float speed = 2f;
    private Rigidbody2D rigid;
    private Animator animator;
    private Vector3 movement;
    private int movementFlag = 0;

    // Start is called before the first frame update
    void Start()
    {
        Application.targetFrameRate = 60;
        rigid = this.GetComponent<Rigidbody2D>();
        animator = this.GetComponent<Animator>();
        StartCoroutine("ChangeMovement");
    }

    private void FixedUpdate() {
        Move();
    }

    void Move() {
        Vector3 moveVelocity = Vector3.zero;
        if (movementFlag != 3) {
            if (movementFlag == 1) {
                moveVelocity = Vector3.left;
                transform.localScale = new Vector3(1, 1, 1);
            }
            else if (movementFlag == 2) {
                moveVelocity = Vector3.right;
                transform.localScale = new Vector3(-1, 1, 1);
            }

            transform.position += moveVelocity * speed * Time.deltaTime;
        }
        else {
            if (rigid.velocity.y < 1 && rigid.velocity.y > -1)
            rigid.AddForce(Vector2.up * 6f, ForceMode2D.Impulse);
        }
    }
    IEnumerator ChangeMovement() {

        movementFlag = Random.Range(0, 4);
        float timer = Random.Range(2, 4);
        switch(movementFlag) {
            case 0:
                animator.SetBool("isMoving", false);
                break;
            case 1:
                animator.SetBool("isMoving", true);
                break;
            case 2:
                animator.SetBool("isMoving", true);
                break;
        }
        if (movementFlag == 3) {
            animator.SetBool("isMoving", false);
            yield return null;
        }
        else
            yield return new WaitForSeconds(timer);

        StartCoroutine("ChangeMovement");
    }
}
