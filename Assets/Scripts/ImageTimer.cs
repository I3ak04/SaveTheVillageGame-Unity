using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImageTimer : MonoBehaviour
{

    public bool Tick;

    [SerializeField] private float MaxTime;
    
    private Image _img;
    private float _currentTime;

    void Start()
    {
        _img = GetComponent<Image>();
        _currentTime = MaxTime;
    }

    void Update()
    {
        _currentTime -= Time.deltaTime;
        Tick = false;

        if(_currentTime < 0)
        {
            Tick = true;
            _currentTime = MaxTime;
        }

        _img.fillAmount = _currentTime / MaxTime;
    }

    public void RetryTime()
    {
        _currentTime = MaxTime;
    }
       
}
