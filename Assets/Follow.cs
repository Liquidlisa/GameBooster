using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Follow : MonoBehaviour {

    public GameObject objectToFollow;

    public float speed = 2.0f;

    void Update()
    {
        float interpolation = speed * Time.deltaTime;
        Vector3 position = this.transform.position;
        if (Mathf.Abs(objectToFollow.transform.position.y - this.transform.position.y) > 0.03 || Mathf.Abs(objectToFollow.transform.position.x - this.transform.position.x) > 0.03)
        {
        position.y = Mathf.Lerp(this.transform.position.y, objectToFollow.transform.position.y, interpolation);
        position.x = Mathf.Lerp(this.transform.position.x, objectToFollow.transform.position.x, interpolation);
        }
        else
        {
            position.y = objectToFollow.transform.position.y;
            position.x = objectToFollow.transform.position.x;
        }
        this.transform.position = position;
    }
}
