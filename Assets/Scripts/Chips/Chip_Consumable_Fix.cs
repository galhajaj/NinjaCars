using System.Collections;
using System.Collections.Generic;

public class Chip_Consumable_Fix : Chip
{
    protected override void activate()
    {
        Tank.Instance.LifeData.Life++;
    }
}
