using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace AiDev.Items.Summon.Minions
{
    internal class AdamantiteBattleaxe : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            // Denotes that this projectile is a pet or minion
            Main.projPet[Projectile.type] = true;

            // This is needed so your minion can properly spawn when summoned and replaced when other minions are summoned
            ProjectileID.Sets.MinionSacrificable[Projectile.type] = true;
        }

        public override void SetDefaults()
        {
            // Only controls if it deals damage to enemies on contact (more on that later)
            Projectile.friendly = true;

            // Only determines the damage type
            Projectile.minion = true;

            // Amount of slots this minion occupies from the total minion slots available to the player (more on that later)
            Projectile.minionSlots = 1f;

            // Needed so the minion doesn't despawn on collision with enemies or tiles
            Projectile.penetrate = -1;
        }

        public override bool? CanCutTiles()
        {
            return false;
        }

        public override bool MinionContactDamage()
        {
            return true;
        }

        public override void AI()
        {
            Player player = Main.player[Projectile.owner];

            if (player.dead || !player.active)
            {
                player.ClearBuff(ModContent.BuffType<AdamantiteBuff>());
            }

            if (player.HasBuff(ModContent.BuffType<AdamantiteBuff>()))
            {
                Projectile.timeLeft = 2;
            }
        }
    }
}
