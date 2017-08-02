public enum DamageType
{
    Physical, // Blocked by defense.
    Magical,  // Blocked by magical resistance.
    Absolute, // Blocked by nothing. In other words, this type of damage will always deal it's EXACT amount.
}
