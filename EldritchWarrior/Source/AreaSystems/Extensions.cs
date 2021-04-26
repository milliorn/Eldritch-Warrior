using System;
using NWN.Framework.Lite;
using NWN.Framework.Lite.Enum;

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
                    return true;
                }
                objectInArea = NWScript.GetNextObjectInArea(area);
            }
            return false;
        }

        public static void DestroyCreaturesInArea(this uint objectInArea)
        {
            uint area = NWScript.GetArea(objectInArea);
            while (NWScript.GetIsObjectValid(objectInArea))
            {
                if (Convert.ToBoolean(NWScript.GetObjectType(objectInArea) == ObjectType.Creature) && Convert.ToBoolean(NWScript.GetAssociateType(objectInArea)))
                {
                    if (NWScript.GetHasInventory(objectInArea))
                    {
                        objectInArea.DestroyCreatureInventory();
                    }
                    NWScript.AssignCommand(objectInArea, () => NWScript.SetIsDestroyable());
                    NWScript.SetPlotFlag(objectInArea, false);
                    NWScript.SetImmortal(objectInArea, false);
                    NWScript.DestroyObject(objectInArea);
                }
                objectInArea = NWScript.GetNextObjectInArea(area);
            }
        }

        public static void DestroyCreatureInventory(this uint objectInArea)
        {
            uint itemInInventory = NWScript.GetFirstItemInInventory(objectInArea);
            while (NWScript.GetIsObjectValid(itemInInventory))
            {
                NWScript.AssignCommand(itemInInventory, () => NWScript.SetIsDestroyable());
                NWScript.SetPlotFlag(itemInInventory, false);
                NWScript.DestroyObject(itemInInventory);
                itemInInventory = NWScript.GetNextItemInInventory(objectInArea);
            }
        }

        public static void DestroyItemsInArea(this uint objectInArea)
        {
            uint area = NWScript.GetArea(objectInArea);
            while (NWScript.GetIsObjectValid(objectInArea))
            {
                if (Convert.ToBoolean(NWScript.GetObjectType(objectInArea) == ObjectType.Item))
                {
                    NWScript.AssignCommand(objectInArea, () => NWScript.SetIsDestroyable());
                    NWScript.SetPlotFlag(objectInArea, false);
                    NWScript.DestroyObject(objectInArea);
                }
                objectInArea = NWScript.GetNextObjectInArea(area);
            }
        }

        public static void DestroyAOEInArea(this uint objectInArea)
        {
            uint area = NWScript.GetArea(objectInArea);
            while (NWScript.GetIsObjectValid(objectInArea))
            {
                if (Convert.ToBoolean(NWScript.GetObjectType(objectInArea) == ObjectType.AreaOfEffect))
                {
                    NWScript.AssignCommand(objectInArea, () => NWScript.SetIsDestroyable());
                    NWScript.SetPlotFlag(objectInArea, false);
                    NWScript.DestroyObject(objectInArea);
                }
                objectInArea = NWScript.GetNextObjectInArea(area);
            }
        }
    }
}