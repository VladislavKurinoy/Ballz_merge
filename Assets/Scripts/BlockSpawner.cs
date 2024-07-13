using System;
using UnityEngine;

public class BlockSpawner : MonoBehaviour
{
    [SerializeField] GameObject _blockPrefab;
    [SerializeField] Transform _cellTransform;

    void Start()
    {
	    Debug.Log(_cellTransform.position);
        Instantiate(_blockPrefab, _cellTransform.position, Quaternion.identity);
    }
}
