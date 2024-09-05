using NaughtyAttributes;
using UnityEngine;

public class IgnoreTickDamage : MonoBehaviour
{
    [SerializeField] private TickDamage.TickDamageType tickDamageTypetoResist;

    [SerializeField][MaxValue(100)][MinValue(0)] int resistance;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent(out Status theStat))
        {
            switch (tickDamageTypetoResist)
            {
                case TickDamage.TickDamageType.Cold:
                    theStat.ColdTickResistanceExternal = resistance;
                    break;
                case TickDamage.TickDamageType.Flame:
                    theStat.FlameTickResistanceExternal = resistance;
                    break;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.TryGetComponent(out Status theStat))
        {
            switch (tickDamageTypetoResist)
            {
                case TickDamage.TickDamageType.Cold:
                    theStat.ColdTickResistanceExternal = 0;
                    break;
                case TickDamage.TickDamageType.Flame:
                    theStat.FlameTickResistanceExternal = 0;
                    break;
            }
        }
    }
}
