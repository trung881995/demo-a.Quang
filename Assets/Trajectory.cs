using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trajectory : MonoBehaviour
{
    [SerializeField] int dotsNumber;
    [SerializeField] GameObject dotsParent;
    [SerializeField] GameObject dotPrefab;
    [SerializeField] float dotSpacing;
    
    Vector2 pos;
    float timeStamp;
    Transform[] dotsList;
    // Start is called before the first frame update
    void Start()
    {
        hide();
        prepareDots();
    }

    // Update is called once per frame
    void Update()
    {

    }

    void prepareDots()
    {
        dotsList = new Transform[dotsNumber];
        for (int i = 0; i < dotsNumber; i++)
        {
            dotsList[i] = Instantiate(dotPrefab, null).transform;
            dotsList[i].parent = dotsParent.transform;
        }
    }
    public void updateDots(Vector3 playerPos,Vector2 forceApplied)
    {
        timeStamp = dotSpacing;
        for (int i = 0;i< dotsNumber;i++)
        {
            pos.x = (playerPos.x + forceApplied.x * timeStamp);
            pos.y = (playerPos.y + forceApplied.y * timeStamp) - (Physics2D.gravity.magnitude * timeStamp * timeStamp)/ 2f;
            dotsList[i].position = pos;
            timeStamp += dotSpacing;
        }
    }
    
    public void show()
    {
        dotsParent.SetActive(true);
    }
    public void hide()
    {
        dotsParent.SetActive(false);
    }
}
