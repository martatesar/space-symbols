using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public static class Helper
{
    public static Dictionary<int, T> CreateAnimatorStateDictionary<T>() where T : struct
    {
        if (!typeof(T).IsEnum) throw new ArgumentException("T must be an enumerated type");

        var dict = new Dictionary<int, T>();
        foreach (T item in Enum.GetValues(typeof(T)))
        {
            dict.Add(Animator.StringToHash(item.ToString()), item);
        }
        return dict;
    }
}

