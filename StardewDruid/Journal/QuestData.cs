﻿
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Media;
using StardewDruid.Cast;
using StardewDruid.Cast.Effect;
using StardewDruid.Character;
using StardewDruid.Data;
using StardewDruid.Dialogue;
using StardewDruid.Event.Relics;
using StardewDruid.Location;
using StardewValley;
using StardewValley.Buildings;
using StardewValley.GameData.Buildings;
using StardewValley.GameData.Characters;
using StardewValley.GameData.Movies;
using StardewValley.Locations;
using StardewValley.Tools;
using System.Collections.Generic;

namespace StardewDruid.Journal
{
    public static class QuestData
    {

        public enum triggerLocales
        {
            FarmCave,
            Beach,
            CommunityCenter,
            UndergroundMine20,
            Town,
            Forest,
            Mountain,
            UndergroundMine77377,
        }

        public static Dictionary<string, Quest> QuestList()
        {

            Dictionary<string, Quest> quests = new();

            // =====================================================
            // APPROACH EFFIGY

            Quest approachEffigy = new()
            {

                name = QuestHandle.approachEffigy,

                type = Quest.questTypes.approach,

                icon = IconData.displays.effigy,

                // -----------------------------------------------

                give = Quest.questGivers.none,

                trigger = true,

                triggerLocation = triggerLocales.FarmCave.ToString(),

                triggerRite = Rite.rites.none,

                origin = new Vector2(7, 7) * 64,

                // -----------------------------------------------

                title = Mod.instance.Helper.Translation.Get("QuestData.32"),

                description = Mod.instance.Helper.Translation.Get("QuestData.34") +
                    ReactionData.quotationMark + Mod.instance.Helper.Translation.Get("QuestData.35") +
                    Mod.instance.Helper.Translation.Get("QuestData.36") + ReactionData.quotationMark,

                instruction = Mod.instance.Helper.Translation.Get("QuestData.38").Tokens(new { button = Mod.instance.Config.riteButtons.ToString() }),

                explanation = Mod.instance.Helper.Translation.Get("QuestData.41") +
                    Mod.instance.Helper.Translation.Get("QuestData.42") +
                    Mod.instance.Helper.Translation.Get("QuestData.43"),

                progression = null,

                // -----------------------------------------------

            };

            quests.Add(approachEffigy.name, approachEffigy);

            // =====================================================
            // SWORD WEALD

            Quest swordWeald = new()
            {

                name = QuestHandle.swordWeald,

                type = Quest.questTypes.sword,

                icon = IconData.displays.weald,

                // -----------------------------------------------

                give = Quest.questGivers.dialogue,

                trigger = true,

                triggerLocation = Location.LocationData.druid_grove_name,

                triggerTime = 0,

                triggerRite = Rite.rites.none,

                origin = new Vector2(21, 10) * 64,

                // -----------------------------------------------

                title = Mod.instance.Helper.Translation.Get("QuestData.81"),

                description = Mod.instance.Helper.Translation.Get("QuestData.83") +
                Mod.instance.Helper.Translation.Get("QuestData.84"),

                instruction = Mod.instance.Helper.Translation.Get("QuestData.86").Tokens(new { button = Mod.instance.Config.riteButtons.ToString() }),

                explanation = Mod.instance.Helper.Translation.Get("QuestData.89"),

                progression = null,

                requirement = 0,

                reward = 250,

                // -----------------------------------------------

                before = new()
                {

                    [CharacterHandle.characters.Effigy] = new()
                    {
                        prompt = true,
                        intro = Mod.instance.Helper.Translation.Get("QuestData.104") +
                            Mod.instance.Helper.Translation.Get("QuestData.105") +
                            Mod.instance.Helper.Translation.Get("QuestData.106"),

                    }
                },

                after = new()
                {
                    [CharacterHandle.characters.Effigy] = new()
                    {
                        prompt = true,
                        intro = Mod.instance.Helper.Translation.Get("QuestData.116"),
                        responses = new()
                        {
                            Mod.instance.Helper.Translation.Get("QuestData.119"),
                            Mod.instance.Helper.Translation.Get("QuestData.120"),
                            Mod.instance.Helper.Translation.Get("QuestData.121")
                        },
                        answers = new(){
                            Mod.instance.Helper.Translation.Get("QuestData.124"),
                        }
                    }
                },

                // -----------------------------------------------

            };

            quests.Add(swordWeald.name, swordWeald);

            // =====================================================
            // HERBALISM

            Quest herbalism = new()
            {

                name = QuestHandle.herbalism,

                icon = IconData.displays.chaos,

                type = Quest.questTypes.miscellaneous,

                give = Quest.questGivers.none,

                title = Mod.instance.Helper.Translation.Get("QuestData.149"),

                description = Mod.instance.Helper.Translation.Get("QuestData.151"),

                instruction = Mod.instance.Helper.Translation.Get("QuestData.153"),

                before = new()
                {

                    [CharacterHandle.characters.Effigy] = new()
                    {
                        prompt = true,
                        intro = Mod.instance.Helper.Translation.Get("QuestData.161") +
                        Mod.instance.Helper.Translation.Get("QuestData.162"),

                    }
                },

            };

            quests.Add(herbalism.name, herbalism);

            // =====================================================
            // WEALD LESSONS

            Quest wealdOne = new()
            {

                name = QuestHandle.wealdOne,

                icon = IconData.displays.weald,

                type = Quest.questTypes.lesson,

                give = Quest.questGivers.none,

                title = Mod.instance.Helper.Translation.Get("QuestData.185"),

                description = Mod.instance.Helper.Translation.Get("QuestData.187"),

                instruction = Mod.instance.Helper.Translation.Get("QuestData.189").Tokens(new { button = Mod.instance.Config.effectsButtons.ToString() }),

                progression = Mod.instance.Helper.Translation.Get("QuestData.192"),

                requirement = 100,

                reward = 1000,

                before = new()
                {

                    [CharacterHandle.characters.Effigy] = new()
                    {
                        prompt = true,
                        intro = Mod.instance.Helper.Translation.Get("QuestData.204") +
                        Mod.instance.Helper.Translation.Get("QuestData.205"),

                    }
                },

            };

            quests.Add(wealdOne.name, wealdOne);

            // -----------------------------------------------------

            Quest wealdTwo = new()
            {

                name = QuestHandle.wealdTwo,

                icon = IconData.displays.weald,

                type = Quest.questTypes.lesson,

                give = Quest.questGivers.dialogue,

                title = Mod.instance.Helper.Translation.Get("QuestData.227"),

                description = Mod.instance.Helper.Translation.Get("QuestData.229"),

                instruction = Mod.instance.Helper.Translation.Get("QuestData.231"),

                progression = Mod.instance.Helper.Translation.Get("QuestData.233"),

                requirement = 20,

                reward = 1000,

                before = new()
                {

                    [CharacterHandle.characters.Effigy] = new()
                    {
                        prompt = true,
                        intro = Mod.instance.Helper.Translation.Get("QuestData.245") +
                        Mod.instance.Helper.Translation.Get("QuestData.246") +
                        Mod.instance.Helper.Translation.Get("QuestData.247"),

                    }
                },

            };

            quests.Add(wealdTwo.name, wealdTwo);

            // -----------------------------------------------------

            Quest wealdThree = new()
            {

                name = QuestHandle.wealdThree,

                icon = IconData.displays.weald,

                type = Quest.questTypes.lesson,

                give = Quest.questGivers.dialogue,

                title = Mod.instance.Helper.Translation.Get("QuestData.269"),

                description = Mod.instance.Helper.Translation.Get("QuestData.271"),

                instruction = Mod.instance.Helper.Translation.Get("QuestData.273") +

                Mod.instance.Helper.Translation.Get("QuestData.275") +

                Mod.instance.Helper.Translation.Get("QuestData.277"),

                progression = Mod.instance.Helper.Translation.Get("QuestData.279"),

                requirement = 5,

                reward = 1000,

                before = new()
                {

                    [CharacterHandle.characters.Effigy] = new()
                    {
                        prompt = true,
                        intro = Mod.instance.Helper.Translation.Get("QuestData.291") +
                        Mod.instance.Helper.Translation.Get("QuestData.292") +
                        Mod.instance.Helper.Translation.Get("QuestData.293"),

                    }
                },

            };

            quests.Add(wealdThree.name, wealdThree);

            // -----------------------------------------------------

            Quest wealdFour = new()
            {

                name = QuestHandle.wealdFour,

                icon = IconData.displays.weald,

                type = Quest.questTypes.lesson,

                give = Quest.questGivers.dialogue,

                title = Mod.instance.Helper.Translation.Get("QuestData.315"),

                description = Mod.instance.Helper.Translation.Get("QuestData.317"),

                instruction = Mod.instance.Helper.Translation.Get("QuestData.319") +
                Mod.instance.Helper.Translation.Get("QuestData.320"),

                progression = Mod.instance.Helper.Translation.Get("QuestData.322"),

                requirement = 5,

                reward = 1000,

                before = new()
                {

                    [CharacterHandle.characters.Effigy] = new()
                    {
                        prompt = true,
                        intro = Mod.instance.Helper.Translation.Get("QuestData.334") +
                        Mod.instance.Helper.Translation.Get("QuestData.335") +
                        Mod.instance.Helper.Translation.Get("QuestData.336"),

                    }
                },

            };

            quests.Add(wealdFour.name, wealdFour);

            // -----------------------------------------------------

            Quest wealdFive = new()
            {

                name = QuestHandle.wealdFive,

                icon = IconData.displays.weald,

                type = Quest.questTypes.lesson,

                give = Quest.questGivers.dialogue,

                title = Mod.instance.Helper.Translation.Get("QuestData.358"),

                description = Mod.instance.Helper.Translation.Get("QuestData.360"),

                instruction = Mod.instance.Helper.Translation.Get("QuestData.362"),

                progression = Mod.instance.Helper.Translation.Get("QuestData.364"),

                requirement = 100,

                reward = 1000,

                before = new()
                {

                    [CharacterHandle.characters.Effigy] = new()
                    {
                        prompt = true,
                        intro = Mod.instance.Helper.Translation.Get("QuestData.376") +
                        Mod.instance.Helper.Translation.Get("QuestData.377") +
                        Mod.instance.Helper.Translation.Get("QuestData.378") +
                        Mod.instance.Helper.Translation.Get("QuestData.379"),

                    }
                },

            };

            quests.Add(wealdFive.name, wealdFive);

            // =====================================================
            // WEALD CHALLENGE

            Quest challengeWeald = new()
            {

                name = QuestHandle.challengeWeald,

                type = Quest.questTypes.challenge,

                icon = IconData.displays.weald,

                // -----------------------------------------------

                give = Quest.questGivers.dialogue,

                trigger = true,

                triggerLocation = triggerLocales.UndergroundMine20.ToString(),

                triggerTime = 0,

                triggerRite = Rite.rites.weald,

                origin = new Vector2(25f, 13f) * 64,

                // -----------------------------------------------

                title = Mod.instance.Helper.Translation.Get("QuestData.416"),

                description = Mod.instance.Helper.Translation.Get("QuestData.418") +
                Mod.instance.Helper.Translation.Get("QuestData.419"),

                instruction = Mod.instance.Helper.Translation.Get("QuestData.421"),

                explanation = Mod.instance.Helper.Translation.Get("QuestData.423") +
                Mod.instance.Helper.Translation.Get("QuestData.424") +
                Mod.instance.Helper.Translation.Get("QuestData.425"),

                progression = null,

                requirement = 0,

                reward = 2000,

                replay = Mod.instance.Helper.Translation.Get("QuestData.433"),

                // -----------------------------------------------

                before = new()
                {

                    [CharacterHandle.characters.Effigy] = new()
                    {
                        prompt = true,
                        intro = Mod.instance.Helper.Translation.Get("QuestData.443") +
                        Mod.instance.Helper.Translation.Get("QuestData.444") +
                        Mod.instance.Helper.Translation.Get("QuestData.445"),

                    }
                },

                after = new()
                {
                    [CharacterHandle.characters.Effigy] = new()
                    {
                        prompt = true,
                        intro = Mod.instance.Helper.Translation.Get("QuestData.455"),
                        responses = new()
                        {
                            Mod.instance.Helper.Translation.Get("QuestData.458"),
                            Mod.instance.Helper.Translation.Get("QuestData.459")
                        },
                        answers = new(){
                            Mod.instance.Helper.Translation.Get("QuestData.462"),
                        }
                    }
                },

            };

            quests.Add(challengeWeald.name, challengeWeald);

            // =====================================================
            // RELICS QUESTS

            Quest relicWeald = new()
            {

                name = QuestHandle.relicWeald,

                type = Quest.questTypes.relics,

                icon = IconData.displays.weald,

                // -----------------------------------------------

                give = Quest.questGivers.none,

                trigger = true,

                triggerLocation = triggerLocales.CommunityCenter.ToString(),

                triggerTime = 0,

                triggerRite = Rite.rites.weald,

                origin = new Vector2(14, 22) * 64,

                // -----------------------------------------------

                title = Mod.instance.Helper.Translation.Get("QuestData.499"),

                description = Mod.instance.Helper.Translation.Get("QuestData.501") +
                Mod.instance.Helper.Translation.Get("QuestData.502") +
                Mod.instance.Helper.Translation.Get("QuestData.503"),

                instruction = Mod.instance.Helper.Translation.Get("QuestData.505"),

                explanation = Mod.instance.Helper.Translation.Get("QuestData.507") +
                Mod.instance.Helper.Translation.Get("QuestData.508"),

            };

            quests.Add(relicWeald.name, relicWeald);

            // =====================================================
            // SWORD MISTS

            Quest swordMists = new()
            {

                name = QuestHandle.swordMists,

                type = Quest.questTypes.sword,

                icon = IconData.displays.mists,

                // -----------------------------------------------

                give = Quest.questGivers.dialogue,

                trigger = true,

                triggerLocation = Location.LocationData.druid_atoll_name,

                triggerTime = 0,

                triggerRite = Rite.rites.none,

                origin = new Vector2(30, 21) * 64,

                // -----------------------------------------------

                title = Mod.instance.Helper.Translation.Get("QuestData.542"),

                description = Mod.instance.Helper.Translation.Get("QuestData.544") +
                Mod.instance.Helper.Translation.Get("QuestData.545"),

                instruction = Mod.instance.Helper.Translation.Get("QuestData.547") +
                Mod.instance.Helper.Translation.Get("QuestData.548"),

                explanation = Mod.instance.Helper.Translation.Get("QuestData.550") +
                Mod.instance.Helper.Translation.Get("QuestData.551"),

                progression = null,

                requirement = 0,

                reward = 250,

                // -----------------------------------------------

                before = new()
                {

                    [CharacterHandle.characters.Effigy] = new()
                    {
                        prompt = true,
                        intro = Mod.instance.Helper.Translation.Get("QuestData.567") +
                        Mod.instance.Helper.Translation.Get("QuestData.568") +
                        Mod.instance.Helper.Translation.Get("QuestData.569") +
                        Mod.instance.Helper.Translation.Get("QuestData.570") +
                        Mod.instance.Helper.Translation.Get("QuestData.571"),

                    }
                },

                after = new()
                {
                    [CharacterHandle.characters.Effigy] = new()
                    {
                        prompt = true,
                        intro = Mod.instance.Helper.Translation.Get("QuestData.581"),
                        responses = new()
                        {
                            Mod.instance.Helper.Translation.Get("QuestData.584"),
                            Mod.instance.Helper.Translation.Get("QuestData.585"),
                        },
                        answers = new()
                        {
                            Mod.instance.Helper.Translation.Get("QuestData.589"),
                        },
                    }
                },


            };

            quests.Add(swordMists.name, swordMists);

            // =====================================================
            // MISTS LESSONS

            Quest mistsOne = new()
            {

                name = QuestHandle.mistsOne,

                icon = IconData.displays.mists,

                type = Quest.questTypes.lesson,

                give = Quest.questGivers.none,

                title = Mod.instance.Helper.Translation.Get("QuestData.613"),

                description = Mod.instance.Helper.Translation.Get("QuestData.615"),

                instruction = Mod.instance.Helper.Translation.Get("QuestData.617"),

                progression = Mod.instance.Helper.Translation.Get("QuestData.619"),

                requirement = 10,

                reward = 1000,

                before = new()
                {

                    [CharacterHandle.characters.Effigy] = new()
                    {
                        prompt = true,
                        intro = Mod.instance.Helper.Translation.Get("QuestData.631") +
                        Mod.instance.Helper.Translation.Get("QuestData.632") +
                        Mod.instance.Helper.Translation.Get("QuestData.633"),

                    }
                },

            };

            quests.Add(mistsOne.name, mistsOne);

            // -----------------------------------------------------

            Quest mistsTwo = new()
            {

                name = QuestHandle.mistsTwo,

                icon = IconData.displays.mists,

                type = Quest.questTypes.lesson,

                give = Quest.questGivers.dialogue,

                title = Mod.instance.Helper.Translation.Get("QuestData.655"),

                description = Mod.instance.Helper.Translation.Get("QuestData.657"),

                instruction = Mod.instance.Helper.Translation.Get("QuestData.659") +
                Mod.instance.Helper.Translation.Get("QuestData.660"),

                progression = Mod.instance.Helper.Translation.Get("QuestData.662"),

                requirement = 10,

                reward = 1000,

                before = new()
                {

                    [CharacterHandle.characters.Effigy] = new()
                    {
                        prompt = true,
                        intro = Mod.instance.Helper.Translation.Get("QuestData.674") +
                        Mod.instance.Helper.Translation.Get("QuestData.675") +
                        Mod.instance.Helper.Translation.Get("QuestData.676"),

                    }
                },

            };

            quests.Add(mistsTwo.name, mistsTwo);

            // -----------------------------------------------------

            Quest mistsThree = new()
            {

                name = QuestHandle.mistsThree,

                icon = IconData.displays.mists,

                type = Quest.questTypes.lesson,

                give = Quest.questGivers.dialogue,

                title = Mod.instance.Helper.Translation.Get("QuestData.698"),

                description = Mod.instance.Helper.Translation.Get("QuestData.700"),

                instruction = Mod.instance.Helper.Translation.Get("QuestData.702") +
                Mod.instance.Helper.Translation.Get("QuestData.703") +
                Mod.instance.Helper.Translation.Get("QuestData.704"),

                progression = Mod.instance.Helper.Translation.Get("QuestData.706"),

                requirement = 10,

                reward = 1000,

                before = new()
                {

                    [CharacterHandle.characters.Effigy] = new()
                    {
                        prompt = true,
                        intro = Mod.instance.Helper.Translation.Get("QuestData.718") +
                        Mod.instance.Helper.Translation.Get("QuestData.719"),

                    }
                },

            };

            quests.Add(mistsThree.name, mistsThree);

            // -----------------------------------------------------

            Quest relicMists = new()
            {

                name = QuestHandle.relicMists,

                type = Quest.questTypes.relics,

                icon = IconData.displays.mists,

                // -----------------------------------------------

                give = Quest.questGivers.none,

                trigger = true,

                triggerLocation = triggerLocales.CommunityCenter.ToString(),

                triggerTime = 0,

                triggerRite = Rite.rites.mists,

                origin = new Vector2(40, 9) * 64,

                // -----------------------------------------------

                title = Mod.instance.Helper.Translation.Get("QuestData.755"),

                description = Mod.instance.Helper.Translation.Get("QuestData.757") +
                Mod.instance.Helper.Translation.Get("QuestData.758"),

                instruction = Mod.instance.Helper.Translation.Get("QuestData.760"),

                explanation = Mod.instance.Helper.Translation.Get("QuestData.762"),

            };

            quests.Add(relicMists.name, relicMists);

            // -----------------------------------------------------

            Quest mistsFour = new()
            {

                name = QuestHandle.mistsFour,

                icon = IconData.displays.mists,

                type = Quest.questTypes.lesson,

                give = Quest.questGivers.dialogue,

                title = Mod.instance.Helper.Translation.Get("QuestData.781"),

                description = Mod.instance.Helper.Translation.Get("QuestData.783"),

                instruction = Mod.instance.Helper.Translation.Get("QuestData.785"),

                progression = Mod.instance.Helper.Translation.Get("QuestData.787"),

                requirement = 20,

                reward = 1000,

                before = new()
                {

                    [CharacterHandle.characters.Effigy] = new()
                    {
                        prompt = true,
                        intro = Mod.instance.Helper.Translation.Get("QuestData.799") +
                        Mod.instance.Helper.Translation.Get("QuestData.800"),

                    }
                },

            };

            quests.Add(mistsFour.name, mistsFour);


            // =====================================================
            // QUEST EFFIGY

            Quest questEffigy = new()
            {

                name = QuestHandle.questEffigy,

                type = Quest.questTypes.heart,

                icon = IconData.displays.effigy,

                // -----------------------------------------------

                give = Quest.questGivers.dialogue,

                trigger = true,

                triggerLocation = triggerLocales.Beach.ToString(),

                triggerTime = 0,

                triggerRite = Rite.rites.none,

                origin = new Vector2(12f, 13f) * 64,

                // -----------------------------------------------

                title = Mod.instance.Helper.Translation.Get("QuestData.838"),

                description = Mod.instance.Helper.Translation.Get("QuestData.840"),

                instruction = Mod.instance.Helper.Translation.Get("QuestData.842") +
                    Mod.instance.Helper.Translation.Get("QuestData.843"),

                explanation = Mod.instance.Helper.Translation.Get("QuestData.845") +
                        Mod.instance.Helper.Translation.Get("QuestData.846") +
                        Mod.instance.Helper.Translation.Get("QuestData.847") +
                        Mod.instance.Helper.Translation.Get("QuestData.848"),

                progression = null,

                requirement = 0,

                reward = 1000,

                // -----------------------------------------------

                before = new()
                {

                    [CharacterHandle.characters.Effigy] = new()
                    {
                        prompt = true,
                        intro = Mod.instance.Helper.Translation.Get("QuestData.864") +
                        Mod.instance.Helper.Translation.Get("QuestData.865") +
                        Mod.instance.Helper.Translation.Get("QuestData.866"),

                    }
                },

                after = new()
                {
                    [CharacterHandle.characters.Effigy] = new()
                    {
                        prompt = true,
                        intro = Mod.instance.Helper.Translation.Get("QuestData.876"),
                    }
                },

            };

            quests.Add(questEffigy.name, questEffigy);


            // =====================================================
            // MISTS CHALLENGE

            Quest challengeMists = new()
            {

                name = QuestHandle.challengeMists,

                type = Quest.questTypes.challenge,

                icon = IconData.displays.mists,

                // -----------------------------------------------

                //give = Quest.questGivers.none,
                give = Quest.questGivers.dialogue,

                trigger = true,

                triggerLocation = triggerLocales.Town.ToString(),

                //triggerTime = 0,
                triggerTime = 1900,

                triggerRite = Rite.rites.mists,

                origin = new Vector2(47f, 88f) * 64,

                // -----------------------------------------------

                title = Mod.instance.Helper.Translation.Get("QuestData.915"),

                description = Mod.instance.Helper.Translation.Get("QuestData.917") +
                Mod.instance.Helper.Translation.Get("QuestData.918"),

                instruction = Mod.instance.Helper.Translation.Get("QuestData.920"),

                explanation = Mod.instance.Helper.Translation.Get("QuestData.922") +
                Mod.instance.Helper.Translation.Get("QuestData.923") +
                Mod.instance.Helper.Translation.Get("QuestData.924") +
                Mod.instance.Helper.Translation.Get("QuestData.925") +
                Mod.instance.Helper.Translation.Get("QuestData.926"),

                progression = null,

                requirement = 0,

                reward = 3000,

                replay = Mod.instance.Helper.Translation.Get("QuestData.934"),

                // -----------------------------------------------

                before = new()
                {

                    [CharacterHandle.characters.Effigy] = new()
                    {
                        prompt = true,
                        intro = 
                        Mod.instance.Helper.Translation.Get("QuestData.952") +
                        Mod.instance.Helper.Translation.Get("QuestData.953") +
                        Mod.instance.Helper.Translation.Get("QuestData.954") +
                        Mod.instance.Helper.Translation.Get("QuestData.955") +
                        Mod.instance.Helper.Translation.Get("QuestData.956") +
                        Mod.instance.Helper.Translation.Get("QuestData.957") +
                        Mod.instance.Helper.Translation.Get("QuestData.958"),

                    }
                },

                after = new()
                {
                    [CharacterHandle.characters.Effigy] = new()
                    {
                        prompt = true,
                        intro = 
                        Mod.instance.Helper.Translation.Get("QuestData.971") +
                        Mod.instance.Helper.Translation.Get("QuestData.972") +
                        Mod.instance.Helper.Translation.Get("QuestData.973"),
                    }
                },

            };

            quests.Add(challengeMists.name, challengeMists);


            // =====================================================
            // SWORD STARS

            Quest swordStars = new()
            {

                name = QuestHandle.swordStars,

                type = Quest.questTypes.sword,

                icon = IconData.displays.revenant,

                // -----------------------------------------------

                give = Quest.questGivers.dialogue,

                trigger = true,

                triggerLocation = Location.LocationData.druid_chapel_name,

                triggerTime = 0,

                triggerRite = Rite.rites.stars,

                origin = new Vector2(27, 16) * 64,

                // -----------------------------------------------

                title = Mod.instance.Helper.Translation.Get("QuestData.1010"),

                description = Mod.instance.Helper.Translation.Get("QuestData.1012"),

                instruction = Mod.instance.Helper.Translation.Get("QuestData.1014"),

                explanation = Mod.instance.Helper.Translation.Get("QuestData.1016") +
                        Mod.instance.Helper.Translation.Get("QuestData.1017"),

                progression = null,

                requirement = 0,

                reward = 250,

                // -----------------------------------------------

                before = new()
                {

                    [CharacterHandle.characters.Effigy] = new()
                    {
                        prompt = true,
                        intro = Mod.instance.Helper.Translation.Get("QuestData.1033") +
                        Mod.instance.Helper.Translation.Get("QuestData.1034") +
                        Mod.instance.Helper.Translation.Get("QuestData.1035") +
                        Mod.instance.Helper.Translation.Get("QuestData.1036") +
                        Mod.instance.Helper.Translation.Get("QuestData.1037") +
                        Mod.instance.Helper.Translation.Get("QuestData.1038"),

                    }
                },

                after = new()
                {
                    [CharacterHandle.characters.Effigy] = new()
                    {
                        prompt = true,
                        intro = Mod.instance.Helper.Translation.Get("QuestData.1048"),
                        responses = new()
                        {
                            Mod.instance.Helper.Translation.Get("QuestData.1051"),
                            Mod.instance.Helper.Translation.Get("QuestData.1052"),
                        },
                        answers = new()
                        {
                            Mod.instance.Helper.Translation.Get("QuestData.1056"),
                        }

                    }
                },

            };

            quests.Add(swordStars.name, swordStars);

            // =====================================================
            // STARS LESSONS

            Quest starsOne = new()
            {

                name = QuestHandle.starsOne,

                icon = IconData.displays.stars,

                type = Quest.questTypes.lesson,

                give = Quest.questGivers.dialogue,

                title = Mod.instance.Helper.Translation.Get("QuestData.1080"),

                description = Mod.instance.Helper.Translation.Get("QuestData.1082"),

                instruction = Mod.instance.Helper.Translation.Get("QuestData.1084"),

                progression = Mod.instance.Helper.Translation.Get("QuestData.1086"),

                requirement = 20,

                reward = 1000,

                before = new()
                {

                    [CharacterHandle.characters.Revenant] = new()
                    {
                        prompt = true,
                        intro = Mod.instance.Helper.Translation.Get("QuestData.1098") +
                        Mod.instance.Helper.Translation.Get("QuestData.1099") +
                        Mod.instance.Helper.Translation.Get("QuestData.1100") +
                        Mod.instance.Helper.Translation.Get("QuestData.1101"),

                    }
                },

            };

            quests.Add(starsOne.name, starsOne);

            // -----------------------------------------------------

            Quest starsTwo = new()
            {

                name = QuestHandle.starsTwo,

                icon = IconData.displays.stars,

                type = Quest.questTypes.lesson,

                give = Quest.questGivers.dialogue,

                title = Mod.instance.Helper.Translation.Get("QuestData.1123"),

                description = Mod.instance.Helper.Translation.Get("QuestData.1125"),

                instruction = Mod.instance.Helper.Translation.Get("QuestData.1127") +
                Mod.instance.Helper.Translation.Get("QuestData.1128"),

                progression = Mod.instance.Helper.Translation.Get("QuestData.1130"),

                requirement = 10,

                reward = 1000,

                before = new()
                {

                    [CharacterHandle.characters.Revenant] = new()
                    {
                        prompt = true,
                        intro = Mod.instance.Helper.Translation.Get("QuestData.1142") +
                        Mod.instance.Helper.Translation.Get("QuestData.1143") +
                        Mod.instance.Helper.Translation.Get("QuestData.1144"),

                    },

                    [CharacterHandle.characters.Effigy] = new()
                    {
                        prompt = true,
                        questContext = 1,
                        intro = Mod.instance.Helper.Translation.Get("QuestData.1152") +
                        Mod.instance.Helper.Translation.Get("QuestData.1153") +
                        Mod.instance.Helper.Translation.Get("QuestData.1154"),

                    },

                },

            };

            quests.Add(starsTwo.name, starsTwo);


            // =====================================================
            // STARS CHALLENGE

            Quest challengeStars = new()
            {

                name = QuestHandle.challengeStars,

                type = Quest.questTypes.challenge,

                icon = IconData.displays.stars,

                // -----------------------------------------------

                give = Quest.questGivers.dialogue,

                trigger = true,

                triggerLocation = triggerLocales.Forest.ToString(),

                triggerTime = 0,

                triggerRite = Rite.rites.stars,

                origin = new Vector2(79f, 78f) * 64,

                // -----------------------------------------------

                title = Mod.instance.Helper.Translation.Get("QuestData.1193"),

                description = Mod.instance.Helper.Translation.Get("QuestData.1195") +
                    Mod.instance.Helper.Translation.Get("QuestData.1196") +
                    Mod.instance.Helper.Translation.Get("QuestData.1197"),

                instruction = Mod.instance.Helper.Translation.Get("QuestData.1199"),

                explanation = Mod.instance.Helper.Translation.Get("QuestData.1201") +
                    Mod.instance.Helper.Translation.Get("QuestData.1202") +
                    Mod.instance.Helper.Translation.Get("QuestData.1203"),

                progression = null,

                requirement = 0,

                reward = 5000,

                replay = Mod.instance.Helper.Translation.Get("QuestData.1211"),

                // -----------------------------------------------

                before = new()
                {

                    [CharacterHandle.characters.Effigy] = new()
                    {
                        prompt = true,
                        intro = Mod.instance.Helper.Translation.Get("QuestData.1221") +
                            Mod.instance.Helper.Translation.Get("QuestData.1222") +
                            Mod.instance.Helper.Translation.Get("QuestData.1223"),

                    }
                },

                after = new()
                {

                    [CharacterHandle.characters.Effigy] = new()
                    {
                        prompt = true,
                        intro = Mod.instance.Helper.Translation.Get("QuestData.1234") +
                        Mod.instance.Helper.Translation.Get("QuestData.1235") +
                        Mod.instance.Helper.Translation.Get("QuestData.1236"),
                    },

                    [CharacterHandle.characters.Revenant] = new()
                    {
                        prompt = true,
                        intro = Mod.instance.Helper.Translation.Get("QuestData.1242"),
                    },

                },

            };

            quests.Add(challengeStars.name, challengeStars);

            // =====================================================
            // ATOLL CHALLENGE

            Quest challengeAtoll = new()
            {

                name = QuestHandle.challengeAtoll,

                type = Quest.questTypes.challenge,

                icon = IconData.displays.mists,

                // -----------------------------------------------

                give = Quest.questGivers.dialogue,

                trigger = true,

                triggerLocation = LocationData.druid_atoll_name,

                triggerTime = 0,

                triggerRite = Rite.rites.mists,

                origin = new Vector2(28, 10f) * 64,

                // -----------------------------------------------

                title = Mod.instance.Helper.Translation.Get("QuestData.1279"),

                description = Mod.instance.Helper.Translation.Get("QuestData.1281") +
                Mod.instance.Helper.Translation.Get("QuestData.1282"),

                instruction = Mod.instance.Helper.Translation.Get("QuestData.1284"),

                explanation = Mod.instance.Helper.Translation.Get("QuestData.1286"),

                progression = null,

                requirement = 0,

                reward = 5000,

                replay = Mod.instance.Helper.Translation.Get("QuestData.1294"),

                // -----------------------------------------------

                before = new()
                {

                    [CharacterHandle.characters.Effigy] = new()
                    {
                        prompt = true,
                        intro = Mod.instance.Helper.Translation.Get("QuestData.1304") +
                        Mod.instance.Helper.Translation.Get("QuestData.1305") +
                        Mod.instance.Helper.Translation.Get("QuestData.1306") +
                        Mod.instance.Helper.Translation.Get("QuestData.1307"),

                    }
                },

                after = new()
                {
                    [CharacterHandle.characters.Effigy] = new()
                    {
                        prompt = true,
                        intro = Mod.instance.Helper.Translation.Get("QuestData.1317") +
                        Mod.instance.Helper.Translation.Get("QuestData.1318"),
                    },
                },

            };

            quests.Add(challengeAtoll.name, challengeAtoll);

            // =====================================================
            // DRAGON CHALLENGE

            Quest challengeDragon = new()
            {

                name = QuestHandle.challengeDragon,

                type = Quest.questTypes.challenge,

                icon = IconData.displays.stars,

                // -----------------------------------------------

                give = Quest.questGivers.dialogue,

                trigger = true,

                triggerLocation = LocationData.druid_vault_name,

                triggerTime = 0,

                origin = new Vector2(27, 15) * 64,

                // -----------------------------------------------

                title = Mod.instance.Helper.Translation.Get("QuestData.1352"),

                description = Mod.instance.Helper.Translation.Get("QuestData.1354"),

                instruction = Mod.instance.Helper.Translation.Get("QuestData.1356"),

                explanation = Mod.instance.Helper.Translation.Get("QuestData.1358"),

                progression = null,

                requirement = 0,

                reward = 5000,

                // -----------------------------------------------

                before = new()
                {

                    [CharacterHandle.characters.Revenant] = new()
                    {
                        prompt = true,
                        intro = Mod.instance.Helper.Translation.Get("QuestData.1374") +
                        Mod.instance.Helper.Translation.Get("QuestData.1375") +
                        Mod.instance.Helper.Translation.Get("QuestData.1376") +
                        Mod.instance.Helper.Translation.Get("QuestData.1377") +
                        Mod.instance.Helper.Translation.Get("QuestData.1378"),

                    },

                    [CharacterHandle.characters.Effigy] = new()
                    {
                        prompt = true,
                        questContext = 1,
                        intro = Mod.instance.Helper.Translation.Get("QuestData.1386") +
                            Mod.instance.Helper.Translation.Get("QuestData.1387") +
                            Mod.instance.Helper.Translation.Get("QuestData.1388"),


                    },

                },

                replay = Mod.instance.Helper.Translation.Get("QuestData.1395"),

                after = new()
                {
                    [CharacterHandle.characters.Revenant] = new()
                    {
                        prompt = true,
                        intro = Mod.instance.Helper.Translation.Get("QuestData.1402") +
                        Mod.instance.Helper.Translation.Get("QuestData.1403") +
                        Mod.instance.Helper.Translation.Get("QuestData.1404"),
                    },
                },

            };

            quests.Add(challengeDragon.name, challengeDragon);


            // =====================================================
            // APPROACH JESTER

            Quest approachJester = new()
            {

                name = QuestHandle.approachJester,

                type = Quest.questTypes.approach,

                icon = IconData.displays.jester,

                // -----------------------------------------------

                give = Quest.questGivers.dialogue,

                trigger = false,

                triggerLocation = triggerLocales.Mountain.ToString(),

                triggerTime = 0,

                origin = new Vector2(90, 27) * 64,

                // -----------------------------------------------

                title = Mod.instance.Helper.Translation.Get("QuestData.1439"),

                description = Mod.instance.Helper.Translation.Get("QuestData.1441") +
                Mod.instance.Helper.Translation.Get("QuestData.1442") +
                Mod.instance.Helper.Translation.Get("QuestData.1443") +
                Mod.instance.Helper.Translation.Get("QuestData.1444"),

                instruction = Mod.instance.Helper.Translation.Get("QuestData.1446"),

                explanation = Mod.instance.Helper.Translation.Get("QuestData.1448") +
                Mod.instance.Helper.Translation.Get("QuestData.1449") +
                Mod.instance.Helper.Translation.Get("QuestData.1450") +
                Mod.instance.Helper.Translation.Get("QuestData.1451"),

                progression = null,

                requirement = 0,

                reward = 1000,

                // -----------------------------------------------

                before = new()
                {

                    [CharacterHandle.characters.Revenant] = new()
                    {
                        prompt = true,
                        intro = Mod.instance.Helper.Translation.Get("QuestData.1467") +
                        Mod.instance.Helper.Translation.Get("QuestData.1468") +
                        Mod.instance.Helper.Translation.Get("QuestData.1469") +
                        Mod.instance.Helper.Translation.Get("QuestData.1470") +
                        Mod.instance.Helper.Translation.Get("QuestData.1471") +
                        Mod.instance.Helper.Translation.Get("QuestData.1472"),

                    },
                    [CharacterHandle.characters.Effigy] = new()
                    {
                        prompt = true,
                        questContext = 1,
                        intro = Mod.instance.Helper.Translation.Get("QuestData.1479"),
                    },

                },

                after = new()
                {
                    [CharacterHandle.characters.Revenant] = new()
                    {
                        prompt = true,
                        intro = Mod.instance.Helper.Translation.Get("QuestData.1489") +
                        Mod.instance.Helper.Translation.Get("QuestData.1490"),
                    },
                    [CharacterHandle.characters.Effigy] = new()
                    {
                        prompt = true,
                        intro = Mod.instance.Helper.Translation.Get("QuestData.1495"),
                    },
                },

            };

            quests.Add(approachJester.name, approachJester);

            // =====================================================
            // SWORD (SCYTHE) FATES

            Quest swordFates = new()
            {

                name = QuestHandle.swordFates,

                type = Quest.questTypes.sword,

                icon = IconData.displays.fates,

                // -----------------------------------------------

                give = Quest.questGivers.dialogue,

                trigger = true,

                triggerLocation = triggerLocales.UndergroundMine77377.ToString(),

                triggerTime = 0,

                triggerRite = Rite.rites.none,

                origin = new Vector2(27, 99) * 64,

                // -----------------------------------------------

                title = Mod.instance.Helper.Translation.Get("QuestData.1531"),

                description = Mod.instance.Helper.Translation.Get("QuestData.1533") +
                    Mod.instance.Helper.Translation.Get("QuestData.1534"),

                instruction = Mod.instance.Helper.Translation.Get("QuestData.1536"),

                explanation = Mod.instance.Helper.Translation.Get("QuestData.1538") +
                    Mod.instance.Helper.Translation.Get("QuestData.1539") +
                    Mod.instance.Helper.Translation.Get("QuestData.1540"),

                progression = null,

                requirement = 0,

                reward = 2000,

                // -----------------------------------------------

                before = new()
                {

                    [CharacterHandle.characters.Jester] = new()
                    {
                        prompt = true,
                        intro = Mod.instance.Helper.Translation.Get("QuestData.1556") +
                        Mod.instance.Helper.Translation.Get("QuestData.1557"),

                    }
                },

                after = new()
                {
                    [CharacterHandle.characters.Jester] = new()
                    {
                        prompt = true,
                        intro = Mod.instance.Helper.Translation.Get("QuestData.1567")

                    }
                },

            };

            quests.Add(swordFates.name, swordFates);


            // =====================================================
            // FATES LESSONS

            Quest fatesOne = new()
            {

                name = QuestHandle.fatesOne,

                icon = IconData.displays.fates,

                type = Quest.questTypes.lesson,

                give = Quest.questGivers.dialogue,

                title = Mod.instance.Helper.Translation.Get("QuestData.1591"),

                description = Mod.instance.Helper.Translation.Get("QuestData.1593"),

                instruction = Mod.instance.Helper.Translation.Get("QuestData.1595"),

                progression = Mod.instance.Helper.Translation.Get("QuestData.1597"),

                requirement = 20,

                reward = 1000,

                before = new()
                {

                    [CharacterHandle.characters.Jester] = new()
                    {
                        prompt = true,
                        intro = Mod.instance.Helper.Translation.Get("QuestData.1609") +
                        Mod.instance.Helper.Translation.Get("QuestData.1610") +
                        Mod.instance.Helper.Translation.Get("QuestData.1611"),

                    }
                },

            };

            quests.Add(fatesOne.name, fatesOne);

            Quest fatesTwo = new()
            {

                name = QuestHandle.fatesTwo,

                icon = IconData.displays.fates,

                type = Quest.questTypes.lesson,

                give = Quest.questGivers.dialogue,

                title = Mod.instance.Helper.Translation.Get("QuestData.1631"),

                description = Mod.instance.Helper.Translation.Get("QuestData.1633"),

                instruction = Mod.instance.Helper.Translation.Get("QuestData.1635") +
                Mod.instance.Helper.Translation.Get("QuestData.1636") +
                Mod.instance.Helper.Translation.Get("QuestData.1637"),

                progression = Mod.instance.Helper.Translation.Get("QuestData.1639"),

                requirement = 20,

                reward = 1000,

                before = new()
                {

                    [CharacterHandle.characters.Jester] = new()
                    {
                        prompt = true,
                        intro = Mod.instance.Helper.Translation.Get("QuestData.1651") +
                        Mod.instance.Helper.Translation.Get("QuestData.1652") +
                        Mod.instance.Helper.Translation.Get("QuestData.1653"),

                    }
                },

            };

            quests.Add(fatesTwo.name, fatesTwo);

            Quest fatesThree = new()
            {

                name = QuestHandle.fatesThree,

                icon = IconData.displays.fates,

                type = Quest.questTypes.lesson,

                give = Quest.questGivers.dialogue,

                title = Mod.instance.Helper.Translation.Get("QuestData.1673"),

                description = Mod.instance.Helper.Translation.Get("QuestData.1675"),

                instruction = Mod.instance.Helper.Translation.Get("QuestData.1677"),

                progression = Mod.instance.Helper.Translation.Get("QuestData.1679"),

                requirement = 5,

                reward = 1000,

                before = new()
                {

                    [CharacterHandle.characters.Jester] = new()
                    {
                        prompt = true,
                        intro = Mod.instance.Helper.Translation.Get("QuestData.1691") +
                        Mod.instance.Helper.Translation.Get("QuestData.1692"),

                    }
                },

            };

            quests.Add(fatesThree.name, fatesThree);

            // =====================================================
            // QUEST JESTER

            Quest questJester = new()
            {

                name = QuestHandle.questJester,

                type = Quest.questTypes.heart,

                icon = IconData.displays.jester,

                // -----------------------------------------------

                give = Quest.questGivers.dialogue,

                trigger = true,

                triggerLocation = triggerLocales.Town.ToString(),

                triggerTime = 1200,

                triggerRite = Rite.rites.none,

                origin = new Vector2(29f, 56f) * 64,

                // -----------------------------------------------

                title = Mod.instance.Helper.Translation.Get("QuestData.1729"),

                description = Mod.instance.Helper.Translation.Get("QuestData.1731"),

                instruction = Mod.instance.Helper.Translation.Get("QuestData.1733") +
                Mod.instance.Helper.Translation.Get("QuestData.1734"),

                explanation =
                        Mod.instance.Helper.Translation.Get("QuestData.1737") +
                        Mod.instance.Helper.Translation.Get("QuestData.1738") +
                        Mod.instance.Helper.Translation.Get("QuestData.1739") +
                        Mod.instance.Helper.Translation.Get("QuestData.1740") +
                        Mod.instance.Helper.Translation.Get("QuestData.1741"),

                reward = 5000,

                // -----------------------------------------------

                before = new()
                {

                    [CharacterHandle.characters.Jester] = new()
                    {
                        prompt = true,
                        intro = Mod.instance.Helper.Translation.Get("QuestData.1753"),

                    }
                },

                after = new()
                {
                    [CharacterHandle.characters.Jester] = new()
                    {
                        prompt = true,
                        intro = Mod.instance.Helper.Translation.Get("QuestData.1763"),
                    },
                    [CharacterHandle.characters.Buffin] = new()
                    {
                        prompt = true,
                        intro = Mod.instance.Helper.Translation.Get("QuestData.1768"),
                    },

                },

            };

            quests.Add(questJester.name, questJester);

            // =====================================================
            // FATES FINAL LESSONS

            Quest fatesFour = new()
            {

                name = QuestHandle.fatesFour,

                icon = IconData.displays.buffin,

                type = Quest.questTypes.lesson,

                give = Quest.questGivers.dialogue,

                title = Mod.instance.Helper.Translation.Get("QuestData.1791"),

                description = Mod.instance.Helper.Translation.Get("QuestData.1793"),

                instruction = Mod.instance.Helper.Translation.Get("QuestData.1795") +
                Mod.instance.Helper.Translation.Get("QuestData.1796"),

                progression = Mod.instance.Helper.Translation.Get("QuestData.1798"),

                requirement = 10,

                reward = 1000,

                before = new()
                {

                    [CharacterHandle.characters.Jester] = new()
                    {
                        prompt = true,
                        intro = Mod.instance.Helper.Translation.Get("QuestData.1810") +
                        Mod.instance.Helper.Translation.Get("QuestData.1811"),
                        responses = new()
                        {
                            Mod.instance.Helper.Translation.Get("QuestData.1814"),
                            Mod.instance.Helper.Translation.Get("QuestData.1815")
                        },
                        answers = new()
                        {
                            Mod.instance.Helper.Translation.Get("QuestData.1819") +
                            Mod.instance.Helper.Translation.Get("QuestData.1820"),
                        },
                        questContext = 1
                    },
                    [CharacterHandle.characters.Buffin] = new()
                    {
                        prompt = true,
                        intro = Mod.instance.Helper.Translation.Get("QuestData.1827") +
                        Mod.instance.Helper.Translation.Get("QuestData.1828") +
                        Mod.instance.Helper.Translation.Get("QuestData.1829"),
                    },

                },

            };

            quests.Add(fatesFour.name, fatesFour);

            // =====================================================
            // FATES CHALLENGE

            Quest challengeFates = new()
            {

                name = QuestHandle.challengeFates,

                icon = IconData.displays.fates,

                type = Quest.questTypes.challenge,

                give = Quest.questGivers.dialogue,

                trigger = true,

                triggerLocation = LocationData.druid_court_name,

                triggerTime = 1700,

                triggerRite = Rite.rites.none,

                origin = new Vector2(30, 20) * 64,

                title = Mod.instance.Helper.Translation.Get("QuestData.1862"),

                description = Mod.instance.Helper.Translation.Get("QuestData.1864") +
                Mod.instance.Helper.Translation.Get("QuestData.1865"),

                instruction = Mod.instance.Helper.Translation.Get("QuestData.1867"),

                explanation =
                        Mod.instance.Helper.Translation.Get("QuestData.1870") +
                        Mod.instance.Helper.Translation.Get("QuestData.1871") +
                        Mod.instance.Helper.Translation.Get("QuestData.1872") +
                        Mod.instance.Helper.Translation.Get("QuestData.1873") +
                        Mod.instance.Helper.Translation.Get("QuestData.1874") +
                        Mod.instance.Helper.Translation.Get("QuestData.1875"),

                reward = 10000,

                replay = Mod.instance.Helper.Translation.Get("QuestData.1879"),

                before = new()
                {

                    [CharacterHandle.characters.Jester] = new()
                    {
                        prompt = true,
                        questContext = 1,
                        intro = Mod.instance.Helper.Translation.Get("QuestData.1888") +
                        Mod.instance.Helper.Translation.Get("QuestData.1889") +
                        Mod.instance.Helper.Translation.Get("QuestData.1890")
                    },
                    [CharacterHandle.characters.Buffin] = new()
                    {
                        prompt = true,
                        intro = Mod.instance.Helper.Translation.Get("QuestData.1895") +
                        Mod.instance.Helper.Translation.Get("QuestData.1896") +
                        Mod.instance.Helper.Translation.Get("QuestData.1897") +
                        Mod.instance.Helper.Translation.Get("QuestData.1898")
                    },

                },

                after = new()
                {

                    [CharacterHandle.characters.Buffin] = new()
                    {
                        prompt = true,
                        intro = Mod.instance.Helper.Translation.Get("QuestData.1909") +
                        Mod.instance.Helper.Translation.Get("QuestData.1910") +
                        Mod.instance.Helper.Translation.Get("QuestData.1911"),
                    },

                },

            };

            quests.Add(challengeFates.name, challengeFates);


            // =====================================================
            // SWORD ETHER (CUTLASS)

            Quest swordEther = new()
            {

                name = QuestHandle.swordEther,

                type = Quest.questTypes.sword,

                icon = IconData.displays.ether,

                // -----------------------------------------------

                give = Quest.questGivers.dialogue,

                trigger = true,

                triggerLocation = LocationData.druid_tomb_name,

                triggerTime = 0,

                triggerRite = Rite.rites.none,

                origin = new Vector2(27, 15) * 64,

                // -----------------------------------------------

                title = Mod.instance.Helper.Translation.Get("QuestData.1949"),

                description = Mod.instance.Helper.Translation.Get("QuestData.1951") +
                Mod.instance.Helper.Translation.Get("QuestData.1952") +
                Mod.instance.Helper.Translation.Get("QuestData.1953"),

                instruction = Mod.instance.Helper.Translation.Get("QuestData.1955"),

                explanation = Mod.instance.Helper.Translation.Get("QuestData.1957") +
                Mod.instance.Helper.Translation.Get("QuestData.1958"),

                progression = null,

                requirement = 0,

                reward = 10000,

                replay = Mod.instance.Helper.Translation.Get("QuestData.1966"),

                // -----------------------------------------------

                before = new()
                {

                    [CharacterHandle.characters.Effigy] = new()
                    {
                        prompt = true,
                        questContext = 1,
                        intro = Mod.instance.Helper.Translation.Get("QuestData.1977") +
                        Mod.instance.Helper.Translation.Get("QuestData.1978") +
                        Mod.instance.Helper.Translation.Get("QuestData.1979") +
                        Mod.instance.Helper.Translation.Get("QuestData.1980") +
                        Mod.instance.Helper.Translation.Get("QuestData.1981"),


                    },
                    [CharacterHandle.characters.Revenant] = new()
                    {
                        prompt = true,
                        intro = Mod.instance.Helper.Translation.Get("QuestData.1988"),
                        responses = new()
                        {
                            Mod.instance.Helper.Translation.Get("QuestData.1991"),
                            Mod.instance.Helper.Translation.Get("QuestData.1992"),
                        },
                        answers = new(){
                            Mod.instance.Helper.Translation.Get("QuestData.1995") +
                            Mod.instance.Helper.Translation.Get("QuestData.1996") +
                            Mod.instance.Helper.Translation.Get("QuestData.1997") +
                            Mod.instance.Helper.Translation.Get("QuestData.1998") +
                            Mod.instance.Helper.Translation.Get("QuestData.1999") +
                            Mod.instance.Helper.Translation.Get("QuestData.2000")+
                            Mod.instance.Helper.Translation.Get("QuestData.2001"),
                        },

                    },

                },

                after = new()
                {
                    [CharacterHandle.characters.Revenant] = new()
                    {
                        prompt = true,
                        intro = Mod.instance.Helper.Translation.Get("QuestData.2013")
                    },
                    [CharacterHandle.characters.Jester] = new()
                    {
                        prompt = true,
                        intro = Mod.instance.Helper.Translation.Get("QuestData.2018")
                    },
                },

            };

            quests.Add(swordEther.name, swordEther);

            // =====================================================
            // ETHER LESSONS


            Quest relicFates = new()
            {

                name = QuestHandle.relicFates,

                type = Quest.questTypes.relics,

                icon = IconData.displays.fates,

                // -----------------------------------------------

                give = Quest.questGivers.none,

                trigger = true,

                triggerLocation = triggerLocales.CommunityCenter.ToString(),

                triggerTime = 0,

                triggerRite = Rite.rites.fates,

                origin = new Vector2(45, 12) * 64,

                // -----------------------------------------------

                title = Mod.instance.Helper.Translation.Get("QuestData.2055"),

                description = Mod.instance.Helper.Translation.Get("QuestData.2057"),

                instruction = Mod.instance.Helper.Translation.Get("QuestData.2059"),

                explanation = Mod.instance.Helper.Translation.Get("QuestData.2061"),

            };

            quests.Add(relicFates.name, relicFates);


            Quest etherOne = new()
            {

                name = QuestHandle.etherOne,

                icon = IconData.displays.ether,

                type = Quest.questTypes.lesson,

                give = Quest.questGivers.dialogue,

                title = Mod.instance.Helper.Translation.Get("QuestData.2079"),

                description = Mod.instance.Helper.Translation.Get("QuestData.2081"),

                instruction = Mod.instance.Helper.Translation.Get("QuestData.2083"),

                progression = Mod.instance.Helper.Translation.Get("QuestData.2085"),

                requirement = 10,

                reward = 1000,

                before = new()
                {

                    [CharacterHandle.characters.Shadowtin] = new()
                    {
                        prompt = true,
                        intro = Mod.instance.Helper.Translation.Get("QuestData.2097") +
                        Mod.instance.Helper.Translation.Get("QuestData.2098") +
                        Mod.instance.Helper.Translation.Get("QuestData.2099") +
                        Mod.instance.Helper.Translation.Get("QuestData.2100"),

                    },

                },

            };

            quests.Add(etherOne.name, etherOne);

            Quest etherTwo = new()
            {

                name = QuestHandle.etherTwo,

                icon = IconData.displays.ether,

                type = Quest.questTypes.lesson,

                give = Quest.questGivers.dialogue,

                title = Mod.instance.Helper.Translation.Get("QuestData.2121"),

                description = Mod.instance.Helper.Translation.Get("QuestData.2123"),

                instruction = Mod.instance.Helper.Translation.Get("QuestData.2125"),

                progression = Mod.instance.Helper.Translation.Get("QuestData.2127"),

                requirement = 20,

                reward = 1000,

                before = new()
                {

                    [CharacterHandle.characters.Shadowtin] = new()
                    {
                        prompt = true,
                        intro = Mod.instance.Helper.Translation.Get("QuestData.2139") +
                        Mod.instance.Helper.Translation.Get("QuestData.2140") +
                        Mod.instance.Helper.Translation.Get("QuestData.2141"),

                    }
                },

            };

            quests.Add(etherTwo.name, etherTwo);

            Quest etherThree = new()
            {

                name = QuestHandle.etherThree,

                icon = IconData.displays.ether,

                type = Quest.questTypes.lesson,

                give = Quest.questGivers.dialogue,

                title = Mod.instance.Helper.Translation.Get("QuestData.2161"),

                description = Mod.instance.Helper.Translation.Get("QuestData.2163"),

                instruction = Mod.instance.Helper.Translation.Get("QuestData.2165"),

                progression = Mod.instance.Helper.Translation.Get("QuestData.2167"),

                requirement = 10,

                reward = 1000,

                before = new()
                {

                    [CharacterHandle.characters.Shadowtin] = new()
                    {
                        prompt = true,
                        intro = Mod.instance.Helper.Translation.Get("QuestData.2179") +
                        Mod.instance.Helper.Translation.Get("QuestData.2180") +
                        Mod.instance.Helper.Translation.Get("QuestData.2181"),

                    }
                },

            };

            quests.Add(etherThree.name, etherThree);


            // =====================================================
            // QUEST SHADOWTIN

            Quest questShadowtin = new()
            {

                name = QuestHandle.questShadowtin,

                type = Quest.questTypes.heart,

                icon = IconData.displays.shadowtin,

                // -----------------------------------------------

                give = Quest.questGivers.dialogue,

                trigger = true,

                triggerLocation = triggerLocales.Forest.ToString(),

                triggerTime = 0,

                triggerRite = Rite.rites.none,

                origin = new Vector2(60f, 20f) * 64,

                // -----------------------------------------------

                title = Mod.instance.Helper.Translation.Get("QuestData.2219"),

                description = Mod.instance.Helper.Translation.Get("QuestData.2221") +
                Mod.instance.Helper.Translation.Get("QuestData.2222") +
                Mod.instance.Helper.Translation.Get("QuestData.2223"),

                instruction = Mod.instance.Helper.Translation.Get("QuestData.2225"),

                explanation = Mod.instance.Helper.Translation.Get("QuestData.2227") +
                Mod.instance.Helper.Translation.Get("QuestData.2228") +
                Mod.instance.Helper.Translation.Get("QuestData.2229") +
                Mod.instance.Helper.Translation.Get("QuestData.2230") +
                Mod.instance.Helper.Translation.Get("QuestData.2231") +
                Mod.instance.Helper.Translation.Get("QuestData.2232") +
                Mod.instance.Helper.Translation.Get("QuestData.2233") +
                Mod.instance.Helper.Translation.Get("QuestData.2234"),

                reward = 5000,

                // -----------------------------------------------

                before = new()
                {

                    [CharacterHandle.characters.Shadowtin] = new()
                    {
                        prompt = true,
                        intro = Mod.instance.Helper.Translation.Get("QuestData.2246") +
                        Mod.instance.Helper.Translation.Get("QuestData.2247") +
                        Mod.instance.Helper.Translation.Get("QuestData.2248") +
                        Mod.instance.Helper.Translation.Get("QuestData.2249") +
                        Mod.instance.Helper.Translation.Get("QuestData.2250") +
                        Mod.instance.Helper.Translation.Get("QuestData.2251") +
                        Mod.instance.Helper.Translation.Get("QuestData.2252"),

                    },
                    [CharacterHandle.characters.Jester] = new()
                    {
                        prompt = true,
                        questContext = 1,
                        intro = Mod.instance.Helper.Translation.Get("QuestData.2259") +
                        Mod.instance.Helper.Translation.Get("QuestData.2260"),

                    },

                },

                after = new()
                {


                },

            };

            quests.Add(questShadowtin.name, questShadowtin);

            Quest etherFour = new()
            {

                name = QuestHandle.etherFour,

                icon = IconData.displays.ether,

                type = Quest.questTypes.lesson,

                give = Quest.questGivers.dialogue,

                title = Mod.instance.Helper.Translation.Get("QuestData.2287"),

                description = Mod.instance.Helper.Translation.Get("QuestData.2289"),

                instruction = Mod.instance.Helper.Translation.Get("QuestData.2291") +
                Mod.instance.Helper.Translation.Get("QuestData.2292"),

                progression = Mod.instance.Helper.Translation.Get("QuestData.2294"),

                requirement = 5,

                reward = 1000,

                before = new()
                {

                    [CharacterHandle.characters.Jester] = new()
                    {
                        prompt = true,
                        questContext = 1,
                        intro = Mod.instance.Helper.Translation.Get("QuestData.2307") +
                        Mod.instance.Helper.Translation.Get("QuestData.2308") +
                        Mod.instance.Helper.Translation.Get("QuestData.2309") +
                        Mod.instance.Helper.Translation.Get("QuestData.2310"),

                    },

                    [CharacterHandle.characters.Shadowtin] = new()
                    {
                        prompt = true,
                        intro =

                        Mod.instance.Helper.Translation.Get("QuestData.2319") +
                        Mod.instance.Helper.Translation.Get("QuestData.2320") +
                        Mod.instance.Helper.Translation.Get("QuestData.2321") +
                        Mod.instance.Helper.Translation.Get("QuestData.2322") +
                        Mod.instance.Helper.Translation.Get("QuestData.2323"),

                    },


                },

            };

            quests.Add(etherFour.name, etherFour);

            Quest relicEther = new()
            {

                name = QuestHandle.relicEther,

                type = Quest.questTypes.relics,

                icon = IconData.displays.ether,

                // -----------------------------------------------

                give = Quest.questGivers.none,

                trigger = true,

                triggerLocation = triggerLocales.CommunityCenter.ToString(),

                triggerTime = 0,

                triggerRite = Rite.rites.ether,

                origin = new Vector2(16, 7) * 64,

                // -----------------------------------------------

                title = Mod.instance.Helper.Translation.Get("QuestData.2359"),

                description = Mod.instance.Helper.Translation.Get("QuestData.2361") +
                Mod.instance.Helper.Translation.Get("QuestData.2362"),

                instruction = Mod.instance.Helper.Translation.Get("QuestData.2364"),

                explanation = Mod.instance.Helper.Translation.Get("QuestData.2366") +
                Mod.instance.Helper.Translation.Get("QuestData.2367"),

            };

            quests.Add(relicEther.name, relicEther);

            // =====================================================
            // Challenge Ether

            Quest challengeEther = new()
            {

                name = QuestHandle.challengeEther,

                icon = IconData.displays.ether,

                type = Quest.questTypes.challenge,

                give = Quest.questGivers.dialogue,

                trigger = true,

                triggerLocation = LocationData.druid_gate_name,

                triggerRite = Rite.rites.none,

                origin = new Vector2(28, 9) * 64,

                title = Mod.instance.Helper.Translation.Get("QuestData.2395"),

                description = Mod.instance.Helper.Translation.Get("QuestData.2397") +
                Mod.instance.Helper.Translation.Get("QuestData.2398") +
                Mod.instance.Helper.Translation.Get("QuestData.2399"),

                instruction = Mod.instance.Helper.Translation.Get("QuestData.2401"),

                explanation = Mod.instance.Helper.Translation.Get("QuestData.2404"),


                reward = 10000,

                replay = Mod.instance.Helper.Translation.Get("QuestData.2409"),

                before = new()
                {

                    [CharacterHandle.characters.Shadowtin] = new()
                    {
                        prompt = true,
                        questContext = 1,
                        intro = Mod.instance.Helper.Translation.Get("QuestData.2418") +
                        Mod.instance.Helper.Translation.Get("QuestData.2419") +
                        Mod.instance.Helper.Translation.Get("QuestData.2420") +
                        Mod.instance.Helper.Translation.Get("QuestData.2421") +
                        Mod.instance.Helper.Translation.Get("QuestData.2422"),

                    },
                    [CharacterHandle.characters.Effigy] = new()
                    {
                        prompt = true,
                        intro = Mod.instance.Helper.Translation.Get("QuestData.2428"),
                        responses = new()
                        {

                            Mod.instance.Helper.Translation.Get("QuestData.2432"),
                            Mod.instance.Helper.Translation.Get("QuestData.2433"),

                        },
                        answers = new()
                        {
                            Mod.instance.Helper.Translation.Get("QuestData.2438") +
                            Mod.instance.Helper.Translation.Get("QuestData.2439") +
                            Mod.instance.Helper.Translation.Get("QuestData.2440") +
                            Mod.instance.Helper.Translation.Get("QuestData.2441") +
                            Mod.instance.Helper.Translation.Get("QuestData.2442") +
                            Mod.instance.Helper.Translation.Get("QuestData.2443")

                        }

                    },
                },


            };

            quests.Add(challengeEther.name, challengeEther);

            return quests;

        }


    }

    public class Quest
    {

        public enum questTypes
        {
            none,
            approach,
            sword,
            challenge,
            lesson,
            heart,
            relics,
            miscellaneous
        }

        public enum questGivers
        {
            none,
            dialogue,
            chain,
        }

        public string name;

        public questTypes type = questTypes.none;

        public questGivers give = questGivers.none;

        public Vector2 origin = Vector2.Zero;

        // -----------------------------------------------
        // trigger

        public bool trigger;

        public string triggerLocation;

        public int triggerTime;

        public Rite.rites triggerRite = Rite.rites.none;

        // -----------------------------------------------
        // journal

        public string title;

        public IconData.displays icon = IconData.displays.none;

        public string description;

        public string instruction;

        public string explanation;

        public string progression;

        public int requirement;

        public int reward;

        public string replay;

        // -----------------------------------------------
        // dialogues

        public Dictionary<CharacterHandle.characters, Dialogue.DialogueSpecial> before = new();

        public Dictionary<CharacterHandle.characters, Dialogue.DialogueSpecial> after = new();

    }

    public class QuestProgress
    {

        public int status;

        public int progress;

        public int replay;

        public int delay;

        public QuestProgress()
        {

        }

        public QuestProgress(int Status, int Delay = 0, int Progress = 0, int Replay = 0)
        {

            status = Status;

            progress = Progress;

            replay = Replay;

            delay = Delay;

        }

    }
}
