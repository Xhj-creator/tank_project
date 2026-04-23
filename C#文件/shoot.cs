using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class shoot : MonoBehaviour
{
    int hitcount = 0;
    public int maxhit = 1;
    private Rigidbody rigid;
    // Start is called before the first frame update
    void Start()
    {
        rigid = GetComponent<Rigidbody>();
        rigid.velocity = transform.forward * 5;
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
                Destroy (gameObject);
            }
        }
        if (collision.gameObject.CompareTag("enemy"))
        {
            IDamageble damageable = collision.gameObject.GetComponent<IDamageble>();
            if (damageable != null)
            {
                damageable.damage(10);
            }
            Destroy(gameObject);
        }
    }
}
