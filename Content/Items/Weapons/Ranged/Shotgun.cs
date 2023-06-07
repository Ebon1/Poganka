using System;
using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.ID;
using Poganka.Content.Projectiles.Weapons;
using Terraria.DataStructures;
using System.Collections.Generic;
using System.Linq;

namespace Poganka.Content.Items.Weapons.Ranged
{
    public class Shotgun : GlobalItem
    {
        public override void SetDefaults(Item item)
        {
            if (item.type == ItemID.Shotgun)
            {
                item.width = 48;
                item.height = 18;
                item.crit = 0;
                item.noUseGraphic = true;
                item.noMelee = true;
                item.channel = true;
                item.DamageType = DamageClass.Ranged;
                item.useStyle = ItemUseStyleID.Shoot;
                item.shootSpeed = 1f;
            }
        }
        public override bool Shoot(Item item, Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            if (item.type == ItemID.Shotgun)
            {
                velocity.Normalize();
                Projectile.NewProjectile(source, position, velocity, ModContent.ProjectileType<ShotgunP>(), damage, knockback, player.whoAmI);
                return false;
            }
            return base.Shoot(item, player, source, position, velocity, type, damage, knockback);
        }
        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
        {
            if (item.type == ItemID.Shotgun)
            {
                int index = tooltips.IndexOf(tooltips.FirstOrDefault(x => x.Name == "Tooltip0" && x.Mod == "Terraria"));
                tooltips.Insert(index, new TooltipLine(Poganka.Instance, "Rip", "Rip and Tear"));
            }
        }
    }
}
