using NWN.Framework.Lite.Enum;
using static NWN.Framework.Lite.NWScript;

namespace EldritchWarrior.Source.Shifter
{
    public class Save
    {
        public static void Character()
        {
            int spell = Extensions.ScanForPolymorphEffect(OBJECT_SELF);
            if (spell < 0) return; // If not applied by a spell, return

            // Note: Druid wildshapes will have their duration renewed!
            // There is no way to get the duration left on an effect.
            // Change the number 396 to 405 to not allow druid wildshapes' properties to be reapplied.
            else if ((spell >= (int)SpellType.Shapechange && spell <= (int)SpellType.PolymorphSelf)
                    || spell == (int)SpellType.TensersTransformation) return;
            else
            {
                DelayCommand(0.01f, () => OBJECT_SELF.ReFireSpell(spell));
            }
        }
    }
}