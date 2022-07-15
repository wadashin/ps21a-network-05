using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnObj : MonoBehaviour
{
    [Tooltip("�G�𐶐�����͈�")]
    [SerializeField] float _spawnPos;
    public float Range
    {
        get
        {
            return this._spawnPos * 2;
        }
    }

    [Tooltip("�G�����͈͂̒[1")]
    [SerializeField] Transform cube1;
    [Tooltip("�G�����͈͂̒[2")]
    [SerializeField] Transform cube2;


    WaveManager waveManager;

    private void Awake()
    {
        WaveManager.spawnObjs.Add(this);
    }

    private void OnDrawGizmosSelected()
    {
        cube1.position = transform.position + transform.right * _spawnPos;
        cube2.position = transform.position + transform.right * -_spawnPos;

        Gizmos.color = Color.red;
        Gizmos.DrawLine(this.transform.position, cube1.position);
        Gizmos.DrawLine(this.transform.position, cube2.position);
    }

    public void SpawnEnemy(Enemy x)
    {
        Vector3 y = cube1.position + (cube2.position - cube1.position) * Random.Range(0, 1f);
        Instantiate(x, y, Quaternion.identity);
    }
}
