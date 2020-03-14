﻿using robotManager.Helpful;
using wManager.Wow.Class;

public static class VendorDB
{
    public static Npc OGTailoringSupplies = new Npc();
    public static Npc OGTailoringTrainer = new Npc();

    public static Npc ThrallmarTailoringTrainer = new Npc();
    public static Npc ThrallmarTailoringSupplies = new Npc();

    public static Npc ShattrathTailoringSupplies = new Npc();

    static VendorDB()
    {
        OGTailoringSupplies.Entry = 3364;
        OGTailoringSupplies.Position = new Vector3(1792.65, -4565.39, 23.0066);
        
        OGTailoringTrainer.Entry = 3363;
        OGTailoringTrainer.Position = new Vector3(1806.85, -4573.32, 23.00661);

        ThrallmarTailoringTrainer.Entry = 18749;
        ThrallmarTailoringTrainer.Position = new Vector3(204.177, 2617.73, 87.2837);
        ThrallmarTailoringTrainer.GossipOption = 1;

        ThrallmarTailoringSupplies.Entry = 18749;
        ThrallmarTailoringSupplies.Position = new Vector3(204.177, 2617.73, 87.2837);
        ThrallmarTailoringSupplies.GossipOption = 2;

        ShattrathTailoringSupplies.Entry = 19213;
        ShattrathTailoringSupplies.Position = new Vector3(-2077.26, 5270.03, -37.3236);
    }
}