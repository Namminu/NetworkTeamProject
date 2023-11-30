using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaxItemInfo : MonoBehaviour
{
    public int maxFeathers;
    public int maxBackPacks;
    public int maxScrolls;

    [Header("개발자 전용")]
    [Tooltip("체크 시 모든 아이템 박스에서 무조건 아이템이 나오도록 보장해줍니다")]
    public bool isAlwaysSpawnObjects = false;
}
