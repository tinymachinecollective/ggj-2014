using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Audio;
using GGJ2014.Game.Engine;

namespace GGJ2014.Game.Logic
{
    public static class Music
    {
        private static Cue intro;
        private static Cue quietLoop;
        private static Cue loudLoop;

        public static Cue Intro
        {
            get
            {
                if (intro == null)
                {
                    intro = AudioManager.Instance.LoadCue("music-Intro");
                }

                return intro;
            }
        }

        public static Cue QuietLoop
        {
            get
            {
                if (quietLoop == null)
                {
                    quietLoop = AudioManager.Instance.LoadCue("music-QuietLoop");
                }

                return quietLoop;
            }
        }

        public static Cue LoadLoop
        {
            get
            {
                if (loudLoop == null)
                {
                    loudLoop = AudioManager.Instance.LoadCue("music-LoudLoop");
                }

                return loudLoop;
            }
        }
    }
}
