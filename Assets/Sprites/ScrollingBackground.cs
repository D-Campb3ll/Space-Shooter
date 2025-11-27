using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollingBackground : MonoBehaviour
{
    [SerializeField] private float _scrollSpeed;
    [SerializeField] private Renderer _bgRenderer;
    private GameManager _gameManager;

    private void Start()
    {
        _gameManager = GameObject.Find("Game_Manager").GetComponent<GameManager>();

        if (_gameManager == null)
        {
            Debug.LogError("Game Manager on Scrolling Background Script is NULL");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (_gameManager._isGameStarted == true)
        {
            _bgRenderer.material.mainTextureOffset += new Vector2(0, _scrollSpeed * Time.deltaTime);
        }
    }
}
