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
        static Dictionary<Sounds, SoundEffectInstance> instances = null;
        static Dictionary<Sounds, SoundEffect> sounds = null;

        //Methods

        //SFX Calls
        public static void PlaySound(Sounds s)
        {
            sounds[s].Play();
        }

        /// <summary>
        /// Creates an instance of a sound and plays it
        /// </summary>
        /// <param name="index">Index of sound to play</param>
        /// <param name="ovride">True if overriding previously played sound</param>
        public static void LoopInstancedSound(Sounds s, bool ovride)
        {
            //If the selected index is null
            if(instances[s] != null)
            {
                //If overriding, stop the current sound and set spot to null
                if (ovride)
                {
                    instances[s].Stop();
                    instances[s] = null;
                }
                else
                {
                    //If not overriding, allow current sound to keep playing
                    return;
                }
            }
            //Selected index is null, simply create instance at index
            instances[s] = sounds[s].CreateInstance();
            instances[s].IsLooped = true;
            instances[s].Play();
        }

        /// <summary>
        /// Stops the playing of an instanced sound
        /// </summary>
        /// <param name="index"></param>
        public static void StopInstancedSound(Sounds s)
        {
            if (instances[s] != null)
            {             
                instances[s].Stop();
                instances[s].Dispose();
                instances[s] = null;
            }
        }

        /// <summary>
        /// Stops all currently playing and looping instances of sound
        /// </summary>
        public static void StopAllInstances()
        {
            
            for(int i = 0; i < instances.Count; i++)
            {
                Sounds s = IndexToEnum(i);
                if (instances[s] != null)
                {
                    instances[s].Stop();
                    instances[s].Dispose();
                    instances[s] = null;
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
            sounds = new Dictionary<Sounds, SoundEffect>();

            //Add sounds to dictionary
            sounds[Sounds.Catch] = snds[0];
            sounds[Sounds.SClick1] = snds[1];
            sounds[Sounds.SClick2] = snds[2];
            sounds[Sounds.SClick3] = snds[3];
            sounds[Sounds.SClick4] = snds[4];
            sounds[Sounds.SAmbience] = snds[5];
            sounds[Sounds.HBNormal] = snds[6];
            sounds[Sounds.HBRushed] = snds[7];
            sounds[Sounds.HBFrantic] = snds[8];
            sounds[Sounds.BrNormal] = snds[9];
            sounds[Sounds.BrMedium] = snds[10];
            sounds[Sounds.BrHeavy] = snds[11];
            sounds[Sounds.BrSigh] = snds[12];

            instances = new Dictionary<Sounds, SoundEffectInstance>(sounds.Count);
            for(int i = 0; i < sounds.Count; i++)
            {
                Sounds s = IndexToEnum(i);
                instances[s] = null;
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

        private static Sounds IndexToEnum(int i)
        {
            switch (i) {

                case 0:
                    return Sounds.Catch;
                case 1:
                    return Sounds.SClick1;
                case 2:
                    return Sounds.SClick2;
                case 3:
                    return Sounds.SClick3;
                case 4:
                    return Sounds.SClick4;
                case 5:
                    return Sounds.SAmbience;
                case 6:
                    return Sounds.HBNormal;
                case 7:
                    return Sounds.HBRushed;
                case 8:
                    return Sounds.HBFrantic;
                case 9:
                    return Sounds.BrNormal;
                case 10:
                    return Sounds.BrMedium;
                case 11:
                    return Sounds.BrHeavy;
                case 12:
                    return Sounds.BrSigh;
            }
            return Sounds.SClick1;
        }
    }
}
