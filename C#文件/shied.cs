using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class shied : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
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
            Move.noharm_mode = true;
            Destroy(gameObject);
        }
    }
}
