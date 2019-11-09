using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class slaim_move : MonoBehaviour {
    public float jumpPower = 3;

    private bool isJump;
    private float move;
    private Animator an;
    private Vector3 movement;
    private SpriteRenderer sp;
    private Rigidbody2D rig;

    // Start is called before the first frame update
    void Start() {
        an = this.GetComponent<Animator>();
        sp = this.GetComponent<SpriteRenderer>();
        rig = this.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update() {
        movement = Vector3.zero;
        if (Input.GetAxisRaw("Horizontal") < 0 || move < 0) {
            movement = Vector3.left;
            // an.SetBool("isMoving", true);
            if (sp.flipX == true) sp.flipX = false;
        }
        else if (Input.GetAxisRaw("Horizontal") > 0 || move > 0) {
            movement = Vector3.right;
            // an.SetBool("isMoving", true);
            if (sp.flipX == false) sp.flipX = true;
        }

        if ((Input.GetAxisRaw("Vertical") > 0) && rig.velocity.y < 0.01 && rig.velocity.y > -0.01) {
            Jump();
        }

        this.transform.position += movement * 4f * Time.deltaTime;
        move = 0;
    }

    public void MoveRight() {
        move = 1f;
    }
    public void MoveLeft() {
        move = -1f;
    }

    public void Jump() {
        rig.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);
    }
    public bool getIsJump() {
        return isJump;
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.CompareTag("ground")) {
            isJump = false;
            an.SetBool("isJumping", false);
        }
    }

    private void OnTriggerExit2D(Collider2D collision) {
        if (collision.CompareTag("ground")) {
            isJump = true;
            an.SetBool("isJumping", true);
        }
    }
}
