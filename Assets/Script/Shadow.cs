using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shadow : MonoBehaviour
{
    public GameObject groundEndPoint;

    private GameObject shadow;

    // Start is called before the first frame update
    void Start()
    {
        //shadow = new GameObject();

        //shadow.AddComponent<SpriteRenderer>().sprite =
        //this.GetComponent<SpriteRenderer>().sprite;

        //shadow.transform.localScale = this.transform.localScale;

        //shadow.GetComponent<SpriteRenderer>().flipY = true;
        //shadow.GetComponent<SpriteRenderer>().sortingOrder = 2;
        //shadow.transform.SetParent(this.transform);
    }

    // Update is called once per frame
    void Update() {
        //shadow.GetComponent<SpriteRenderer>().flipX = this.GetComponent<SpriteRenderer>().flipX;
        //shadow.transform.position = new Vector2(this.transform.position.x,
        //    groundEndPoint.transform.position.y - (this.transform.position.y - groundEndPoint.transform.position.y));
    }
}
