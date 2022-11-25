using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] PlayerCollision _playerCollision;
    [SerializeField] PlayerMovement _playerMovement;
    [SerializeField] PlayerLife _playerLife;
    [SerializeField] Color _defaultSpriteColor;

    public static PlayerController Instance;

    void Awake() 
    {
        #region Singleton
        if (!Instance) Instance = this;
        #endregion

        if (!_playerCollision)
            _playerCollision = GetComponent<PlayerCollision>();
        
        if(_playerMovement!)
            _playerMovement = GetComponent<PlayerMovement>();
        
        if(!_playerLife)
            _playerLife = GetComponent<PlayerLife>();
    }

    public PlayerMovement PlayerMovement
    {
        get 
        {
            if (_playerMovement)
                return _playerMovement;
            else
                Debug.Log("Player Movement reference in Player Controller script is missing.", this);
            return null;
        }
        set { _playerMovement = value; }
    }

    public PlayerLife PlayerLife
    {
        get
        {
            if (_playerLife)
                return _playerLife;
            else
                Debug.LogWarning("Player Life reference in Player Controller script is missing.", this);
            return null;
        }
        set { _playerLife = value; }
    }

    public PlayerCollision PlayerCollision
    {
        get
        {
            if (_playerCollision)
                return _playerCollision;
            else
                Debug.LogWarning("Player Collision reference in Player Controller script is missing.", this);
            return null;
        }
        set { _playerCollision = value; }
    }

    public Color DefaultSpriteColor
    {
        get { return _defaultSpriteColor; }
        set { _defaultSpriteColor = value; }
    }

}
