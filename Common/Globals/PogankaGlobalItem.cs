using System;
using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.ID;
using Terraria.Audio;
using ReLogic.Utilities;
using Terraria.DataStructures;

namespace Poganka.Common.Globals
{
    public class PogankaGlobalItemNonInstanced : GlobalItem
    {
        public override bool InstancePerEntity => false;
    }
    public class PogankaGlobalItemInstanced : GlobalItem
    {
        public override bool InstancePerEntity => true;
        int soundDelay;
        public override void UpdateInventory(Item item, Player player)
        {
            if (soundDelay > 0)
                soundDelay--;
        }
        public override bool CanUseItem(Item item, Player player)
        {
            if (item.useAmmo == AmmoID.Bullet && !player.HasAmmo(item) && soundDelay <= 0)
            {
                SoundStyle emptyGun = new SoundStyle("Poganka/Assets/Sounds/EmptyGun");
                emptyGun.Volume = 0.75f;
                SoundEngine.PlaySound(emptyGun, player.Center);
                soundDelay = 30 + item.useAnimation;
                player.itemAnimation = item.useAnimation / 2;
                player.itemTime = item.useAnimation / 2;
            }
            return base.CanUseItem(item, player);
        }
    }
}
