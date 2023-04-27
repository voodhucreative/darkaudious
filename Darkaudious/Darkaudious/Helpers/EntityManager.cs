using System;
namespace Darkaudious.Helpers
{
    public class EntityManager
    {
        public int MagneticStimulusLevel { get; set; }
        public int LocationStimulusLevel { get; set; }
        public int PressureStimulusLevel { get; set; }
        public int MovementStimulusLevel { get; set; }
        public int RandomStimulusLevel { get; set; }

        public int StimulusLevel { get; set; }
        public int CurrentActivityLevel { get; set; }

        /*
        public int LowActivityTrigger = 15;
        public int MediumActivityTrigger = 20;
        public int HighActivityTrigger = 30;
        public int ChaoticActivityTrigger = 80;
        */
        public int LowActivityTrigger = 5;
        public int MediumActivityTrigger = 15;
        public int HighActivityTrigger = 30;
        public int ChaoticActivityTrigger = 50;
        public int Memory = 0;


        public enum ActivityLevels
        {
            NothingHappening,
            LowActivity,
            MediumActivity,
            HighActivity,
            ChaoticActivity
        }

        public enum StimulusTypes
        {
            Magnetic,
            Location,
            Pressure,
            Movement,
            Random
        }

        public EntityManager()
        {
            CurrentActivityLevel = (int)ActivityLevels.NothingHappening;
            ResetStimuli();
        }

        public void ResetStimuli()
        {

            MagneticStimulusLevel = 0;
            LocationStimulusLevel = 0;
            PressureStimulusLevel = 0;
            MovementStimulusLevel = 0;
            RandomStimulusLevel = 0;
            StimulusLevel = 0;
            
        }

        public void IncreaseStimulus()
        {
            IncreaseStimulus((int)StimulusTypes.Random, 1);
        }

        public void IncreaseStimulus(int type, int amount)
        {
            switch(type)
            {
                case (int)StimulusTypes.Magnetic:
                    MagneticStimulusLevel += amount;
                    break;
                case (int)StimulusTypes.Location:
                    LocationStimulusLevel += amount;
                    break;
                case (int)StimulusTypes.Pressure:
                    PressureStimulusLevel += amount;
                    break;
                case (int)StimulusTypes.Movement:
                    if (Numbers.GetNextRandom(0, 100) > 50)
                    {
                        MovementStimulusLevel += amount;
                    }
                    else
                    {
                        MovementStimulusLevel -= amount;
                    }
                    break;
                case (int)StimulusTypes.Random:
                    if (Numbers.GetNextRandom(0, 100) > 50)
                    {
                        MovementStimulusLevel += amount;
                    }
                    else
                    {
                        MovementStimulusLevel -= amount;
                    }
                    break;
            }
            StimulusLevel = MagneticStimulusLevel + LocationStimulusLevel + PressureStimulusLevel + MovementStimulusLevel + RandomStimulusLevel;
        }

        public int GetCurrentActvityLevel()
        {
            StimulusLevel = MagneticStimulusLevel + LocationStimulusLevel + PressureStimulusLevel + MovementStimulusLevel + RandomStimulusLevel;



            CurrentActivityLevel = (int)ActivityLevels.NothingHappening;
            Memory = 0;

            if (StimulusLevel > 0 && StimulusLevel <= LowActivityTrigger)
            {
                CurrentActivityLevel = (int)ActivityLevels.LowActivity;
                Memory = 1;
            }
            else if (StimulusLevel > LowActivityTrigger && StimulusLevel <= MediumActivityTrigger)
            {
                CurrentActivityLevel = (int)ActivityLevels.MediumActivity;
                Memory = Numbers.GetNextRandom(1, 3);
            }
            else if (StimulusLevel > MediumActivityTrigger && StimulusLevel <= HighActivityTrigger)
            {
                CurrentActivityLevel = (int)ActivityLevels.HighActivity;
                Memory = Numbers.GetNextRandom(2, 4);
            }
            else if (StimulusLevel > HighActivityTrigger)
            {
                CurrentActivityLevel = (int)ActivityLevels.ChaoticActivity;
                Memory = 100; // fully engaged and can repeat any stimulus
            }

            return CurrentActivityLevel;
        }

        public void UpdateLevels()
        {
            if (StimulusLevel > MediumActivityTrigger) // limit increasing into high / chaotic levels
            {
                if (Numbers.GetNextRandom(0, 100) > 50)
                {
                    StimulusLevel = MediumActivityTrigger;
                }
            }

            if (StimulusLevel > HighActivityTrigger)
            {
                if (Numbers.GetNextRandom(0, 100) > 50)
                {
                    StimulusLevel = MediumActivityTrigger;
                }

                if (Numbers.GetNextRandom(0, 100) > 90)
                {
                    StimulusLevel = LowActivityTrigger;
                }
            }
        }


        public string GetCurrentActvityLevelName()
        {
            string currentActivityLevelName = "NONE";
            UpdateLevels();

            switch (CurrentActivityLevel)
            {
                case (int)ActivityLevels.LowActivity:
                    currentActivityLevelName = "LOW";
                    break;
                case (int)ActivityLevels.MediumActivity:
                    currentActivityLevelName = "MEDIUM";
                    break;
                case (int)ActivityLevels.HighActivity:
                    currentActivityLevelName = "HIGH";
                    break;
                case (int)ActivityLevels.ChaoticActivity:
                    currentActivityLevelName = "CHAOTIC";
                    break;
            }

            return currentActivityLevelName;
        }
    }
}

