﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Netcode;
using StardewDruid.Cast;
using StardewDruid.Cast.Weald;
using StardewDruid.Event;
using StardewDruid.Map;
using StardewModdingAPI;
using StardewValley;
using StardewValley.GameData.FruitTrees;
using StardewValley.Internal;
using StardewValley.Monsters;
using StardewValley.Network;
using StardewValley.Objects;
using StardewValley.TerrainFeatures;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using static System.Net.Mime.MediaTypeNames;


namespace StardewDruid.Character
{
    public class Effigy : StardewDruid.Character.Character
    {
        public List<Vector2> ritesDone;
        public int riteIcon;
        public bool showIcon;

        public Texture2D iconsTexture;

        public NetBool netCastActive = new(false);
        public NetBool netLieActive = new(false);
        public Dictionary<int, Rectangle> castFrames;

        public Effigy()
        {
        }

        public Effigy(Vector2 position, string map)
          : base(position, map, nameof(Effigy))
        {

            
        }

        protected override void initNetFields()
        {
            base.initNetFields();

            NetFields.AddField(netCastActive);
            NetFields.AddField(netLieActive);

        }

        public override void LoadOut()
        {

            LoadBase();

            characterTexture = CharacterData.CharacterTexture(Name);

            haltFrames = FrameSeries(32, 32, 0, 0, 1);

            walkFrames = FrameSeries(32, 32, 0, 128, 6, haltFrames);

            walkLeft = 1;

            walkRight = 4;

            specialScheme = SpellHandle.schemes.stars;

            specialIndicator = SpellHandle.indicators.stars;

            idleFrames = new()
            {
                [0] = new()
                {
                    new Rectangle(128, 0, 32, 32),
                    new Rectangle(160, 0, 32, 32),
                },
                [1] = new()
                {
                    new Rectangle(128, 0, 32, 32),
                    new Rectangle(160, 0, 32, 32),
                },
                [2] = new()
                {
                    new Rectangle(128, 0, 32, 32),
                    new Rectangle(160, 0, 32, 32),
                },
                [3] = new()
                {
                    new Rectangle(128, 0, 32, 32),
                    new Rectangle(160, 0, 32, 32),
                },
            };

            specialFrames = new()
            {
                [0] = new()
                {

                    new(64, 64, 32, 32),
                    new(96, 64, 32, 32),

                },
                [1] = new()
                {

                    new(64, 32, 32, 32),
                    new(96, 32, 32, 32),

                },
                [2] = new()
                {

                    new(64, 0, 32, 32),
                    new(96, 0, 32, 32),

                },
                [3] = new()
                {

                    new(64, 96, 32, 32),
                    new(96, 96, 32, 32),

                },

            };

            specialInterval = 30;
            specialCeiling = 1;
            specialFloor = 1;

            dashCeiling = 2;
            dashFloor = 1;

            dashFrames = new()
            {
                [0] = new()
                {
                    new(0, 192, 32, 32),
                    new(32, 64, 32, 32),
                    new(32, 64, 32, 32),
                },
                [1] = new()
                {
                    new(0, 160, 32, 32),
                    new(32, 32, 32, 32),
                    new(32, 32, 32, 32),
                },
                [2] = new()
                {
                    new(0, 128, 32, 32),
                    new(32, 0, 32, 32),
                    new(32, 0, 32, 32),
                },
                [3] = new()
                {
                    new(0, 224, 32, 32),
                    new(32, 96, 32, 32),
                    new(32, 96, 32, 32),
                },
            };

            sweepFrames = new()
            {
                [0] = new()
                {
                    new Rectangle(128, 320, 64, 64),
                    new Rectangle(192, 320, 64, 64),
                    new Rectangle(0, 320, 64, 64),
                    new Rectangle(64, 320, 64, 64),
                },
                [1] = new()
                {
                    new Rectangle(0, 384, 64, 64),
                    new Rectangle(64, 384, 64, 64),
                    new Rectangle(128, 384, 64, 64),
                    new Rectangle(192, 384, 64, 64),
                },
                [2] = new()
                {
                    new Rectangle(0, 256, 64, 64),
                    new Rectangle(64, 256, 64, 64),
                    new Rectangle(128, 256, 64, 64),
                    new Rectangle(192, 256, 64, 64),
                },
                [3] = new()
                {
                    new Rectangle(0, 320, 64, 64),
                    new Rectangle(64, 320, 64, 64),
                    new Rectangle(128, 320, 64, 64),
                    new Rectangle(192, 320, 64, 64),
                },
            };

            ritesDone = new List<Vector2>();

            iconsTexture = Mod.instance.Helper.ModContent.Load<Texture2D>(Path.Combine("Images", "Icons.png"));

            loadedOut = true;

        }

        public override void draw(SpriteBatch b, float alpha = 1f)
        {

            if (IsInvisible || !Utility.isOnScreen(Position, 128))
            {
                return;
            }

            if (characterTexture == null)
            {

                return;

            }

            Vector2 localPosition = getLocalPosition(Game1.viewport);

            float drawLayer = (float)StandingPixel.Y / 10000f;

            if (IsEmoting && !Game1.eventUp)
            {
                b.Draw(Game1.emoteSpriteSheet, localPosition - new Vector2(0, 160), new Microsoft.Xna.Framework.Rectangle(base.CurrentEmoteIndex * 16 % Game1.emoteSpriteSheet.Width, base.CurrentEmoteIndex * 16 / Game1.emoteSpriteSheet.Width * 16, 16, 16), Color.White, 0f, Vector2.Zero, 4f, SpriteEffects.None, drawLayer);
            }

            b.Draw(
                Game1.shadowTexture,
                localPosition + new Vector2(6, 44f),
                Game1.shadowTexture.Bounds,
                Color.White * alpha, 0f,
                Vector2.Zero,
                4f,
                SpriteEffects.None,
                drawLayer - 0.0001f
                );

            DrawIcon(b, localPosition, drawLayer);

            if (netLieActive.Value)
            {

                DrawLieDown(b, localPosition, drawLayer);

                return;

            } else if (netHaltActive.Value)
            {

                int chooseFrame = idleFrame % 8;

                if (chooseFrame < 5 || !currentLocation.IsOutdoors || netSceneActive.Value)
                {
                    b.Draw(
                        characterTexture,
                        localPosition - new Vector2(36, 72f),
                        haltFrames[netDirection.Value][0],
                        Color.White,
                        0f,
                        Vector2.Zero,
                        4.25f,
                        flip || (netDirection.Value % 2 == 0 && netAlternative.Value == 3) ? SpriteEffects.FlipHorizontally : SpriteEffects.None,
                        drawLayer
                    );

                    return;

                }

                DrawLieDown(b, localPosition, drawLayer);

            }
            else if (netSweepActive.Value)
            {

                b.Draw(
                     characterTexture,
                     localPosition - new Vector2(104, 112f),
                     sweepFrames[netDirection.Value][sweepFrame],
                     Color.White,
                     0f,
                     Vector2.Zero,
                     4.25f,
                     flip || (netDirection.Value % 2 == 0 && netAlternative.Value == 3) ? SpriteEffects.FlipHorizontally : SpriteEffects.None,
                     drawLayer
                 );

            }
            else if (netSpecialActive.Value)
            {

                b.Draw(
                    characterTexture,
                    localPosition - new Vector2(36, 72f),
                    specialFrames[netDirection.Value][specialFrame],
                    Color.White,
                    0.0f,
                    Vector2.Zero,
                    4.25f,
                    flip || (netDirection.Value % 2 == 0 && netAlternative.Value == 3) ? (SpriteEffects)1 : 0,
                    drawLayer
                );

            }
            else if (netDashActive.Value)
            {

                b.Draw(
                    characterTexture,
                    localPosition - new Vector2(36, 72f),
                    dashFrames[netDirection.Value][dashFrame],
                    Color.White,
                    0f,
                    Vector2.Zero,
                    4.25f,
                    flip || (netDirection.Value % 2 == 0 && netAlternative.Value == 3) ? SpriteEffects.FlipHorizontally : SpriteEffects.None,
                    drawLayer
                );

            }
            else
            {

                b.Draw(
                    characterTexture,
                    localPosition - new Vector2(36, 72f),
                    walkFrames[netDirection.Value][moveFrame],
                    Color.White,
                    0f,
                    Vector2.Zero,
                    4.25f,
                    flip || (netDirection.Value % 2 == 0 && netAlternative.Value == 3) ? SpriteEffects.FlipHorizontally : SpriteEffects.None,
                    drawLayer
                );

            }

        }

        public override Rectangle GetBoundingBox()
        {

            return new Rectangle((int)Position.X + 8, (int)Position.Y + 8, 48, 48);

        }

        public override Rectangle GetHitBox()
        {
            return new Rectangle((int)Position.X - 32, (int)Position.Y - 32, 128, 128);
        }

        public void DrawIcon(SpriteBatch b, Vector2 localPosition, float drawLayer)
        {
            if (netDirection.Value != 0)
            {
            
                return;
            
            }

            int riteIcon;

            switch (Mod.instance.CurrentBlessing())
            {

                case "mists":
                    riteIcon = 2;
                    break;
                case "stars":
                    riteIcon = 3;
                    break;
                case "fates":
                    riteIcon = 4;
                    break;
                case "ether":
                    riteIcon = 6;
                    break;
                default: // weald
                    riteIcon = 1;
                    break;

            }

            b.Draw(
                iconsTexture,
                localPosition + new Vector2(21, -16),
                new Rectangle((riteIcon % 4) * 8, (riteIcon / 4) * 8, 8, 8),
                Color.White*0.65f,
                0f,
                new Vector2(0, 0),
                3f,
                SpriteEffects.None,
                drawLayer + 0.0001f
            );

        }

        public void DrawLieDown(SpriteBatch b, Vector2 localPosition, float drawLayer)
        {

            int chooseFrame = idleFrame % 2;

            b.Draw(
                characterTexture,
                localPosition - new Vector2(32f, 64f),
                idleFrames[0][chooseFrame],
                Color.White,
                0f,
                Vector2.Zero,
                4f,
                flip ? SpriteEffects.FlipHorizontally : SpriteEffects.None,
                drawLayer
            );

            b.Draw(
                characterTexture,
                localPosition - new Vector2(30f, 60f),
                idleFrames[0][chooseFrame],
                Color.Black * 0.25f,
                0f,
                Vector2.Zero,
                4f,
                flip ? SpriteEffects.FlipHorizontally : SpriteEffects.None,
                drawLayer - 0.001f
            );

            return;

        }

        public override bool checkAction(Farmer who, GameLocation l)
        {

            if (Mod.instance.eventRegister.ContainsKey("transform"))
            {

                Mod.instance.CastMessage("Unable to converse while transformed");

                return false;

            }

            foreach (NPC character in currentLocation.characters)
            {

                if (character is StardewValley.Monsters.Monster monster && (double)Vector2.Distance(Position, monster.Position) <= 1280.0)
                {

                    return false;

                }

            }

            if (netDashActive.Value || netSpecialActive.Value)
            {

                return false;

            }

            if (!Mod.instance.dialogue.ContainsKey(nameof(Effigy)))
            {

                Dictionary<string, StardewDruid.Dialogue.Dialogue> dialogue = Mod.instance.dialogue;

                StardewDruid.Dialogue.Effigy effigy = new StardewDruid.Dialogue.Effigy();

                effigy.npc = this;

                dialogue[nameof(Effigy)] = effigy;

            }

            if(netSceneActive.Value && Mod.instance.dialogue[nameof(Effigy)].specialDialogue.Count == 0)
            {

                return false;

            }

            Mod.instance.dialogue[nameof(Effigy)].DialogueApproach();

            Halt();

            LookAtTarget(who.Position);

            return true;

        }

        public override void ResetActives()
        {
            base.ResetActives();

            netLieActive.Set(false);

        }

        public override void UpdateBehaviour()
        {

            base.UpdateBehaviour();

        }

        public override List<Vector2> RoamAnalysis()
        {
            
            List<Vector2> vector2List = new List<Vector2>();

            int takeABreak = 0;

            foreach (Dictionary<Vector2, StardewValley.Object> dictionary in currentLocation.Objects)
            {
                
                foreach (KeyValuePair<Vector2, StardewValley.Object> keyValuePair in dictionary)
                {
                    
                    if (keyValuePair.Value.IsScarecrow())
                    {

                        Vector2 scareVector = new(keyValuePair.Key.X * 64f, keyValuePair.Key.Y * 64f);

                        vector2List.Add(scareVector);

                        takeABreak++;

                    }

                    if (takeABreak >= 4)
                    {

                        vector2List.Add(new Vector2(-1f));

                        takeABreak = 0;

                    }

                }

                if (vector2List.Count >= 30)
                {

                    break;

                }
                    
            }
           
            List<Vector2> collection = base.RoamAnalysis();
            
            vector2List.AddRange(collection);
            
            return vector2List;
        
        }

        public override void ReachedRoamPosition()
        {
            Vector2 vector2 = new(roamVectors[roamIndex].X / 64f, roamVectors[roamIndex].Y / 64f);//Vector2.op_Division(roamVectors[roamIndex], 64f);

            string location = currentLocation.Name;

            if (Game1.currentSeason == "winter")
            {
                return;
            }

            if (ritesDone.Contains(vector2) || !currentLocation.Objects.ContainsKey(vector2) || !currentLocation.Objects[vector2].IsScarecrow())
            {
                return;
            }


            if (!Mod.instance.targetCasts.ContainsKey(currentLocation.Name))
            {

                Mod.instance.targetCasts[currentLocation.Name] = new();
            
            }

            ResetActives();

            netSpecialActive.Set(true);

            specialFrame = 1;

            specialTimer = 30;

            bool Reseed = !Mod.instance.EffectDisabled("Seeds");

            List<CastHandle> casts = new();

            for (int level = 1; level < (Mod.instance.PowerLevel()+2); level++)
            {
                
                foreach (Vector2 tilesWithinRadius in ModUtility.GetTilesWithinRadius(currentLocation, vector2, level))
                {

                    if (Mod.instance.targetCasts[currentLocation.Name].ContainsKey(tilesWithinRadius))
                    {

                        continue;

                    }

                    if (currentLocation.terrainFeatures.ContainsKey(tilesWithinRadius) && currentLocation.terrainFeatures[tilesWithinRadius].GetType().Name.ToString() == "HoeDirt")
                    {
                        StardewDruid.Cast.Weald.Crop cropHustle = new(tilesWithinRadius, Reseed, true);

                        cropHustle.targetLocation = currentLocation;

                        casts.Add(cropHustle);

                        Mod.instance.targetCasts[currentLocation.Name][tilesWithinRadius] = "Crop";

                    }

                }
            
            }
            
            if (location == Game1.player.currentLocation.Name && Utility.isOnScreen(Position, 128))
            {
                
                ModUtility.AnimateDecoration(currentLocation, Position, "weald", 1f);
                
                Game1.player.currentLocation.playSound("discoverMineral", Position, 1000);
            
            }

            for(int radius = 0; radius < 9; radius++)
            {

                List<Vector2> radii = ModUtility.GetTilesWithinRadius(currentLocation, vector2, radius);

                foreach(Vector2 radiiVector in radii)
                {

                    if (!Mod.instance.targetCasts.ContainsKey(location))
                    {

                        Mod.instance.targetCasts[location] = ModUtility.LocationTargets(currentLocation);

                    }

                    if (Mod.instance.targetCasts[location].ContainsKey(radiiVector))
                    {
                        continue;
                    }

                    if (currentLocation.terrainFeatures.ContainsKey(radiiVector))
                    {

                        if (currentLocation.terrainFeatures[radiiVector] is StardewValley.TerrainFeatures.FruitTree fruitTree)
                        {

                            if (fruitTree.stump.Value)
                            {

                                continue;

                            }

                            if(fruitTree.growthStage.Value < 4)
                            {
                                
                                fruitTree.dayUpdate();

                            }
                            else if (!Game1.IsWinter)
                            {
                                Debris debris;

                                if ((int)fruitTree.struckByLightningCountdown.Value > 0)
                                {
                                    debris = new Debris("382", new Vector2(radiiVector.X * 64f + 32f, (radiiVector.Y - 3f) * 64f + 32f), Game1.player.Position)
                                    {
                                        itemQuality = 0,
                                    };
                                }
                                else
                                {

                                    FruitTreeData data = fruitTree.GetData();

                                    Item item = null;

                                    if (data?.Fruit != null)
                                    {

                                        foreach (FruitTreeFruitData item2 in data.Fruit)
                                        {

                                            item = ItemQueryResolver.TryResolveRandomItem(item2, new ItemQueryContext(currentLocation, null, null), avoidRepeat: false, null, null, null, delegate (string query, string error){});

                                        }

                                    }

                                    if (item == null)
                                    {

                                        continue;

                                    }

                                    int itemQuality = 0;

                                    if ((int)fruitTree.daysUntilMature.Value <= -112)
                                    {
                                        itemQuality = 1;
                                    }

                                    if ((int)fruitTree.daysUntilMature.Value <= -224)
                                    {
                                        itemQuality = 2;
                                    }

                                    if ((int)fruitTree.daysUntilMature.Value <= -336)
                                    {
                                        itemQuality = 4;
                                    }

                                    if ((int)fruitTree.struckByLightningCountdown.Value > 0)
                                    {
                                        itemQuality = 0;
                                    }

                                    debris = new Debris(item, new Vector2(radiiVector.X * 64f + 32f, (radiiVector.Y - 3f) * 64f + 32f), Game1.player.Position)
                                    {
                                        itemQuality = itemQuality,
                                    };

                                }

                                debris.Chunks[0].xVelocity.Value += (float)Game1.random.Next(-10, 11) / 10f;

                                debris.chunkFinalYLevel = (int)(radiiVector.Y * 64f + 64f);

                                currentLocation.debris.Add(debris);

                            }

                            Mod.instance.targetCasts[location][radiiVector] = "Tree";

                        }

                    }

                }

            }

            if (casts.Count != 0)
            {

                foreach (Cast.CastHandle effect in casts)
                {

                    effect.CastEffect();

                }

            }
            
            ritesDone.Add(vector2);
        
        }

    }

}
