using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using static Unity.Burst.Intrinsics.X86.Avx;

public class PlayerWarpSkill : MonoBehaviour
{
    [SerializeField] PlayerMovement _playerMovement;
    [SerializeField] Transform _warpSpriteLocator;
    [SerializeField] Vector2 _warpSpriteSize;
    [SerializeField] SpriteRenderer _mainSpriteRenderer;
    [SerializeField] float _warpDistance;
    [SerializeField] Transform _rayStart;
    [SerializeField] SpriteRenderer _spriteToSpawnInPlace;
    [SerializeField] float _timeToHideClone;
    private Vector2 _warpPos;
    public LayerMask mask;
    private void Update()
    {
        ShowWarpLocation();
    }
    public void ShowWarpLocation()
    {
        RaycastHit2D hit = Physics2D.BoxCast(_rayStart.position, _warpSpriteSize, 0, transform.right*_playerMovement.FlipSide, _warpDistance, mask);
        if(hit)
        {
            _warpSpriteLocator.position = hit.point;
        }
        else
        {
            _warpSpriteLocator.position = _rayStart.position + new Vector3(_warpDistance*_playerMovement.FlipSide, 0, 0);
        }
        //_warpSpriteLocator.MovePosition(new Vector2(transform.position.x+ (_playerMovement.FlipSide) * _warpDistance, transform.position.y));
    }

    public void SpawnSprite()
    {
        _spriteToSpawnInPlace.transform.position = transform.position;
        _spriteToSpawnInPlace.sprite = _mainSpriteRenderer.sprite;
        StartCoroutine(HideCloneCor());
    }
    public void Warp()
    {
        SpawnSprite();
        transform.position = _warpSpriteLocator.position;
    }

    IEnumerator HideCloneCor()
    {
        float hidePct = 0;
        float time = 0;
        while(hidePct<1)
        {
            hidePct = time/_timeToHideClone;
            float tmp = (float)math.remap(0, 1, -0.5, 0.5,1- hidePct);
            Logger.Log(tmp);
            _spriteToSpawnInPlace.sharedMaterial.SetFloat("_HideValue", tmp);
            time+= Time.deltaTime;
            yield return null;
        }
        _spriteToSpawnInPlace.sharedMaterial.SetFloat("_HideValue", 0.5f);
    }
}
