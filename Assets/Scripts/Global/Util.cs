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
}
