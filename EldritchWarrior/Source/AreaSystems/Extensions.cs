using NWN.Framework.Lite;

namespace EldritchWarrior.Source.AreaSystems
{
    public static class Extensions
    {
        public static bool PlayersRemainInArea(this uint objectInArea)
        {
            uint area = NWScript.GetArea(objectInArea);
            while (NWScript.GetIsObjectValid(objectInArea))
            {
                if (NWScript.GetIsPC(objectInArea))
                {
                    System.Console.WriteLine("true");
                    return true;
                }
                objectInArea = NWScript.GetNextObjectInArea();
            }
            System.Console.WriteLine("false");
            return false;
        }

        public static void DestroyAOE(this uint objectInArea)
        {
            NWScript.AssignCommand(objectInArea, () => NWScript.SetIsDestroyable());
            NWScript.SetPlotFlag(objectInArea, false);
            NWScript.DestroyObject(objectInArea);
        }

        public static void DestroyCreatureInArea(this uint objectInArea)
        {
            uint area = NWScript.GetArea(objectInArea);
            if (NWScript.GetHasInventory(objectInArea))
            {
                objectInArea.DestroyInventory();
            }
            NWScript.AssignCommand(objectInArea, () => NWScript.SetIsDestroyable());
            NWScript.SetPlotFlag(objectInArea, false);
            NWScript.SetImmortal(objectInArea, false);
            NWScript.DestroyObject(objectInArea);
        }

        public static void DestroyInventory(this uint objectInArea)
        {
            uint itemInInventory = NWScript.GetFirstItemInInventory();
            while (NWScript.GetIsObjectValid(itemInInventory))
            {
                NWScript.AssignCommand(itemInInventory, () => NWScript.SetIsDestroyable());
                NWScript.SetPlotFlag(itemInInventory, false);
                NWScript.DestroyObject(itemInInventory);
                itemInInventory = NWScript.GetNextItemInInventory();
            }
        }

        public static void DestroyItem(this uint objectInArea)
        {
            NWScript.AssignCommand(objectInArea, () => NWScript.SetIsDestroyable());
            NWScript.SetPlotFlag(objectInArea, false);
            NWScript.DestroyObject(objectInArea);
        }
    }
}