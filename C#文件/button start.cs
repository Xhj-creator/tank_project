using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class buttonstart : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject chooseCanvas;

    void Start()
    {
        chooseCanvas.SetActive(false);
    }

    public void OnStartButtonClick()
    {
        transform.root.gameObject.SetActive(false);
        chooseCanvas.SetActive(true);
    }

// Update is called once per frame
void Update()
    {
        
    }
}
