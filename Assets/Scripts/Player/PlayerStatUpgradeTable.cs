using UnityEngine;

/// <summary>
/// 능력치별 업그레이드 테이블 ScriptableObject.
/// </summary>
[CreateAssetMenu(fileName = "PlayerStatUpgradeTable", menuName = "ScriptableObjects/PlayerStatUpgradeTable")]
public class PlayerStatUpgradeTable : ScriptableObject
{
    /// <summary>
    /// 능력치 종류
    /// </summary>
    public PlayerStatType statType;

    /// <summary>
    /// 1회 업그레이드 시 증가량(고정)
    /// </summary>
    public float upgradeValue;

    /// <summary>
    /// 1단계 업그레이드 비용(기본값)
    /// </summary>
    public int baseCost;

    /// <summary>
    /// 업그레이드 비용 배율(예: 1.2 = 20% 증가)
    /// </summary>
    public float costMultiplier = 1.2f;
} 