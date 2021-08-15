using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyAfter : MonoBehaviour
{
    [SerializeField]
    private float milisec;
    Clock _clock = new Clock();
    // Start is called before the first frame update
    void Start()
    {
        _clock.Reset();
    }

    // Update is called once per frame
    void Update()
    {
        if (_clock.ElapsedTime() > milisec)
            Destroy(gameObject);
    }
}
