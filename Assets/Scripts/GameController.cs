using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : Singleton<GameController>
{
    public static bool IsPlaying;

    public static T GetItemFromArray<T>(T[] array)
    {
        return array[Random.Range(0, array.Length)];
    }

    public static int GetWeightedRandomFromArray(int[] array)
    {
        int max = 0;
        for (int i = 0; i < array.Length; i++)
        {
            max += array[i];
        }

        var a = Random.Range(0, max);

        var offset = 0;
        int actualIndex = 0;

        for (int i = 0; i < array.Length; i++)
        {
            if (array[i] > 0) actualIndex = i;
            offset += array[i];            
            if (a < offset) return actualIndex;            
        }

        return array[array.Length - 1];
    }

    private static string[] _suffixArray =
    {
        "K", "M", "B", "T", "AA", "BB", "CC", "DD", "EE", "FF"
    };

    public static string GetPrettyDouble(double value)
    {
        var initial = value;

        if (value < 1000)
        {
            return value.ToString("N0");
        }

        for (int i = 0; i < _suffixArray.Length; i++)
        {
            value /= 1000;

            if (value < 1000) return value.ToString("N2") + _suffixArray[i];
        }

        return initial.ToString("N0");
    }

    //public static int GetWeightedRandomFrom2DArray(int[][] array)
    //{
    //    int max = 0;
    //    for (int i = 0; i < array.Length; i++)
    //    {
    //        max += array[i][0];
    //    }

    //    var a = Random.Range(0, max);

    //    var offset = 0;
    //    for (int i = 0; i < array.Length; i++)
    //    {
    //        offset += array[i][0];
    //        if (a < offset) return i;
    //    }

    //    return array[array.Length - 1][0];
    //}
}
