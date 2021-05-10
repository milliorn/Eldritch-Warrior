using System;
using NWN.Framework.Lite;
using NWN.Framework.Lite.Enum;
using static NWN.Framework.Lite.NWScript;

namespace EldritchWarrior.Source.AreaSystems
{
	public class Cleanup
	{
		[ScriptHandler("are_exit_cleanup")]
		public static void Map()
		{
			uint pc = GetExitingObject();
			uint area = GetArea(pc);
			uint objectInArea = GetFirstObjectInArea(area);

			if (!area.PlayersRemainInArea())
			{
				while (GetIsObjectValid(objectInArea))
				{
					switch (GetObjectType(objectInArea))
					{
						case ObjectType.AreaOfEffect:
						case ObjectType.Creature:
						case ObjectType.Item:DestroyObject(objectInArea); break;
						case ObjectType.Door: PlayAnimation(AnimationType.DoorClose); break;
						case ObjectType.Placeable: objectInArea.DestroyInventory(); break;
					}
					objectInArea = GetNextObjectInArea(area);
				}
			}
		}
	}
}
