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
        
        isJump = false;
        AnimationUpdate(false);
        if (Input.GetAxisRaw("Horizontal") < 0 || move < 0) {
            movement = Vector3.left;
            AnimationUpdate(true);
            if (sp.flipX == true) sp.flipX = false;
        }
        else if (Input.GetAxisRaw("Horizontal") > 0 || move > 0) {
            movement = Vector3.right;
            AnimationUpdate(true);
            if (sp.flipX == false) sp.flipX = true;
        }
        if ((Input.GetAxisRaw("Vertical") > 0 || isJump == true) && rig.velocity.y < 0.01 && rig.velocity.y > -0.01) {
            rig.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);
        }
        this.transform.position += movement * 1f * Time.deltaTime;
        move = 0;
    }
    void AnimationUpdate(bool b) {
        an.SetBool("isMoving", b);
    }

    public void MoveRight() {
        move = 1f;
    }
    public void MoveLeft() {
        move = -1f;
    }
    public void Jump() {
        isJump = true;
    }
}
