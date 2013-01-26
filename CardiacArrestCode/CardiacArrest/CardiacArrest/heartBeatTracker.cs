using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework;

namespace CardiacArrest
{
    class HeartBeatTracker
    {
        SoundEffect heartBeat;
        SoundEffectInstance heartBeatInst;

        double volume;
        double pan;

       

        List<double> caseDistance = new List<double>();

        List<CaseFile> casesList = new List<CaseFile>();

        List<Rectangle> casesListTest = new List<Rectangle>();

        public HeartBeatTracker(SoundEffect soundEffect)
        {
            heartBeat = soundEffect;
            heartBeatInst = heartBeat.CreateInstance();
        }

        public void Update(Player player, Dictionary<string,CaseFile> levelCases)
        {
            casesListTest.Clear();
            caseDistance.Clear();

            foreach(KeyValuePair<string,CaseFile> i in levelCases)
            {
                double x;
                double y;
                double distance;

                x = Math.Abs(i.Value.rectangle.X - player.playerRectangle.X);
                y = Math.Abs(i.Value.rectangle.Y - player.playerRectangle.Y);
                distance = Math.Sqrt(Math.Pow(x, 2) + Math.Pow(y, 2));
                caseDistance.Add(distance);
                casesList.Add(i.Value);
            }

            
            if (caseDistance[0] < caseDistance[1] && caseDistance[0] < caseDistance[2])
            {
                playRelativeSound(player, 0);
            }

            else if (caseDistance[1] < caseDistance[0] && caseDistance[1] < caseDistance[2])
            {
                playRelativeSound(player, 1);
            }

           else if (caseDistance[2] < caseDistance[1] && caseDistance[2] < caseDistance[0])
            {
                playRelativeSound(player, 2);
            }
        }

        private void playRelativeSound(Player player, int i)
        {
            double panNum = casesListTest[i].X - player.playerRectangle.X;
            pan = MathHelper.Clamp((float)(panNum / 100), -1, 1);
            volume = MathHelper.Clamp((float)(100 / caseDistance[i]), 0, 1);

            heartBeatInst.Pan = (float)pan;
            heartBeatInst.Volume = (float)volume;

            if (heartBeatInst.State != SoundState.Playing)
            {
                heartBeatInst.Play();
            }
        }

        #region Testing
        public void Update3(Player player, Rectangle _1,Rectangle _2, Rectangle _3)
        {
            casesListTest.Clear();
            caseDistance.Clear();
            casesListTest.Add(_1);
            casesListTest.Add(_2);
            casesListTest.Add(_3);

            foreach (Rectangle i in casesListTest)
            {
                double x;
                double y;
                double distance;

                x = Math.Abs(i.X - player.playerRectangle.X);
                y = Math.Abs(i.Y - player.playerRectangle.Y);
                distance = Math.Sqrt(Math.Pow(x, 2) + Math.Pow(y, 2));
                caseDistance.Add(distance);
                
            }

            if (caseDistance[0] < caseDistance[1] && caseDistance[0] < caseDistance[2])
            {
                playRelativeSound(player, 0);
            }

            else if (caseDistance[1] < caseDistance[0] && caseDistance[1] < caseDistance[2])
            {
                playRelativeSound(player, 1);
            }

            else if (caseDistance[2] < caseDistance[1] && caseDistance[2] < caseDistance[0])
            {
                playRelativeSound(player, 2);
            }
        }

         public void UpdateTest(Player player, Rectangle inRectangle)
        {
            double x;
            double y;
            double distance;

            x = Math.Abs(inRectangle.X - player.playerRectangle.X);
            y = Math.Abs(inRectangle.Y - player.playerRectangle.Y);
             distance = Math.Sqrt(Math.Pow(x,2) + Math.Pow(y,2));
             double panNum = inRectangle.X - player.playerRectangle.X;
             pan = MathHelper.Clamp((float)(panNum/100), -1, 1);
             volume = MathHelper.Clamp((float)(100 / distance), 0, 1);
             heartBeatInst.Pan = (float)pan;
             heartBeatInst.Volume = (float)volume;

             if (heartBeatInst.State != SoundState.Playing)
             {
                 heartBeatInst.Play();
             }

        }
        #endregion
    }
}
