using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;

public class bouncetrigger : MonoBehaviour
{
    public GameObject bullet;
    // Start is called before the first frame update
    void Start()
    {
        shoot Shoot = bullet.GetComponent<shoot>();
        Shoot.maxhit = 1;
    }

    // Update is called once per frame
    void Update()
    {

    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            move Move = other.gameObject.GetComponent<move>();
            shoot Shoot = bullet.GetComponent<shoot>();
            Move.maxBounces = 3;
            Shoot.maxhit = 3;
            Destroy(gameObject);
        }
    }
}
