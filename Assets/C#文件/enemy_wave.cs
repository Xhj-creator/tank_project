using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemy_wave : MonoBehaviour
{
    public float maxrange = 30f;
    public float speed = 10f;
    public Vector3 startpos;
    private Rigidbody rigid;
    float currentsize = 0.6f;
    // Start is called before the first frame update
    void Start()
    {
        startpos = transform.position;
        transform.localScale = Vector3.one * currentsize;
        rigid = GetComponent<Rigidbody>();
        rigid.velocity = transform.up * 5;
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector3.Distance(startpos, transform.position) >= maxrange)
        {
            Destroy(gameObject);
        }
        currentsize += 0.02f;
        transform.localScale = Vector3.one * currentsize;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            move enemy = other.gameObject.GetComponent<move>();
            enemy.damage(20);
        }
    }
}