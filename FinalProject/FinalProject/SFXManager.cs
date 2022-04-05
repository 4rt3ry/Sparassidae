/*
 * Nick Kannenberg
 * Sound Effects Manager class
 * Static class that hanfles all functionalty of sound effects
 * Useful so that each object does not need to contain variables of every sound effect it might need
 */


using Microsoft.Xna.Framework.Audio;
using System;
using System.Collections.Generic;
using System.Text;

namespace FinalProject
{
    public enum Sounds
    {
        Catch,
        SClick1,
        SClick2,
        SClick3,
        SClick4,
        SAmbience,
        HBNormal,
        HBRushed,
        HBFrantic,
        BrNormal,
        BrMedium,
        BrHeavy,
        BrSigh
    }
    public static class SFXManager
    {
        //Feilds
        //Master list of all sounds
        /// <summary>
        /// 0 - Catch
        /// 1-4 - Spider Clicking Sounds
        /// 5 - Spider Ambience
        /// 6-8 - Heartbeats (Normal, Rushed, Frantic)
        /// 9-11 - Breathing (Normal, Medium, Heavy)
        /// 12 - Sigh
        /// </summary>
        static List<SoundEffect> sounds = null;
        static List<SoundEffectInstance> instances = null;

        //Methods

        //SFX Calls
        public static void PlaySound(Sounds s)
        {
            int index = EnumToIndex(s);
            sounds[index].Play();
        }

        /// <summary>
        /// Creates an instance of a sound and plays it
        /// </summary>
        /// <param name="index">Index of sound to play</param>
        /// <param name="ovride">True if overriding previously played sound</param>
        public static void LoopInstancedSound(Sounds s, bool ovride)
        {
            int index = EnumToIndex(s);
            //If the selected index is null
            if(instances[index] != null)
            {
                //If overriding, stop the current sound and set spot to null
                if (ovride)
                {
                    instances[index].Stop();
                    instances[index] = null;
                }
                else
                {
                    //If not overriding, allow current sound to keep playing
                    return;
                }
            }
            //Selected index is null, simply create instance at index
            instances[index] = sounds[index].CreateInstance();
            instances[index].IsLooped = true;
            instances[index].Play();
        }

        /// <summary>
        /// Stops the playing of an instanced sound
        /// </summary>
        /// <param name="index"></param>
        public static void StopInstancedSound(Sounds s)
        {
            int index = EnumToIndex(s);
            if (instances[index] != null)
            {
                instances[index].Stop();
                instances[index].Dispose();
                instances[index] = null;
            }
        }

        /// <summary>
        /// Stops all currently playing and looping instances of sound
        /// </summary>
        public static void StopAllInstances()
        {
            for(int i = 0; i < instances.Count; i++)
            {
                if(instances[i] != null)
                {
                    instances[i].Stop();
                    instances[i].Dispose();
                    instances[i] = null;
                }
            }
        }

        /// <summary>
        /// Stops all instances of HeartBeat sound effects
        /// </summary>
        public static void StopAllHB()
        {
            StopInstancedSound(Sounds.HBNormal);
            StopInstancedSound(Sounds.HBRushed);
            StopInstancedSound(Sounds.HBFrantic);
        }

        /// <summary>
        /// Stops all instances of breathing sound effects
        /// </summary>
        public static void StopAllBr()
        {
            StopInstancedSound(Sounds.BrNormal);
            StopInstancedSound(Sounds.BrMedium);
            StopInstancedSound(Sounds.BrHeavy);
            StopInstancedSound(Sounds.BrSigh);
        }



        //Sets global volume of all sound effects (Must be a float 0.0f-1.0f);
        public static void SetGlobalVolume(float master)
        {
            SoundEffect.MasterVolume = master;
        }
        
        //Run at game creation, gives this class the sound effects
        public static void GiveSFX(List<SoundEffect> snds)
        {
            sounds = snds;
            instances = new List<SoundEffectInstance>(sounds.Count);
            for(int i = 0; i < sounds.Count; i++)
            {
                instances.Add(null);
            }
        }

        /// <summary>
        /// Class helper method that converts a given enum type into an index within the sound list
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        private static int EnumToIndex(Sounds s)
        {
            switch (s)
            {
                case Sounds.Catch:
                    return 0;
                case Sounds.SClick1:
                    return 1;
                case Sounds.SClick2:
                    return 2;
                case Sounds.SClick3:
                    return 3;
                case Sounds.SClick4:
                    return 4;
                case Sounds.SAmbience:
                    return 5;
                case Sounds.HBNormal:
                    return 6;
                case Sounds.HBRushed:
                    return 7;
                case Sounds.HBFrantic:
                    return 8;
                case Sounds.BrNormal:
                    return 9;
                case Sounds.BrMedium:
                    return 10;
                case Sounds.BrHeavy:
                    return 11;
                case Sounds.BrSigh:
                    return 12;
            }
            return 1;
        }
    }
}
