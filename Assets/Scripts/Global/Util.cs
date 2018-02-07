using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Util //유용한 메서드 모음
{
    public static int CalculDigit(int value) //자릿수 계산 메서드
    {
        if(value < 0)
        {
            Debug.LogError("value must be over zero");
            return 0;
        }

        int digit = 0;

        while (value != 0)
        {
            value /= 10;
            ++digit;
        }

        return digit;
    }

    public static int[] GetRandomIndex(int start, int end) //랜덤한 인덱스 리스트를 넘겨주는 메서드
    {
        int[] index = new int[end - start];

        for (int i = 0; i < index.Length; ++i)
            index[i] = i + 1;

        for (int i = 0; i < 20; ++i)
        {
            int src = Random.Range(0, index.Length);
            int dest = Random.Range(0, index.Length);

            int temp = index[src];
            index[src] = index[dest];
            index[dest] = temp;
        }

        return index;
    }
}
