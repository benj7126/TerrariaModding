using AiDev.Items.Summon.Minions;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace AiDev.Items.Summon
{
    internal class AdamantiteCollectionSummon : ModItem
    {
        public override void SetDefaults()
        {
            Item.damage = 60;
            Item.width = 9;
            Item.height = 9;

            Item.scale = 2;

            Item.useTime = 20;
            Item.useAnimation = 20;

            Item.rare = 2;

            Item.useStyle = ItemUseStyleID.HoldUp;

            // So the weapon doesn't damage like a sword while swinging 
            Item.noMelee = true;

            // The damage type of this weapon
            Item.DamageType = DamageClass.Summon;

            // set the buff en projectile representing this summon
            Item.buffType = ModContent.BuffType<AdamantiteBuff>();
            Item.shoot = ModContent.ProjectileType<AdamantiteAxe>();
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            player.AddBuff(Item.buffType, 2, true);
            //position = Main.MouseWorld;

            return true;
        }
    }
}
