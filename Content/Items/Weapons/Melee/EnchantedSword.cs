using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Mono.Cecil;
using Poganka.Content.Projectiles.Weapons.Melee;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.PlayerDrawLayer;
using Terraria.Audio;

namespace Poganka.Content.Items.Weapons.Melee
{
    public class EnchantedSword : GlobalItem
    {
        public override void SetDefaults(Item item)
        {
            if (item.type == ItemID.EnchantedSword)
            {
                item.useAnimation = item.useTime = 25;
                item.useTurn = false;
                SoundStyle sound = new SoundStyle("Poganka/Assets/Sounds/daggerSlash");
                sound.Volume = 0.5f;
                item.UseSound = sound;
            }
        }
        public override bool InstancePerEntity => true;
        int times = 0;
        public override void HoldItem(Item item, Player player)
        {
            if (player.itemTime == 2)
            {
                player.itemTime = 0;
                player.itemAnimation = 0;
            }
        }
        public override bool CanShoot(Item item, Player player)
        {
            if (item.type == ItemID.EnchantedSword)
                return true;
            return base.CanShoot(item, player);
        }
        public override bool Shoot(Item item, Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            if (item.type == ItemID.EnchantedSword)
            {
                Projectile a = Projectile.NewProjectileDirect(source, position, player.direction * Vector2.UnitX, ModContent.ProjectileType<EnchantedSwordP>(), damage, knockback, player.whoAmI);
                a.ai[1] = item.useAnimation;
                times++;
                if (times >= 3)
                {
                    //for (int i = -1; i < 2; i++)
                    {
                        //  if (i == 0) continue;
                        NPC target = null;
                        if (target == null ? true : (!target.active || target.Center.Distance(player.Center) > 600))
                            foreach (NPC npc in Main.npc)
                            {
                                if (npc.active && !npc.friendly && !npc.dontTakeDamage && npc.Center.Distance(player.Center) < 400 && npc.type != NPCID.TargetDummy)
                                {
                                    target = npc;
                                    break;
                                }
                            }
                        if (target != null && target.active)
                        {
                            SoundEngine.PlaySound(new SoundStyle("Poganka/Assets/Sounds/sparkle"), target.Center + Helper.FromAToB(target.Center, player.Center) * 30);
                            Projectile.NewProjectileDirect(source, target.Center + Helper.FromAToB(target.Center, player.Center) * 30, Helper.FromAToB(player.Center, target.Center), ModContent.ProjectileType<EnchantedSwordP2>(), damage, knockback, player.whoAmI, ai1: Main.rand.Next(new int[] { 1, -1 }));
                        }
                    }
                    times = 0;
                }
                return false;
            }
            return base.Shoot(item, player, source, position, velocity, type, damage, knockback);
        }
    }
}
