﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardewDruid.Cast;
using StardewDruid.Event;
using StardewDruid.Monster;
using StardewValley.Locations;
using StardewValley.Monsters;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StardewDruid.Map
{
    static class MonsterData
    {
        public static StardewValley.Monsters.Monster CreateMonster(int spawnMob, Vector2 spawnVector, int combatModifier, bool partyHats = false)
        {

            StardewValley.Monsters.Monster theMonster;

            /*
              
             Medium
                Start + combat 2
                (2+1)^2 * 7 = 63
                Water + combat 5
                (2+2)^2 * 10 = 160
                Stars + combat 8
                (2+3)^2 * 13 = 325
             
             Hard
                Start + combat 2
                (3+1)^2 * 7 = 112
                Water + combat 5
                (3+2)^2 * 10 = 250
                Stars + combat 8
                (3+3)^2 * 13 = 468          

             */

            switch (spawnMob)
            {

                default: // Bat

                    theMonster = new Monster.Bat(spawnVector, combatModifier);

                    break;

                case 0: // Green Slime

                    theMonster = new Slime(spawnVector, combatModifier, partyHats);

                    break;

                case 1: // Shadow Brute

                    theMonster = new Shadow(spawnVector, combatModifier);

                    break;

                case 2: // Skeleton

                    theMonster = new Monster.Skeleton(spawnVector, combatModifier, partyHats);

                    break;

                case 3: // Golem

                    theMonster = new Golem(spawnVector, combatModifier, partyHats);

                    break;

                case 4: // DustSpirit

                    theMonster = new Spirit(spawnVector, combatModifier, partyHats);

                    break;



                // ------------------ Bosses

                case 11: // Bat

                    theMonster = new BossBat(spawnVector, combatModifier);

                    break;

                case 12: // Shooter

                    theMonster = new BossShooter(spawnVector, combatModifier);

                    break;

                case 13: // Slime

                    theMonster = new BossSlime(spawnVector, combatModifier);

                    break;

                case 14: // Dino Monster

                    theMonster = new BossDragon(spawnVector, combatModifier);

                    break;

                case 15: // firebird

                    theMonster = new Firebird(spawnVector, combatModifier);

                    break;

                case 16: 

                    theMonster = new RedDragon(spawnVector, combatModifier);

                    break;


                // ------------------ Solaris

                case 51:
                case 52:
                case 53:
                case 54:
                case 55:
                case 56:

                    theMonster = new Solaris(spawnVector, combatModifier, spawnMob - 50);

                    break;

            }

            return theMonster;

        }

        public static List<System.Type> CustomMonsters()
        {
            List<System.Type> customMonsters = new()
            {
                typeof(StardewDruid.Monster.BossBat),
                typeof(StardewDruid.Monster.BossDragon),
                typeof(StardewDruid.Monster.BossShooter),
                typeof(StardewDruid.Monster.BossSlime),
                typeof(StardewDruid.Monster.Firebird),
                typeof(StardewDruid.Monster.Solaris),
            };

            return customMonsters;

        }

        public static Texture2D MonsterTexture(string characterName)
        {

            Texture2D characterTexture = Mod.instance.Helper.ModContent.Load<Texture2D>(Path.Combine("Images", characterName + ".png"));

            return characterTexture;

        }

        public static StardewValley.AnimatedSprite MonsterSprite(string characterName)
        {

            StardewValley.AnimatedSprite characterSprite = new();

            characterSprite.textureName.Value = "18465_" + characterName + "_Sprite";

            characterSprite.spriteTexture = MonsterTexture(characterName);

            characterSprite.loadedTexture = "18465_" + characterName + "_Sprite";

            if (characterName.Contains("Zero"))
            {
                
                characterSprite.SpriteHeight = 16;

                characterSprite.SpriteWidth = 16;

            }
            else if (characterName.Contains("Dragon"))
            {

                characterSprite.SpriteHeight = 64;

                characterSprite.SpriteWidth = 64;

            }
            else
            {
                characterSprite.SpriteHeight = 32;

                characterSprite.SpriteWidth = 32;

            }

            return characterSprite;

        }

    }

}
