using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class follow_object : MonoBehaviour
{
    public GameObject obj;
    public Vector3 offset;

    private Transform objTransform;

    // Start is called before the first frame update
    void Start()
    {
        objTransform = obj.transform;
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.position = Vector3.Lerp(this.transform.position, objTransform.position + offset, 2f * Time.deltaTime);
        this.transform.Translate(0, 0, -10);
    }
}
