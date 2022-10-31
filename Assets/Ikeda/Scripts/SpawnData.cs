using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SpawnData
{
    public SpawnData(WaveData[] waves)
    {
        WaveData = waves;
    }

    public SpawnData()
    {
        WaveData = null;
    }

    /// <summary>ウェーブのデータを入れる配列</summary>
    public WaveData[] WaveData = default;

    public WaveData this[int index]
    {
        get
        {
            return this.WaveData[index];
        }
        set
        {
            this.WaveData[index] = value;
        }
    }

    public int Length
    {
        get => WaveData.Length;
        set
        {
            WaveData[] waves = new WaveData[value];
            for(int i = 0; i < Mathf.Min(value, WaveData.Length); i++)
            {
                waves[i] = WaveData[i];
            }
            WaveData = waves;
        }
    }

    static public implicit operator WaveData[] (SpawnData spawn)
    {
        return spawn.WaveData;
    }

    static public implicit operator SpawnData(WaveData[] waves)
    {
        return new SpawnData(waves);
    }
}
