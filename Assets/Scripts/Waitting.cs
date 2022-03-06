using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Waitting : MonoBehaviour
{
    TMP_Text waitting;
    float time = 0;
    [SerializeField] float limitTime =0.5f;

    // Start is called before the first frame update
    void Start()
    {
        waitting = GetComponent<TMP_Text>();
    }

    // Update is called once per frame
    void Update()
    {

        time += Time.deltaTime;
        if(time>limitTime)
        {
            time = 0;
            waitting.enabled = !waitting.enabled;
        }    
    }
}
