using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using Microsoft.Xna.Framework;
using Poganka.Content.Projectiles.Weapons.Melee;
using Terraria.GameContent.UI.Elements;

namespace Poganka.Content.Items.Weapons.Melee
{
    public class IceBlade : GlobalItem
    {
        public override void SetDefaults(Item item)
        {
            if (item.type == ItemID.IceBlade)
            {
                item.useAnimation = item.useTime = 25;
                item.useTurn = false;
                item.shoot = 0;
                item.shootSpeed = 0;
                SoundStyle sound = new SoundStyle("Poganka/Assets/Sounds/daggerSlash");
                sound.Volume = 0.5f;
                item.UseSound = sound;
            }
        }
        public int attacks;
        public override bool InstancePerEntity => true;
        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
        {
            if (item.type == ItemID.IceBlade)
            {
                int index = tooltips.IndexOf(tooltips.FirstOrDefault(x => x.Name == "Tooltip0" && x.Mod == "Terraria"));
                tooltips.Remove(tooltips[index]);
                tooltips.Add(new TooltipLine(Mod, "Ice", "Ice spikes rise from the ground on each 3rd hit, if the enemy is flying it conjures the spikes from within the enemy itself instead."));
            }
        }
        public override void OnHitNPC(Item item, Player player, NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (item.type == ItemID.IceBlade)
            {
                attacks++;
                if (attacks == 3)
                {
                    if (target.collideY)
                        Projectile.NewProjectile(item.GetSource_OnHit(target), Helper.TRay.Cast(player.Center - Vector2.UnitY * 70, Vector2.UnitY, 1200, true), player.direction * Vector2.UnitX, ModContent.ProjectileType<IceBladeP1>(), item.damage, item.knockBack, player.whoAmI);
                    else
                        for (int i = 0; i < 5; i++)
                        {
                            Projectile.NewProjectileDirect(item.GetSource_OnHit(target), target.Center, Main.rand.NextVector2Unit(), ModContent.ProjectileType<IceBladeP3>(), item.damage, item.knockBack, player.whoAmI, ai1: 1, ai2: target.whoAmI).timeLeft = 30;
                        }
                    attacks = 0;
                }
            }
        }
    }
}
