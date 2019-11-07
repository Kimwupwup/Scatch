using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class follow_object : MonoBehaviour
{
    public GameObject obj;
    public Vector3 offset;

    private Vector2 temp;
    private Transform objTransform;
    private Animator playerAn;
    // Start is called before the first frame update
    void Start()
    {
        objTransform = obj.transform;
        if (this.CompareTag("Player"))
            playerAn = this.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (this.CompareTag("Player")) {            
            temp.y = this.transform.position.y;
            temp.x = objTransform.position.x;
            if (this.transform.position.x < temp.x - 0.3 || this.transform.position.x > temp.x + 0.3)
                playerAn.SetBool("isMoving", true);
            else
                playerAn.SetBool("isMoving", false);
            this.transform.position = Vector2.Lerp(this.transform.position, temp, 4f * Time.deltaTime);
        }
        else if (this.CompareTag("MainCamera")) {
            this.transform.position = Vector2.Lerp(this.transform.position, objTransform.position + offset, 2f * Time.deltaTime);
            this.transform.Translate(0, 0, -10);
        }
    }
}
