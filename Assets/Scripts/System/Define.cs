using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Define : MonoBehaviour
{
    public enum StatNum         // 스탯 구분
    {
        Atk,
        Spd,
        Gold,
        Life,
        None,
    }

    public enum ResultNum       // 결과 지표 구분
    {
        Time,
        Kill,
        Gold,
        Spend,
        None,
    }

    public enum TextBoxNum      // 텍스트 박스 구분
    {
        Timer,
        Gold,
    }
}
