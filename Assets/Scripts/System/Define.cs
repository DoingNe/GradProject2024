using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Define : MonoBehaviour
{
    public enum StatNum         // ���� ����
    {
        Atk,
        Spd,
        Gold,
        Life,
        None,
    }

    public enum ResultNum       // ��� ��ǥ ����
    {
        Time,
        Kill,
        Gold,
        Spend,
        None,
    }

    public enum TextBoxNum      // �ؽ�Ʈ �ڽ� ����
    {
        Timer,
        Gold,
    }
}
