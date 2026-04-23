using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameralook : MonoBehaviour
{
    public Transform player;
    public float speed = 5f;
    Vector3 distance;
    // Start is called before the first frame update
    void Start()
    {
        distance = transform.position - player.position;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (player == null) return;
        Vector3 pos = player.position + distance;
        transform.position = Vector3.Lerp(transform.position, pos, speed * Time.deltaTime);
        //transform.LookAt(player);

    }
}
