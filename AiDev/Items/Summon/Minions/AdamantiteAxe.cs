using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace AiDev.Items.Summon.Minions
{
    internal class AdamantiteAxe : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            // Denotes that this projectile is a pet or minion
            Main.projPet[Projectile.type] = true;

            // This is needed so your minion can properly spawn when summoned and replaced when other minions are summoned
            ProjectileID.Sets.MinionSacrificable[Projectile.type] = true;
            ProjectileID.Sets.MinionTargettingFeature[Projectile.type] = true;
        }

        public override void SetDefaults()
        {
            Projectile.damage = 4;
            Projectile.scale = 2;

            Projectile.width = 18;
            Projectile.height = 18;

            Projectile.TopLeft = new Vector2(9, 9);

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

            Vector2 idlePos = player.Center;
            idlePos.Y -= 48f;

            float minonPosOffsetX = (10 + Projectile.minionPos * 40) * -player.direction;
            idlePos.X += minonPosOffsetX;

            Vector2 vecToIdle = idlePos - Projectile.Center;
            float distToIdlePos = vecToIdle.Length();

            float distFromTarget = 700f;
            Vector2 targetCenter = Projectile.position;
            bool foundTarget = false;

            if (player.HasMinionAttackTargetNPC)
            {
                NPC npc = Main.npc[player.MinionAttackTargetNPC];
                float between = Vector2.Distance(npc.Center, Projectile.Center);
                // Reasonable distance away so it doesn't target across multiple screens
                if (between < 2000f)
                {
                    distFromTarget = between;
                    targetCenter = npc.Center;
                    foundTarget = true;
                }
            }
            if (!foundTarget)
            {
                for (int i = 0; i < Main.maxNPCs; i++)
                {
                    NPC npc = Main.npc[i];
                    if (npc.CanBeChasedBy())
                    {
                        float between = Vector2.Distance(npc.Center, Projectile.Center);
                        bool closest = Vector2.Distance(Projectile.Center, targetCenter) > between;
                        bool inRange = between < distFromTarget;

                        if ((closest && inRange) || !foundTarget)
                        {
                            distFromTarget = between;
                            targetCenter = npc.Center;
                            foundTarget = true;
                        }
                    }
                }
            }

            Projectile.friendly = foundTarget;

            if (foundTarget)
            {
                // if it should chase a taget
                float speed = 8f;
                float inertia = 40f;
                Vector2 direction = targetCenter - Projectile.position;
                direction.Normalize();
                direction *= speed;
                Projectile.velocity = (Projectile.velocity * (inertia - 1) + direction) / inertia;

                Projectile.rotation -= Projectile.velocity.Length()/10f;
                if (Projectile.rotation > MathHelper.Pi * 6)
                    Projectile.rotation -= MathHelper.Pi * 2;
            }
            else
            {
                // if it should do idle animation thingy
                float speed = 8f;
                float inertia = 40f;

                float targetRot = MathHelper.Pi*2;

                Vector2 direction = vecToIdle;
                direction.Normalize();
                direction *= speed;
                Projectile.velocity = (Projectile.velocity * (inertia - 1) + direction) / inertia;


                if (targetRot > Projectile.rotation)
                    Projectile.rotation -= Projectile.velocity.Length() / 10f * -1;
                else
                    Projectile.rotation -= Projectile.velocity.Length() / 10f;
            }
        }
    }
}
