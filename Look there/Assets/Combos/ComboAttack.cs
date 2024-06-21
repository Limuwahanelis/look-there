using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="ComboAttack")]
public class ComboAttack : ScriptableObject
{
    public float AttackWindowStart => _nextAttackWindowStart;
    public float AttackWindowEnd => _nextAttackWindowEnd;
    [SerializeField] AnimationClip _associatedAnimation;
    [SerializeField] bool _useFrames;
    [SerializeField] float _nextAttackWindowStart;
    [SerializeField] float _nextAttackWindowEnd;
    [SerializeField] int _nextAttackWindowStartFrame;
    [SerializeField] int _nextAttackWindowEndFrame;

    private void OnValidate()
    {
        if(_useFrames)
        {
            if (_associatedAnimation != null)
            {
                int framesInAnimation =(int)( _associatedAnimation.frameRate * _associatedAnimation.length);
                //float timeForFrame = 1 / _associatedAnimation.frameRate;
                _nextAttackWindowStart =  (_nextAttackWindowStartFrame/ (float)framesInAnimation)*_associatedAnimation.length;
                _nextAttackWindowEnd =  (_nextAttackWindowEndFrame/ (float)framesInAnimation) * _associatedAnimation.length;
            }
        }
    }
}
