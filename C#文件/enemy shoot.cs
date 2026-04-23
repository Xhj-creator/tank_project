using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyshoot : MonoBehaviour
{
    int hitcount = 0;
    int maxhit = 5;
    private Rigidbody rigid;
    // Start is called before the first frame update
    void Start()
    {
        rigid = GetComponent<Rigidbody>();
        rigid.velocity = transform.forward * 8;
    }

    // Update is called once per frame
    void Update()
    {
        rigid.rotation = Quaternion.LookRotation(rigid.velocity.normalized);
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("wall"))
        {
            hitcount++;
            if (hitcount > maxhit)
            {
                Destroy(gameObject);
            }
        }
        if (collision.gameObject.CompareTag("Player"))
        {
            move findenemy = collision.gameObject.GetComponent<move>();
            if (findenemy != null)
            {
                findenemy.damage(10);
            }
            Destroy(gameObject);
        }
    }
}
