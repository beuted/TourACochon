using System;

public enum PigPerks
{
  None = 0,
  Cleaned = 1,
  StomachFull = 1 << 1,
  Vaccinated = 1 << 2,
  Strong = 1 << 3,
  Happy = 1 << 4,
  Bacon = 1 << 5,
}
