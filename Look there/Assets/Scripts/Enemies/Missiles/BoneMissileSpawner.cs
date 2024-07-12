using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoneMissileSpawner : MonoBehaviour
{
    [SerializeField] GameObject _bonePrefab;
    [SerializeField] Transform[] _spawnPositions;
    private void Start()
    {
        //InitialSpawn();
    }
    public GameObject SpawnBone(int spawnerPosIndex)
    {
       return Instantiate(_bonePrefab, _spawnPositions[spawnerPosIndex].position, _bonePrefab.transform.rotation);
    }
    public GameObject SpawnBone(int spawnerPosIndex,Transform target,float speed)
    {
        BoneMissile aa = Instantiate(_bonePrefab, _spawnPositions[spawnerPosIndex].position, _bonePrefab.transform.rotation).GetComponent<BoneMissile>();
        aa.SetDirectionAndSpeed(target.position,speed);
        return aa.gameObject;
    }
    public GameObject SpawnBone(int spawnerPosIndex,float angle)
    {
        BoneMissile aa =Instantiate(_bonePrefab, _spawnPositions[spawnerPosIndex].position, _bonePrefab.transform.rotation).GetComponent<BoneMissile>();
        aa.SetRotation(angle);
        return aa.gameObject;
    }
    public void InitialSpawn(int index)
    {
        SpawnBone(index);
        SpawnBone(index, 90);
        //StartCoroutine(SpawnBoneDelayed(0, 0));
        //StartCoroutine(SpawnBoneDelayed(3.5f, 0));
    }

    IEnumerator SpawnBoneDelayed(float delay,int spawnIndex)
    {
        yield return new WaitForSeconds(delay);
        SpawnBone(spawnIndex);
    }
}