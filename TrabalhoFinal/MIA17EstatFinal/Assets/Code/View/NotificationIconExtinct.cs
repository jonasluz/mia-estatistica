using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NotificationIconExtinct : MonoBehaviour {

    public Sprite inactiveIcon;
    public Sprite activeIcon;

    private Image image;
    private AudioSource notification;

    private bool before = false;

    private void Awake()
    {
        image = GetComponent<Image>();
        notification = GetComponent<AudioSource>(); 
        image.sprite = inactiveIcon;
    }

    private void LateUpdate()
    {
        image.sprite = Board.B.InfectionExtincted ? activeIcon : inactiveIcon;
        if (before != Board.B.InfectionExtincted)
            notification.Play();
        before = Board.B.InfectionExtincted;
    }
}
