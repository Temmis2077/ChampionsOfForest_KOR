﻿using UnityEngine;
namespace ChampionsOfForest
{
    public class EnemyHealthMod : EnemyHealth
    {
        public EnemyProgression progression;

        //creates progression
        protected override void Start()
        {
            base.Start();
            progression = gameObject.AddComponent<EnemyProgression>();
            progression._Health = this;
            progression._AI = ai;
            progression.entity = entity;
            progression.setup = setup;
        }

        //changes how damage is calculated to include armor and abilities
        public override void Hit(int damage)
        {
            HitPhysical(damage);
        }

        public void HitPhysical(int damage)
        {

            int dmg = progression.ClampDamage(false, damage);
            HitReal(dmg);
        }

        public override void HitReal(int damage)
        {
            Network.NetworkManager.SendHitmarker(transform.position + Vector3.up, damage);

            if (Health - damage < 1)
            {
                if (!progression.OnDie())
                {
                    Health -= damage;
                    return;
                }
            }
            base.HitReal(damage);

        }
        protected override void dieExplode()
        {
            if (progression.OnDie())
            {
                base.dieExplode();
            }
        }
        protected override void DieTrap(int type)
        {
            if (progression.OnDie())
            {
                base.DieTrap(type);
            }
        }
        public override void Die()
        {
            if (progression.OnDie())
            {
                base.Die();
            }
        }
    }

}