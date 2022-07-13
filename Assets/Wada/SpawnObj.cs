using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnObj : MonoBehaviour
{
    [SerializeField] float _spawnPos;

    [SerializeField] GameObject cube1;
    [SerializeField] GameObject cube2;

    [SerializeField] GameObject a;

    WaveManager waveManager;

    void Start()
    {
        
    }

    private void Awake()
    {
        WaveManager.spawnObjs.Add(this);
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    private void OnDrawGizmosSelected()
    {
        cube1.transform.position = transform.position + transform.right * _spawnPos;
        cube2.transform.position = transform.position + transform.right * -_spawnPos;

        Gizmos.color = Color.red;
        Gizmos.DrawLine(this.transform.position, cube1.transform.position);
        Gizmos.DrawLine(this.transform.position, cube2.transform.position);
    }

    public void SpawnEnemy(GameObject x)
    {
        Vector3 y = cube1.transform.position + (cube2.transform.position - cube1.transform.position) * Random.Range(0, 1f);
        Instantiate(x, y, Quaternion.identity);
    }
}
