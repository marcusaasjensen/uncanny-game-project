using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] internal PlayerCollision _playerCollision;
    [SerializeField] internal PlayerMovement _playerMovement;
    [SerializeField] internal PlayerLife _playerLife;
    [SerializeField] private Color _defaultSpriteColor;

    public static PlayerController Instance;

    void Start()
    {
        Instance = this;
    }


    public Color DefaultSpriteColor
    {
        get { return _defaultSpriteColor; }
        set { _defaultSpriteColor = value; }
    }

}
