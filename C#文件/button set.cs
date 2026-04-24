using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class buttonset : MonoBehaviour
{
    public GameObject chooseCanvas;
    // Start is called before the first frame update
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
