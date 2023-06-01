using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Darkaudious.Helpers;
using Darkaudiouse.Services;
using Plugin.DeviceInfo;
using Plugin.SimpleAudioPlayer;
using Xamarin.Essentials;
using Xamarin.Forms;
using static Darkaudious.Helpers.AudioManager;
using static Xamarin.Essentials.Permissions;

namespace Darkaudious
{
    public partial class MainPage : ContentPage
    {
        Grid PageContent;

        StackLayout WordList;
        StackLayout WordListContent;
        StackLayout WordListContainer;

        StackLayout InfoContent;
        ScrollView InfoScroller;

        Label CurrentWord;
        Label SeekingWord;
        Label ThinkingIndicator;
        Label ScanSpeedLabel;
        Label SpiritStrengthLabel;
        Label SpiritMoodLabel;

        //Image BackgroundImage;
        //Image BackgroundImage2;

        Image HeaderBG;
        Image FooterBG;
        Image LeftStrip;
        Image RightStrip;


        Image TopLogoOff;
        Image TopLogoOn;

        Image Circuit;
        Image Blackout;
        Image Whiteout;
        Image Mask;

        int CurrentEvent;
        int NextEvent;
        int LastEvent;

        DateTime StartTime;
        DateTime NextEventTime;
        DateTime CurrentTime;

        StoppableTimer BrainTimer;
        StoppableTimer EffectAnimTimer;
        StoppableTimer ScanTimer;
        StoppableTimer LogoAnimTimer;

        StoppableTimer AudioTimer;
        StoppableTimer WordFadeTimer;
        StoppableTimer ActivityTimer;

        bool PerformingEvent;

        ISimpleAudioPlayer AudioPlayer;

        List<string> LoggedWordsList;
        List<string> DisplayWordsList;

        int WordMemory = 2;

        Image CircuitAnimImage;
        int CurrentEffectFrame = 1;
        bool EffectStarted = false;
        float EffectMaxOpacity = 0.5f;

        int EffectFade = 0;
        int EFFECT_FADE_IN = 0;
        int EFFECT_FADE_OUT = 1;

        Label LogLabel;
        Label ViewLabel;
        Label PowerLabel;
        Label SpeechLabel;
        Label StatusLabel;

        int effectAnimDelay = 50;
        int startDelay = 0;

        bool Initialised = false;

        bool AllowSpecialMessage = false;

        static int EVENT_SPEED_FAST = 500;
        static int EVENT_SPEED_MEDIUM_FAST = 750;
        static int EVENT_SPEED_MEDIUM = 1250;
        static int EVENT_SPEED_MEDIUM_SLOW = 2000;
        static int EVENT_SPEED_SLOW = 2500;
        static int EVENT_SPEED_VERY_SLOW = 3000;

        int EventDelay = EVENT_SPEED_MEDIUM;

        int RunningSpeed = 10; // between 10 and 90
        int DelayBetweenEvents = 2000;

        // MAIN DELAY!! INCREASE TO SLOW DOWN
        float ScanFrequency = 8.0f;
        float MaxFrequency = 10.0f;
        float ScanSensitivity = 2.0f;
        float scanCount = 0.0f;


        bool showThinking = false;
        int thinkIndicatorCount = 0;
        int scanSkipCount = 0;

        Image PlusImage;
        Image MinusImage;
        Image StartStopImage;
        Image HistoryImage;
        Image InfoImage;
        Image ClearImage;

        Image PowerImage;
        Image SoundButton;
        Image SpeechButton;
        Image VibrationButton;


        bool DeadboxOn;
        bool IsPaused;

        // movement sensor
        bool LikesMovement = false;
        SensorSpeed MovementSensorSpeed = SensorSpeed.Default;
        bool movingDevice = false;
        int MovementSensorTriggerCount = 0;

        bool ScreenshotMode = false;
        bool UseMockLocation = false;


        int NoteDecay = 0;
        bool PlayNote = true;
        int NoteLength = 1;
        int PauseBetweenNotes = 0;
        int[] NotesToPlay;

        string[] WordsToSpeak;
        bool IsSpeaking = false;

        bool PlayMelody = true;
        int EventFrequency = 50;
        int MelodyFrequency = 500;
        int RandomiseFrequency = 500;
        int MelodyCount = 0;
        int LastPlayedMelody;
        int LastInstrumentPlayed;

        int Mood = 0;


        int UserTriggeredMelodyCount;
        int UserTriggeredMelodyLimit;


        int [,] TunesPlayed;


        public enum DarkaudiousEvents
        {
            DarkaudiousLoad,
            DarkaudiousStartUp,
            DarkaudiousStandBy,
            DarkaudiousSwitchOn,
            DarkaudiousBegin,
            DarkaudiousRestart,
            DarkaudiousSwitchOff,
            DarkaudiousEvent,
            DarkaudiousSpoken
        };

        public string[] DarkaudiousEventNames =
        {
            "DarkaudiousLoad",
            "DarkaudiousStartUp",
            "DarkaudiousStandBy",
            "DarkaudiousSwitchOn",
            "DarkaudiousBegin",
            "DarkaudiousRestart",
            "DarkaudiousSwitchOff",
            "DarkaudiousEvent"
        };

        public int EntityProximityInFeet; // 0 - 100
        public int EntityAgressionLevel; // 0 - 100
        public int EntityAge; // 5 - 90
        public int EntityType; // human, animal, alien
        public int EntityMood; // passive, annoyed, sad, frightened, angry, furious, voilent, sneaky, murderous
        public int EntityGender; // male, female, unknown

        PhraseManager PhraseManager;

        Label SpeechToggle;
        Label VibrationToggle;
        Label SoundToggle;
        Label InvestigationLabel;

        Label SaveHistory;
        Label CloseHistory;

        bool SpeechOn;
        bool VibrateOn;
        bool SoundOn;

        int SafeArea;

        int DEBUG_SESSION = 500;
        int QUICK_JOB = 1500;
        int NORMAL_INVESTIGATION = 2500;
        int SLOW_BURN = 4000;
        int LONG_NIGHT = 8000;

        int WORD_HISTORY = 0;
        int GENERAL_INFO = 1;

        int InfoType;

        bool ShowInfoBox;

        Label WordHistory;
        Label WordHistoryLabel;

        Label ToyPiano;
        Label GrandPiano;

        int CurrentMood = 0;


        AudioManager AudioManager;

        SensorSpeed MagnoSpeed = SensorSpeed.UI;
        SensorSpeed BaroSpeed = SensorSpeed.UI;

        float MagnoX = Numbers.GetNextRandom(3, 10);
        float MagnoY = Numbers.GetNextRandom(3, 10);
        float MagnoZ = Numbers.GetNextRandom(3, 10);
        float BaroPressure = Numbers.GetNextRandom(999, 1010);

        int MoveX = 0;
        int MoveY = 0;
        int MoveZ = 0;

        bool SpiritTriggeredMelody;

        public string[] Moods =
        {
            "FURIOUS",
            "ANGRY",
            "HIDING",
            "SCARED",
            "SHY",
            "PASSIVE",
            "CURIOUS",
            "WATCHING",
            "AMUSED",
            "HAPPY"
        };

        int CurrentStrength = 0;
        public string[] Strengths =
        {
            "FLAT",
            "POOR",
            "WEAK",
            "UNSTABLE",
            "OK",
            "STABLE",
            "GOOD",
            "STRONG",
            "GREAT",
            "POWERFUL"
        };




        string SpiritMood = "PASSIVE";
        string SpiritStrength = "MEDIUM";

        bool IsAnybodyThere = false;

        public Dictionary<string, string> InfoPoints = new Dictionary<string, string>();

        int[] MajorTriad;
        int[] MinorTriad;
        int[] DischordantTriad;
        int[] RandomTriad;
        int[] SignatureMelody;
        List<int> LongNotes;

        int notePosition;
        int selectedInstrument;


        // commands

        // respond

        // play

        // tune 1

        Label Tune1;
        Label Tune2;
        Label Tune3;
        Label Tune4;
        Label Tune5;
        Label Tune6;

        Label Phrase1;
        Label Phrase2;
        Label Phrase3;
        Label Phrase4;

        EntityManager EntityManager;

        Label PianoSelect;
        Label ToySelect;
        Label ToneSelect;

        float OffTextOpacity=0.25f;

        string YellowColour = "fbb515";
        string RedColour = "dd0000";

        int TunesAvailable = 1;
        int MoodsAvailable = 2;
        int SourcesAvailable = 1;
        int UserInteraction = 0;
        DateTime PowerUpTimeCheck;

        public MainPage()
        {
            CurrentEvent = (int)DarkaudiousEvents.DarkaudiousLoad;

            EntityManager = new EntityManager();

            PhraseManager = new PhraseManager();
            PhraseManager.PopulateDictionary();

            if (Device.RuntimePlatform == Device.Android)
            {
                UseMockLocation = true;
            }

            TunesPlayed = new int[,] { { 0, 0, 0, 0 }, { 0, 0, 0, 0 }, { 0, 0, 0, 0 }, { 0, 0, 0, 0 }, { 0, 0, 0, 0 }, { 0, 0, 0, 0 } };
   
            SpiritTriggeredMelody = false;
            UserTriggeredMelodyCount = 0;
            UserTriggeredMelodyLimit = 2;
            LastPlayedMelody = 0;
            LastInstrumentPlayed = (int)SoundSources.Piano;

            Mood = 0; // major
            

            selectedInstrument = (int)SoundSources.Piano;

            AudioManager = new AudioManager();

            MajorTriad = AudioManager.GetMajorTriad(Numbers.GetNextRandom(0, 11), Numbers.GetNextRandom(0, 2));
            MinorTriad = AudioManager.GetMinorTriad(Numbers.GetNextRandom(0, 11), Numbers.GetNextRandom(0, 2));
            DischordantTriad = AudioManager.GetDischordantTriad(Numbers.GetNextRandom(0, 11), Numbers.GetNextRandom(0, 2));
            RandomTriad = AudioManager.GetRandomTriad(Numbers.GetNextRandom(0, 11), Numbers.GetNextRandom(0, 2));



            SignatureMelody = AudioManager.GetSignatureMelody();
            WordsToSpeak = new string[] { "I", "AM", "DARK", "AUDIOUS"};

            LongNotes = new List<int>();

            for (int i = 0; i < 10; i++)
            {
                /*
                MinorTriad = AudioManager.GetMinorTriad(Numbers.GetNextRandom(0, 11), Numbers.GetNextRandom(0, 2));
                foreach(int n in MinorTriad)
                {
                    LongNotes.Add(n);
                }*/

                /*
                MajorTriad = AudioManager.GetMajorTriad(Numbers.GetNextRandom(0, 11), Numbers.GetNextRandom(0, 2));
                foreach (int n in MajorTriad)
                {
                    LongNotes.Add(n);
                }*/
                LongNotes.Add(AudioManager.GetNote(Numbers.GetNextRandom(0, 2), Numbers.GetNextRandom(0, 11)));
            }

            notePosition = 0;

            InfoPoints.Add("GENERAL", "Darkaudious is the second member of the Dark Arts paranormal investigation apps family. It uses intelligent algorithms to generate sounds to encourage communication with spirits, giving them a rich palette of sounds, melodies, tones, vibes and language with which to connect to us.\n\nDuring an investigation, you will likely experience flurries of activity, periods of peace and occasional pulses of activity. Activity may be spoken word, which may be garbled or crystal clear. Look out for subliminal messages. These may be important! Spirits may also choose to use our chosen sound sources and vibes, copying our melodies to try and tell us that they’re listening and they like (or dislike) the stimulus that we’re providing. Listen carefully to understand the mood, meaning and messages that entities are giving us.\n");

            InfoPoints.Add("Controls", "CONTROLS");
            InfoPoints.Add("power_on.png", "POWER ON/OFF - switches Darkaudious System on and off. Switching off will clear all data");
            //InfoPoints.Add("start2off.png", "START/PAUSE INVESTIGATION - starts and pauses the Darkaudious system");
            InfoPoints.Add("copy_on.png", "COPY/SAVE - copies the session to the device clipboard for sharing. You can paste copied sessions into any program or app you like!");
            //InfoPoints.Add("eraser.png", "CLEAR - clears the current session and deletes all messages");

            //InfoPoints.Add("sound.png", "SOUND ON/OFF - switches sound on and off");
            //InfoPoints.Add("mouth.png", "SPEECH ON/OFF - switches speech on and off");
            //InfoPoints.Add("vibrate.png", "VIBRATION ON/OFF - switches vibration on and off");

            InfoPoints.Add("info_on.png", "INFORMATION - opens this page");
            //InfoPoints.Add("more.png", "INCREASE SCAN RATE - turns up the frequency of the scanning system. This can only be done if the power of the spirit connection is strong enough");
            //InfoPoints.Add("less.png", "DECREASE SCAN RATE - turns down the frequency of the scanning system. This will reduce the possibilty of contact, but may increase the accuracy of the messages");

            //InfoPoints.Add("info", "USAGE TIPS\n\na settled spirit doesn't like too much movement so try and keep still to encourage them to respond\n\na restless spirit likes movement so try moving your device around to encourage them to respond\n\nsome spirits are very mischeivious and might try and throw you off guard with some very random phrases!\n\nif you're not having much luck, try moving to a different location... don't worry, Darkaudious will know you've moved!");


            InfoPoints.Add("SoundSource", "Select a sound source:\n\nPiano - a typical piano sound. Old properties often had, or still have, a piano. A familiar sound for entities and spirits from past generations.\n\nToy - a children’s toy piano sound, which is likely to appeal to younger spirits, children and mischievous entities.\n\nTone - a frequency based square wave tone, designed to disrupt the ambient frequencies of investigation space.\n");
            InfoPoints.Add("Vibe", "Select a vibe:\n\nMajor - happy and positive sounding melodies, designed to appeal to settled and happy entities, but also to provove, irritate and annoy the less content spirits into communicating with us.\n\nMinor - sad, negative and moody melodies created to appeal to the more tormented entities. They may also comfort spirits that are lonely, longing or nostalgic for times past.\n\nRandom - a combination of notes that create a discordant, chaotic vibe. These are designed to disrupt, stimulate and agitate any entities into communicating.\n\nSession - melodies that are generated using data specific to the current investigation session. Timestamps and device information to create unique data fingerprints, which are then processed by our intelligent melody generation algorithms. Each session will create a truly unique set of melodies.\n");
            InfoPoints.Add("Components", "App Components\n\nActivity Area - Darkaudious is created to be theoretically receptive to ITC (Instrumental Trans-Communication) activity. Entities can use algorithmic disruption to help influence the activity seen in the app. Darkaudious continuously generates random sequences of letters and words, which can be manipulated by an entity to give us meaningful messages. This activity can be viewed in the top portion of the app screen.\n\nLog - all meaningful spoken activity will be logged, so investigators can go back and examine the messages more closely after the investigation.\n\nActivity Status - this shows the current status of the investigation. It displays the current ambient stimulation level, which combines movement, pressure, magnetic field and unknown device stimulus, potentially triggered by an entity. These are used to calculate the likely spirit activity level.\n\nMovement - using the device accelerometer, we register any device movement. Any subtle changes can be used to affect and stimulate spirit activity. Any movement may also indicate poltergeist activity…\n\nPressure - if the device has a built in, compatible barometer, this will be used to register atmospheric pressure changes. Darkaudious will simulate pressure changes if no barometer is available and periodically stimulate the pressure receptor algorithms when the barometer is present to encourage greater activity.\n\nMagnetic Field - if the device has a built in, compatible magnetometer, this will be used to register magnetic field changes. Darkaudious will simulate magnetic field changes if no magnetometer is available and periodically stimulate the magnetic field receptor algorithms when the magnetometer is present to encourage greater activity.\n\nPresence - using a combination of readings from the device, coupled with any unknown ITC activity, Darkaudious can calculate if a spirit is present.\n\nPLEASE NOTE: Darkaudious is to be used for entertainment purposes only. ITC is a theoretical method, rather than a proven science. All paranormal activity is firmly placed in the realms of the unknown, which makes our work in this field so compelling and fascinating! We hope you enjoy your Darkaudious sessions!\n");
            //InfoPoints.Add("", "");
            //InfoPoints.Add("", "");
            //InfoPoints.Add("", "");
            //InfoPoints.Add("", "");

            //SetMode(DEBUG_SESSION);
            //SetMode(QUICK_JOB);
            SetMode(NORMAL_INVESTIGATION);
            //SetMode(SLOW_BURN);
            //SetMode(LONG_NIGHT);

            SetPersonality();

            startDelay = Numbers.GetNextRandom(10000, 30000);

            Initialised = false;
            DeadboxOn = false;
            ShowInfoBox = false;
            IsPaused = false;

            SpeechOn = true;
            VibrateOn = true;
            SoundOn = false;

            InfoType = WORD_HISTORY;

            // iOS
            SafeArea = 24;

            if (Device.RuntimePlatform == Device.iOS)
            {
                if (CrossDeviceInfo.Current.VersionNumber.Major <= 10)
                {
                    SafeArea = 24;
                }
                else
                {
                    SafeArea = 44;
                }
            }


            AudioPlayer = Plugin.SimpleAudioPlayer.CrossSimpleAudioPlayer.Current;

            MessagingCenter.Subscribe<object, string>(this, "WakeUp", (s, e) =>
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    WakeyWakey();
                });
            });

            CircuitAnimImage = new Image
            {
                Source = "gl" + CurrentEffectFrame + ".png",
                WidthRequest = Units.ScreenWidth,
                HeightRequest = Units.ScreenHeight,
                Aspect = Aspect.AspectFill,
                Opacity = 0.25

            };

            PageContent = new Grid
            {
                BackgroundColor = Color.FromHex("#000000"),
                WidthRequest = Units.ScreenWidth,
                HeightRequest = Units.ScreenHeight,
                HorizontalOptions = LayoutOptions.CenterAndExpand,
                VerticalOptions = LayoutOptions.CenterAndExpand
            };

            WordList = new StackLayout
            {
                HeightRequest = Units.ScreenHeight,
                WidthRequest = Units.ScreenWidth,
                HorizontalOptions = LayoutOptions.CenterAndExpand,
                VerticalOptions = LayoutOptions.CenterAndExpand,
                BackgroundColor = Color.Black,
            };

            WordListContent = new StackLayout
            {
                HorizontalOptions = LayoutOptions.CenterAndExpand,
                VerticalOptions = LayoutOptions.CenterAndExpand,
                Margin = 16,
                BackgroundColor = Color.Transparent,
            };


            HeaderBG = new Image
            {
                Source = "carbonbg_head3.png",
                //Source = "background2.png",
                WidthRequest = Units.ScreenWidth,
                HeightRequest = Units.ScreenUnitL * 8,
                //HeightRequest = 220,
                Aspect = Aspect.Fill,
                HorizontalOptions = LayoutOptions.CenterAndExpand,
                VerticalOptions = LayoutOptions.StartAndExpand
            };

            TopLogoOff = new Image
            {
                Source = "damain_off.png",
                //Source = "background2.png",
                WidthRequest = Units.ScreenWidth * 0.9,
                //HeightRequest = 120,
                Aspect = Aspect.AspectFit,
                HorizontalOptions = LayoutOptions.CenterAndExpand,
                VerticalOptions = LayoutOptions.CenterAndExpand
            };

            TopLogoOn = new Image
            {
                Source = "damain_on.png",
                //Source = "background2.png",
                WidthRequest = Units.ScreenWidth * 0.9,
                //HeightRequest = 120,
                Aspect = Aspect.AspectFit,
                HorizontalOptions = LayoutOptions.CenterAndExpand,
                VerticalOptions = LayoutOptions.CenterAndExpand
            };


            FooterBG = new Image
            {
                //Source = "carbonbg_foot2.png",
                //Source = "background2.png",
                Source = "big_strip2.png",
                WidthRequest = Units.ScreenWidth,
                HeightRequest = Units.ScreenUnitL * 3.5,
                Aspect = Aspect.Fill,
                HorizontalOptions = LayoutOptions.CenterAndExpand,
                VerticalOptions = LayoutOptions.EndAndExpand
            };

            LeftStrip = new Image
            {
                Source = "leftstrip.png",
                HorizontalOptions = LayoutOptions.StartAndExpand,
                VerticalOptions = LayoutOptions.StartAndExpand,
                Aspect = Aspect.AspectFill,

                //Margin = new Thickness(0, Units.ScreenUnitL * 5.5, 0, 0)
            };

            RightStrip = new Image
            {
                Source = "leftstrip.png",
                HorizontalOptions = LayoutOptions.StartAndExpand,
                VerticalOptions = LayoutOptions.StartAndExpand,
                Aspect = Aspect.AspectFill
                //Margin = new Thickness(0, Units.ScreenUnitL * 5.5, 0, 0)
            };


            StackLayout ScreenBorder = new StackLayout
            {
                WidthRequest = Units.ScreenWidth,
                HeightRequest = Units.ScreenHeight,
                HorizontalOptions = LayoutOptions.CenterAndExpand,
                VerticalOptions = LayoutOptions.StartAndExpand,
                Orientation = StackOrientation.Vertical,
                Spacing = 0
            };

            Grid HeaderGrid = new Grid
            {
                ColumnSpacing = 0,
                RowSpacing = 0
            };

            HeaderGrid.Children.Add(
                new StackLayout
                {
                    Spacing = 0,
                    Children =
                    {
                        HeaderBG
                    },
                }, 0, 0);

            InvestigationLabel = new Label
            {
                Text = "INVESTIGATION PENDING",
                FontSize = Units.DynamicFontSizeL,
                FontFamily = Fonts.GetHeaderFont(),
                TextColor = Color.White,
                Opacity = 0.75,
                WidthRequest = Units.ScreenWidth,
                //HeightRequest = 32,
                HorizontalOptions = LayoutOptions.CenterAndExpand,
                HorizontalTextAlignment = TextAlignment.Center,
                VerticalOptions = LayoutOptions.StartAndExpand,
                VerticalTextAlignment = TextAlignment.Start
            };

            Grid LogoSwap = new Grid
            {
                ColumnSpacing = 0,
                RowSpacing = 0,

            };

            LogoSwap.Children.Add(TopLogoOff, 0, 0);
            LogoSwap.Children.Add(TopLogoOn, 0, 0);

            PlusImage = new Image
            {
                Source = "less.png",
                WidthRequest = Units.ScreenUnitL,
                HorizontalOptions = LayoutOptions.CenterAndExpand,
                VerticalOptions = LayoutOptions.CenterAndExpand,
                Margin = new Thickness(0, 0, Units.ScreenUnitM, 0),
                Opacity = 0.25f
            };

            MinusImage = new Image
            {
                Source = "more.png",
                WidthRequest = Units.ScreenUnitL,
                HorizontalOptions = LayoutOptions.CenterAndExpand,
                VerticalOptions = LayoutOptions.CenterAndExpand,
                Margin = new Thickness(0, 0, Units.ScreenUnitM, 0),
                Opacity = 0.25
            };

            SoundButton = new Image
            {
                Source = "sound.png",
                WidthRequest = Units.ScreenUnitL,
                HorizontalOptions = LayoutOptions.StartAndExpand,
                VerticalOptions = LayoutOptions.CenterAndExpand,
                Margin = new Thickness(Units.ScreenWidth10Percent, 0, 0, 0),
                Opacity = 0.25
            };

            SpeechButton = new Image
            {
                Source = "mouth.png",
                WidthRequest = Units.ScreenUnitL,
                HorizontalOptions = LayoutOptions.StartAndExpand,
                VerticalOptions = LayoutOptions.CenterAndExpand,
                Margin = new Thickness(Units.ScreenWidth10Percent, 0, 0, 0),
                Opacity = 0.25
            };

            VibrationButton = new Image
            {
                Source = "vibrate.png",
                WidthRequest = Units.ScreenUnitL,
                HorizontalOptions = LayoutOptions.StartAndExpand,
                VerticalOptions = LayoutOptions.CenterAndExpand,
                Margin = new Thickness(Units.ScreenWidth10Percent, 0, 0, 0),
                Opacity = 0.25
            };

            HeaderGrid.Children.Add(
                new StackLayout
                {
                    Orientation = StackOrientation.Vertical,
                    Margin = new Thickness(0, Units.ScreenUnitM * 5.5, 0, Units.ScreenUnitS),
                    Spacing = Units.ScreenUnitXS,
                    Children =
                    {
                        LogoSwap,
                        new StackLayout
                        {
                            Orientation = StackOrientation.Horizontal,
                            Children =
                            {
                                // MinusImage,
                                 InvestigationLabel,
                                 //PlusImage
                            }
                        }

                    }
                }, 0, 0);


            StackLayout LeftArea = new StackLayout
            {
                Spacing = 0,
                WidthRequest = Units.ScreenUnitL,
                HeightRequest = Units.ScreenHeight,
                HorizontalOptions = LayoutOptions.StartAndExpand,
                VerticalOptions = LayoutOptions.CenterAndExpand,
                BackgroundColor = Color.Black,
                Children =
                {
                    LeftStrip
                }
            };

            StackLayout RightArea = new StackLayout
            {
                Spacing = 0,
                WidthRequest = Units.ScreenUnitL,
                HeightRequest = Units.ScreenHeight,
                HorizontalOptions = LayoutOptions.EndAndExpand,
                VerticalOptions = LayoutOptions.CenterAndExpand,
                BackgroundColor = Color.Black,
                Children =
                {
                    RightStrip
                }
            };

            ScreenBorder.Children.Add(HeaderGrid);




            //ScreenBorder.Children.Add(LeftStrip);


            //ScreenBorder.Children.Add(LeftStrip, 0, 1);
            //ScreenBorder.Children.Add(LeftStrip, 11, 1);
            ScreenBorder.Children.Add(FooterBG);

            //HeaderGrid.TranslateTo(0, -40, 0, Easing.Linear);


            //Grid.SetColumnSpan(HeaderBG, 12);
            //Grid.SetColumnSpan(FooterBG, 12);
            //Grid.SetRowSpan(LeftStrip, 8);

            Blackout = new Image
            {
                Source = "blackout.png",
                WidthRequest = Units.ScreenWidth,
                HeightRequest = Units.ScreenHeight,
                Aspect = Aspect.AspectFit
            };

            Whiteout = new Image
            {
                Source = "whiteout.png",
                WidthRequest = Units.ScreenWidth,
                HeightRequest = Units.ScreenHeight,
                Aspect = Aspect.AspectFit
            };

            Mask = new Image
            {
                Source = "mask.png",
                WidthRequest = Units.ScreenWidth,
                HeightRequest = Units.ScreenHeight,
                Aspect = Aspect.AspectFit
            };

            Circuit = new Image
            {
                Source = "piano1.png",
                WidthRequest = Units.ScreenWidth,
                HeightRequest = Units.ScreenHeight,
                Aspect = Aspect.AspectFit
            };

            int maxWidth = Units.ScreenWidth;
            int maxHeight = Units.ScreenHeight;
            if (Device.Idiom == TargetIdiom.Tablet)
            {
                maxWidth = Units.ThirdScreenWidth;
                maxHeight = Units.ScreenWidth;
            }



            PowerImage = new Image
            {
                Source = "power_off.png",//"power2off.png",
                WidthRequest = 52,
                HeightRequest = 52,
                Aspect = Aspect.AspectFit,
                HorizontalOptions = LayoutOptions.CenterAndExpand,
                VerticalOptions = LayoutOptions.EndAndExpand,
                Margin = new Thickness(24, 8)
            };

            StartStopImage = new Image
            {
                Source = "start2.png",
                WidthRequest = Units.ScreenUnitM * 3,
                HeightRequest = Units.ScreenUnitM * 3,
                Aspect = Aspect.AspectFit,
                HorizontalOptions = LayoutOptions.CenterAndExpand,
                VerticalOptions = LayoutOptions.EndAndExpand,
                Margin = 8
            };

            HistoryImage = new Image
            {
                //Source = "historybtn.png",
                Source = "copy_off.png",
                WidthRequest = 48,
                HeightRequest = 48,
                Aspect = Aspect.AspectFit,
                HorizontalOptions = LayoutOptions.CenterAndExpand,
                VerticalOptions = LayoutOptions.EndAndExpand,
                Margin = new Thickness(24, 8)
            };

            InfoImage = new Image
            {
                //Source = "historybtn.png",
                Source = "info_off.png",
                WidthRequest = 48,
                HeightRequest = 48,
                Aspect = Aspect.AspectFit,
                HorizontalOptions = LayoutOptions.CenterAndExpand,
                VerticalOptions = LayoutOptions.EndAndExpand,
                Margin = new Thickness(24, 8)
            };

            ClearImage = new Image
            {
                //Source = "clearbtn.png",
                Source = "eraser.png",
                WidthRequest = Units.ScreenUnitM * 2,
                HeightRequest = Units.ScreenUnitM * 2,
                Aspect = Aspect.AspectFit,
                HorizontalOptions = LayoutOptions.CenterAndExpand,
                VerticalOptions = LayoutOptions.EndAndExpand,
                Margin = 8
            };

            PlusImage.GestureRecognizers.Add(
                  new TapGestureRecognizer()
                  {
                      Command = new Command(() =>
                      {
                          //Device.BeginInvokeOnMainThread(async () =>
                          //{
                          //if (PlusImage.Opacity > 0.25)
                          {
                              SlowDownScan(false);
                              UpdateElements();
                          }
                          //});
                      })
                  }
              );

            MinusImage.GestureRecognizers.Add(
                  new TapGestureRecognizer()
                  {
                      Command = new Command(() =>
                      {
                          //Device.BeginInvokeOnMainThread(async () =>
                          //{
                          if (MinusImage.Opacity > 0.25)
                          {
                              SpeedUpScan(false);
                              UpdateElements();
                          }
                          //});
                      })
                  }
              );

            StartStopImage.GestureRecognizers.Add(
                  new TapGestureRecognizer()
                  {
                      Command = new Command(() =>
                      {
                          if (DeadboxOn)
                          {
                              PlaySound("click");
                              ToggleStartStop();
                          }
                      })
                  }
              );

            PowerImage.GestureRecognizers.Add(
                  new TapGestureRecognizer()
                  {
                      Command = new Command(() =>
                      {
                          //Device.BeginInvokeOnMainThread(async () =>
                          //{
                          //if (DeadboxOn)
                          //{
                          PlaySound("click");
                          ToggleMainOnOff();
                          //}

                          //});
                      })
                  }
              );

            ClearImage.GestureRecognizers.Add(
                 new TapGestureRecognizer()
                 {
                     Command = new Command(() =>
                     {
                         if (DeadboxOn)
                         {
                             //Device.BeginInvokeOnMainThread(async () =>
                             //{
                             if (LoggedWordsList != null)
                             {
                                 if (LoggedWordsList.Count > 0)
                                 {
                                     PlaySound("click");
                                     LoggedWordsList.Clear();
                                     CurrentWord.Text = "";
                                     UpdateElements();
                                     ChangeLocation();
                                 }
                             }
                             //});
                         }
                     })
                 }
             );

            HistoryImage.GestureRecognizers.Add(
                 new TapGestureRecognizer()
                 {
                     Command = new Command(() =>
                     {

                         if (DeadboxOn)
                         {
                             //ToggleStartStop();


                             if (LoggedWordsList != null)
                             {
                                 if (LoggedWordsList.Count > 0)
                                 {
                                     PlaySound("click");
                                     ShowHistory();
                                 }
                             }
                         }
                     })
                 }
             );

            InfoImage.GestureRecognizers.Add(
                 new TapGestureRecognizer()
                 {
                     Command = new Command(() =>
                     {
                         if (DeadboxOn)
                         {
                             PlaySound("click");
                             ShowGeneralInfo();
                         }
                     })
                 }
             );

            // START COMMANDS
            // SET 1
            Tune1 = new Label
            {
                Text = "VIBE\nONE",
                FontSize = Units.FontSizeM,
                FontFamily = Fonts.GetBoldAppFont(),
                TextColor = Color.FromHex("ffffff"),
                Opacity = 1.0f,
                HorizontalOptions = LayoutOptions.Center,
                HorizontalTextAlignment = TextAlignment.Center,
                VerticalOptions = LayoutOptions.StartAndExpand,
                VerticalTextAlignment = TextAlignment.Start
            };

            Tune1.GestureRecognizers.Add(
                 new TapGestureRecognizer()
                 {
                      Command = new Command(() =>
                      {
                          if (DeadboxOn)
                          {
                              if (TunesAvailable > 0)
                              {
                                  TriggerMelody(1, selectedInstrument, false);
                                  UpdateElements();
                              }
                          }
                      })
                 }
             );

            Phrase1 = new Label
            {
                Text = "MAJOR\nVIBES",
                FontSize = Units.FontSizeM,
                FontFamily = Fonts.GetBoldAppFont(),
                TextColor = Color.FromHex("ffffff"),
                Opacity = 1.0f,
                HorizontalOptions = LayoutOptions.Center,
                HorizontalTextAlignment = TextAlignment.Center,
                VerticalOptions = LayoutOptions.StartAndExpand,
                VerticalTextAlignment = TextAlignment.Start
            };

            Phrase1.GestureRecognizers.Add(
                 new TapGestureRecognizer()
                 {
                     Command = new Command(() =>
                     {
                         if (DeadboxOn)
                         {
                             if (MoodsAvailable > 0)
                             {
                                 SetMood(0);
                                 //TriggerPhrase(1, false);
                                 UpdateElements();
                             }
                         }
                     })
                 }
             );

            // SET 2
            Tune2 = new Label
            {
                Text = "VIBE\nTWO",
                FontSize = Units.FontSizeM,
                FontFamily = Fonts.GetBoldAppFont(),
                TextColor = Color.FromHex("ffffff"),
                Opacity = 1.0f,
                HorizontalOptions = LayoutOptions.Center,
                HorizontalTextAlignment = TextAlignment.Center,
                VerticalOptions = LayoutOptions.StartAndExpand,
                VerticalTextAlignment = TextAlignment.Start
            };

            Tune2.GestureRecognizers.Add(
                 new TapGestureRecognizer()
                 {
                     Command = new Command(() =>
                     {
                         if (DeadboxOn)
                         {
                             if (TunesAvailable > 1)
                             {
                                 TriggerMelody(2, selectedInstrument, false);
                                 UpdateElements();
                             }
                         }
                     })
                 }
             );

            Phrase2 = new Label
            {
                Text = "MINOR\nVIBES",
                FontSize = Units.FontSizeM,
                FontFamily = Fonts.GetBoldAppFont(),
                TextColor = Color.FromHex("ffffff"),
                Opacity = 1.0f,
                HorizontalOptions = LayoutOptions.Center,
                HorizontalTextAlignment = TextAlignment.Center,
                VerticalOptions = LayoutOptions.StartAndExpand,
                VerticalTextAlignment = TextAlignment.Start
            };

            Phrase2.GestureRecognizers.Add(
                 new TapGestureRecognizer()
                 {
                     Command = new Command(() =>
                     {
                         if (DeadboxOn)
                         {
                             if (MoodsAvailable > 1)
                             {
                                 SetMood(1);
                                 //TriggerPhrase(2, false);
                                 UpdateElements();
                             }
                         }
                     })
                 }
             );

            // SET 3
            Tune3 = new Label
            {
                Text = "VIBE\nTHREE",
                FontSize = Units.FontSizeM,
                FontFamily = Fonts.GetBoldAppFont(),
                TextColor = Color.FromHex("ffffff"),
                Opacity = 1.0f,
                HorizontalOptions = LayoutOptions.Center,
                HorizontalTextAlignment = TextAlignment.Center,
                VerticalOptions = LayoutOptions.StartAndExpand,
                VerticalTextAlignment = TextAlignment.Start
            };

            Tune3.GestureRecognizers.Add(
                 new TapGestureRecognizer()
                 {
                     Command = new Command(() =>
                     {
                         if (DeadboxOn)
                         {
                             if (TunesAvailable > 2)
                             {
                                 TriggerMelody(3, selectedInstrument, false);
                                 UpdateElements();
                             }
                         }
                     })
                 }
             );

            Phrase3 = new Label
            {
                Text = "RANDOM\nVIBES",
                FontSize = Units.FontSizeM,
                FontFamily = Fonts.GetBoldAppFont(),
                TextColor = Color.FromHex("ffffff"),
                Opacity = 1.0f,
                HorizontalOptions = LayoutOptions.Center,
                HorizontalTextAlignment = TextAlignment.Center,
                VerticalOptions = LayoutOptions.StartAndExpand,
                VerticalTextAlignment = TextAlignment.Start
            };

            Phrase3.GestureRecognizers.Add(
                 new TapGestureRecognizer()
                 {
                     Command = new Command(() =>
                     {
                         if (DeadboxOn)
                         {
                             if (MoodsAvailable > 2)
                             {
                                 SetMood(2);
                                 //TriggerPhrase(3, false);
                                 UpdateElements();
                             }
                         }
                     })
                 }
             );

            // SET 4
            Tune4 = new Label
            {
                Text = "VIBE\nFOUR",
                FontSize = Units.FontSizeM,
                FontFamily = Fonts.GetBoldAppFont(),
                TextColor = Color.FromHex("ffffff"),
                Opacity = 1.0f,
                HorizontalOptions = LayoutOptions.Center,
                HorizontalTextAlignment = TextAlignment.Center,
                VerticalOptions = LayoutOptions.StartAndExpand,
                VerticalTextAlignment = TextAlignment.Start
            };

            Tune4.GestureRecognizers.Add(
                 new TapGestureRecognizer()
                 {
                     Command = new Command(() =>
                     {
                         if (DeadboxOn)
                         {
                             if (TunesAvailable > 3)
                             {
                                 TriggerMelody(4, selectedInstrument, false);
                                 UpdateElements();
                             }
                         }
                     })
                 }
             );

            Phrase4 = new Label
            {
                Text = "SESSION\nVIBES",
                FontSize = Units.FontSizeM,
                FontFamily = Fonts.GetBoldAppFont(),
                TextColor = Color.FromHex("ffffff"),
                Opacity = 1.0f,
                HorizontalOptions = LayoutOptions.Center,
                HorizontalTextAlignment = TextAlignment.Center,
                VerticalOptions = LayoutOptions.StartAndExpand,
                VerticalTextAlignment = TextAlignment.Start
            };

            Phrase4.GestureRecognizers.Add(
                 new TapGestureRecognizer()
                 {
                     Command = new Command(() =>
                     {
                         if (DeadboxOn)
                         {
                             if (MoodsAvailable > 3)
                             {
                                 SetMood(3);
                                 //TriggerPhrase(4, false);
                                 UpdateElements();
                             }
                         }
                     })
                 }
             );


            Tune5 = new Label
            {
                Text = "PLACE\nVIBE",
                FontSize = Units.FontSizeM,
                FontFamily = Fonts.GetBoldAppFont(),
                TextColor = Color.FromHex("ffffff"),
                Opacity = 1.0f,
                HorizontalOptions = LayoutOptions.Center,
                HorizontalTextAlignment = TextAlignment.Center,
                VerticalOptions = LayoutOptions.StartAndExpand,
                VerticalTextAlignment = TextAlignment.Start
            };

            Tune5.GestureRecognizers.Add(
                 new TapGestureRecognizer()
                 {
                     Command = new Command(() =>
                     {
                         if (DeadboxOn)
                         {
                             if (TunesAvailable > 4)
                             {
                                 if (GotLocation)
                                 {
                                     TriggerMelody(5, selectedInstrument, false);
                                     UpdateElements();
                                 }
                             }
                         }
                     })
                 }
             );

            Tune6 = new Label
            {
                Text = "TWINKLE\nTWINKLE",
                FontSize = Units.FontSizeM,
                FontFamily = Fonts.GetBoldAppFont(),
                TextColor = Color.FromHex("ffffff"),
                Opacity = 1.0f,
                HorizontalOptions = LayoutOptions.Center,
                HorizontalTextAlignment = TextAlignment.Center,
                VerticalOptions = LayoutOptions.StartAndExpand,
                VerticalTextAlignment = TextAlignment.Start
            };

            Tune6.GestureRecognizers.Add(
                 new TapGestureRecognizer()
                 {
                     Command = new Command(() =>
                     {
                         if (DeadboxOn)
                         {
                             if (TunesAvailable > 5)
                             {
                                 if (Mood <= 1)
                                 {
                                     TriggerMelody(6, selectedInstrument, false);
                                     UpdateElements();
                                 }
                             }
                         }
                     })
                 }
             );


            // END COMMANDS


            ToyPiano = new Label
            {
                Text = "TOY",
                FontSize = Units.DynamicFontSizeXXXL,
                FontFamily = Fonts.GetBoldAppFont(),
                TextColor = Color.FromHex("ffffff"),
                Opacity = 1.0f,
                HorizontalOptions = LayoutOptions.Center,
                HorizontalTextAlignment = TextAlignment.Center,
                VerticalOptions = LayoutOptions.EndAndExpand,
                VerticalTextAlignment = TextAlignment.End
            };

            ToyPiano.GestureRecognizers.Add(
                 new TapGestureRecognizer()
                 {
                     Command = new Command(() =>
                     {

                         if (DeadboxOn)
                         {
                             // switch to toy piano
                             //selectedInstrument = (int)SoundSources.ToyPiano;
                             //AudioManager.SetInstrument(selectedInstrument);
                             //AudioManager.SetSoundSource(selectedInstrument);

                             // trigger rapid event
                             //TriggerRapidEvents();

                             // play signature melody

                             SignatureMelody = AudioManager.GetSignatureMelody();
                             WordsToSpeak = PhraseManager.GetDarkPhrase();

                             //IsSpeaking = false; 
                             //playSignatureMelody = true;

                             //WordsToSpeak = PhraseManager.GetSessionPhrase(1);
                             //SignatureMelody = AudioManager.GetSessionMelody(1);

                             //MelodyCount = 0;

                             IsSpeaking = false;
                             playSignatureMelody = true;
                             selectedInstrument = (int)SoundSources.Noise;
                             SignatureMelody = AudioManager.GetSpooky();
                             MelodyCount = 0;
                             UpdateElements();

                         }
                     })
                 }
             );

            PianoSelect = new Label
            {
                Text = "PIANO",
                FontSize = Units.DynamicFontSizeXL,
                FontFamily = Fonts.GetBoldAppFont(),
                TextColor = Color.FromHex("ffffff"),
                Opacity = 1.0f,
                HorizontalOptions = LayoutOptions.Center,
                HorizontalTextAlignment = TextAlignment.Center,
                VerticalOptions = LayoutOptions.StartAndExpand,
                VerticalTextAlignment = TextAlignment.Start
            };

            PianoSelect.GestureRecognizers.Add(
                 new TapGestureRecognizer()
                 {
                     Command = new Command(() =>
                     {

                         if (DeadboxOn)
                         {
                             selectedInstrument = (int)SoundSources.Piano;
                             //PianoSelect.Opacity = 1.0f;
                             //ToySelect.Opacity = OffTextOpacity;
                             //ToneSelect.Opacity = OffTextOpacity;
                             UpdateElements();
                         }
                     })
                 }
             );

            ToySelect = new Label
            {
                Text = "TOY",
                FontSize = Units.DynamicFontSizeXL,
                FontFamily = Fonts.GetBoldAppFont(),
                TextColor = Color.FromHex("ffffff"),
                Opacity = 1.0f,
                HorizontalOptions = LayoutOptions.Center,
                HorizontalTextAlignment = TextAlignment.Center,
                VerticalOptions = LayoutOptions.StartAndExpand,
                VerticalTextAlignment = TextAlignment.Start
            };

            ToySelect.GestureRecognizers.Add(
                 new TapGestureRecognizer()
                 {
                     Command = new Command(() =>
                     {

                         if (DeadboxOn)
                         {
                             selectedInstrument = (int)SoundSources.ToyPiano;
                             //PianoSelect.Opacity = OffTextOpacity;
                             //ToySelect.Opacity = 1.0f;
                             //ToneSelect.Opacity = OffTextOpacity;
                             UpdateElements();
                         }
                     })
                 }
             );

            ToneSelect = new Label
            {
                Text = "TONE",
                FontSize = Units.DynamicFontSizeXL,
                FontFamily = Fonts.GetBoldAppFont(),
                TextColor = Color.FromHex("ffffff"),
                Opacity = 1.0f,
                HorizontalOptions = LayoutOptions.Center,
                HorizontalTextAlignment = TextAlignment.Center,
                VerticalOptions = LayoutOptions.StartAndExpand,
                VerticalTextAlignment = TextAlignment.Start
            };

            ToneSelect.GestureRecognizers.Add(
                 new TapGestureRecognizer()
                 {
                     Command = new Command(() =>
                     {

                         if (DeadboxOn)
                         {
                             selectedInstrument = (int)SoundSources.Tone;
                             //PianoSelect.Opacity = OffTextOpacity;
                             //ToySelect.Opacity = OffTextOpacity;
                             //ToneSelect.Opacity = 1.0f;
                             UpdateElements();
                         }
                     })
                 }
             );


            GrandPiano = new Label
            {
                Text = "GRAND",
                FontSize = Units.DynamicFontSizeXXXL,
                FontFamily = Fonts.GetBoldAppFont(),
                TextColor = Color.FromHex("ffffff"),
                Opacity = 1.0f,
                HorizontalOptions = LayoutOptions.Center,
                HorizontalTextAlignment = TextAlignment.Center,
                VerticalOptions = LayoutOptions.CenterAndExpand,
                VerticalTextAlignment = TextAlignment.Center
            };

            GrandPiano.GestureRecognizers.Add(
                 new TapGestureRecognizer()
                 {
                     Command = new Command(() =>
                     {

                         if (DeadboxOn)
                         {
                             //selectedInstrument = (int)SoundSources.Noise;
                             //AudioManager.SetInstrument(selectedInstrument);
                             //AudioManager.SetSoundSource(selectedInstrument);
                             //TriggerRapidEvents();

                             IsSpeaking = false;
                             playSignatureMelody = true;
                             selectedInstrument = (int)SoundSources.Piano;
                             SignatureMelody = AudioManager.GetMinorTune();
                             MelodyCount = 0;
                             UpdateElements();
                         }
                     })
                 }
             );

            CurrentWord = new Label
            {
                Text = "",
                FontSize = Units.DynamicFontSizeXXXXL,
                FontFamily = Fonts.GetBoldAppFont(),
                TextColor = Color.FromHex(YellowColour),
                Opacity = 1.0f,
                HorizontalOptions = LayoutOptions.Center,
                HorizontalTextAlignment = TextAlignment.Center,
                VerticalOptions = LayoutOptions.Center,
                VerticalTextAlignment = TextAlignment.Center,
                HeightRequest = Units.ScreenHeight,
                Margin = new Thickness(0, 140, 0, 0)
            };

            SeekingWord = new Label
            {
                Text = "",
                FontSize = Units.DynamicFontSizeXXXL,
                FontFamily = Fonts.GetBoldAppFont(),
                TextColor = Color.FromHex(YellowColour),
                Opacity = OffTextOpacity,
                HorizontalOptions = LayoutOptions.Center,
                HorizontalTextAlignment = TextAlignment.Center,
                VerticalOptions = LayoutOptions.Center,
                VerticalTextAlignment = TextAlignment.Center,
                HeightRequest = Units.ScreenHeight,
                Margin = new Thickness(0, 140, 0, 0)
            };

            WordHistory = new Label
            {
                Text = "",
                FontSize = Units.DynamicFontSizeXL,
                FontFamily = Fonts.GetBoldAppFont(),
                TextColor = Color.FromHex(YellowColour),
                Opacity = 1.0f,
                HorizontalOptions = LayoutOptions.CenterAndExpand,
                HorizontalTextAlignment = TextAlignment.Center,
                VerticalOptions = LayoutOptions.Start,
                VerticalTextAlignment = TextAlignment.Start
            };

            ThinkingIndicator = new Label
            {
                Text = "",
                FontSize = Units.DynamicFontSizeL,
                FontFamily = Fonts.GetBoldAppFont(),
                TextColor = Color.White,//FromHex("86eb2e"),
                Opacity = 0.5,
                HorizontalOptions = LayoutOptions.Center,
                HorizontalTextAlignment = TextAlignment.Center,
                VerticalOptions = LayoutOptions.End,
                VerticalTextAlignment = TextAlignment.End,
                HeightRequest = Units.HalfScreenHeight
            };


            ScanSpeedLabel = new Label
            {
                Text = "SCAN RATE:" + ScanFrequency,
                FontSize = Units.DynamicFontSizeM,
                FontFamily = Fonts.GetBoldAppFont(),
                TextColor = Color.FromHex("86eb2e"),
                Opacity = 0.75,
                Margin = 8,
                WidthRequest = Units.ThirdScreenWidth,
                HorizontalOptions = LayoutOptions.EndAndExpand,
                HorizontalTextAlignment = TextAlignment.End,
                VerticalOptions = LayoutOptions.End,
                VerticalTextAlignment = TextAlignment.End
            };

            SpiritStrengthLabel = new Label
            {
                Text = "POWER: NEUTRAL",
                FontSize = Units.DynamicFontSizeM,
                FontFamily = Fonts.GetBoldAppFont(),
                TextColor = Color.FromHex("86eb2e"),
                Opacity = 0.75,
                Margin = 8,
                WidthRequest = Units.ThirdScreenWidth,
                HorizontalOptions = LayoutOptions.EndAndExpand,
                HorizontalTextAlignment = TextAlignment.End,
                VerticalOptions = LayoutOptions.End,
                VerticalTextAlignment = TextAlignment.End
            };

            SpiritMoodLabel = new Label
            {
                Text = "MOOD: PASSIVE",
                FontSize = Units.DynamicFontSizeM,
                FontFamily = Fonts.GetBoldAppFont(),
                TextColor = Color.FromHex("86eb2e"),
                Opacity = 0.75,
                Margin = 8,
                WidthRequest = Units.ThirdScreenWidth,
                HorizontalOptions = LayoutOptions.EndAndExpand,
                HorizontalTextAlignment = TextAlignment.End,
                VerticalOptions = LayoutOptions.End,
                VerticalTextAlignment = TextAlignment.End
            };

            SpeechToggle = new Label
            {
                Text = "SPEECH\nON",
                FontSize = Units.DynamicFontSizeL,
                FontFamily = Fonts.GetBoldAppFont(),
                TextColor = Color.White,
                Opacity = 0.75,
                Margin = 10,
                WidthRequest = Units.ThirdScreenWidth,
                //HeightRequest = 52,
                HorizontalOptions = LayoutOptions.CenterAndExpand,
                HorizontalTextAlignment = TextAlignment.Center,
                VerticalOptions = LayoutOptions.CenterAndExpand,
                VerticalTextAlignment = TextAlignment.Center
            };

            //SpeechToggle.GestureRecognizers.Add(
            SpeechButton.GestureRecognizers.Add(
                 new TapGestureRecognizer()
                 {
                     Command = new Command(() =>
                     {
                         if (DeadboxOn)
                         {
                             //Device.BeginInvokeOnMainThread(() =>
                             //{
                             SpeechOn = !SpeechOn;
                             UpdateElements();
                             //});
                         }
                     })
                 }
             );

            VibrationToggle = new Label
            {
                Text = "VIBRATION\nON",
                FontSize = Units.DynamicFontSizeL,
                FontFamily = Fonts.GetBoldAppFont(),
                TextColor = Color.White,
                Opacity = 0.75,
                Margin = 10,
                WidthRequest = Units.ThirdScreenWidth,
                // HeightRequest = 52,
                HorizontalOptions = LayoutOptions.CenterAndExpand,
                HorizontalTextAlignment = TextAlignment.Center,
                VerticalOptions = LayoutOptions.CenterAndExpand,
                VerticalTextAlignment = TextAlignment.Center
            };

            VibrationButton.GestureRecognizers.Add(
            //VibrationToggle.GestureRecognizers.Add(
                 new TapGestureRecognizer()
                 {
                     Command = new Command(() =>
                     {
                         if (DeadboxOn)
                         {
                             //Device.BeginInvokeOnMainThread(() =>
                             //{
                             VibrateOn = !VibrateOn;
                             UpdateElements();
                             //});
                         }
                     })
                 }
             );

            SoundToggle = new Label
            {
                Text = "SOUND\nOFF",
                FontSize = Units.DynamicFontSizeL,
                FontFamily = Fonts.GetBoldAppFont(),
                TextColor = Color.White,
                Opacity = 0.75,
                Margin = 10,
                WidthRequest = Units.ThirdScreenWidth,
                //HeightRequest = 52,
                HorizontalOptions = LayoutOptions.CenterAndExpand,
                HorizontalTextAlignment = TextAlignment.Center,
                VerticalOptions = LayoutOptions.CenterAndExpand,
                VerticalTextAlignment = TextAlignment.Center
            };

            SoundButton.GestureRecognizers.Add(
            //SoundToggle.GestureRecognizers.Add(
                 new TapGestureRecognizer()
                 {
                     Command = new Command(() =>
                     {
                         if (DeadboxOn)
                         {
                             //Device.BeginInvokeOnMainThread(() =>
                             //{
                             SoundOn = !SoundOn;
                             UpdateElements();
                             //});
                         }
                     })
                 }
             );

            SaveHistory = new Label
            {
                Text = "SAVE",
                FontSize = Units.DynamicFontSizeXXXXL,
                FontFamily = Fonts.GetBoldAppFont(),
                TextColor = Color.White,
                Opacity = 0.75,
                Margin = 10,
                WidthRequest = Units.ThirdScreenWidth,
                //HeightRequest = 52,
                HorizontalOptions = LayoutOptions.CenterAndExpand,
                HorizontalTextAlignment = TextAlignment.Center,
                VerticalOptions = LayoutOptions.EndAndExpand,
                VerticalTextAlignment = TextAlignment.End
            };

            SaveHistory.GestureRecognizers.Add(
                 new TapGestureRecognizer()
                 {
                     Command = new Command(() =>
                     {
                         Device.BeginInvokeOnMainThread(() =>
                         {
                             SaveHistoryList();
                             //HideHistory();
                         });
                     })
                 }
             );


            CloseHistory = new Label
            {
                Text = "CLOSE",
                FontSize = Units.DynamicFontSizeXXXXL,
                FontFamily = Fonts.GetBoldAppFont(),
                TextColor = Color.White,
                Opacity = 0.75,
                Margin = 10,
                WidthRequest = Units.ThirdScreenWidth,
                //HeightRequest = 52,
                HorizontalOptions = LayoutOptions.CenterAndExpand,
                HorizontalTextAlignment = TextAlignment.Center,
                VerticalOptions = LayoutOptions.EndAndExpand,
                VerticalTextAlignment = TextAlignment.End
            };


            WordHistoryLabel = new Label
            {
                Text = "WORD HISTORY",
                FontSize = 16,
                FontFamily = Fonts.GetBoldAppFont(),
                TextColor = Color.White,
                Opacity = 0.75,
                Margin = new Thickness(0, 64, 0, 8),
                WidthRequest = Units.ThirdScreenWidth,
                HorizontalOptions = LayoutOptions.CenterAndExpand,
                HorizontalTextAlignment = TextAlignment.Center,
                VerticalOptions = LayoutOptions.StartAndExpand,
                VerticalTextAlignment = TextAlignment.Start
            };

            CloseHistory.GestureRecognizers.Add(
                 new TapGestureRecognizer()
                 {
                     Command = new Command(() =>
                     {
                         Device.BeginInvokeOnMainThread(() =>
                         {
                             if (InfoType == WORD_HISTORY)
                             {
                                 HideHistory();
                             }
                             else
                             {
                                 HideGeneralInfo();
                             }
                         });
                     })
                 }
             );

            Grid WordGrid = new Grid { };

            WordGrid.Children.Add(CurrentWord, 0, 0);
            WordGrid.Children.Add(SeekingWord, 0, 0);

            StackLayout ContentContainer = new StackLayout
            {
                Orientation = StackOrientation.Vertical,
                
                Padding = new Thickness(0, 16),
                Children =
                {
                    //ControlsLayout,
                    WordGrid,
                    new Grid
                    {

                    },
                    new StackLayout
                    {
                        Orientation = StackOrientation.Horizontal,
                        HorizontalOptions = LayoutOptions.CenterAndExpand,
                        VerticalOptions = LayoutOptions.EndAndExpand,
                        HeightRequest = 132,
                        Padding = 8,
                        Children =
                        {
                            //ControlsPanel
                            
                        }
                    }

                }
            };

            StackLayout TopContentContainer = new StackLayout
            {
                Orientation = StackOrientation.Horizontal,
                VerticalOptions = LayoutOptions.Start,
                //Padding = new Thickness(0, 0, 0, 16),
                //Margin = new Thickness(0, 0, 0, 16),
                Children =
                {
                    SpeechToggle,
                    VibrationToggle,
                    SoundToggle
                }
            };

            PageContent.VerticalOptions = LayoutOptions.EndAndExpand;

            PageContent.Children.Add(CircuitAnimImage, 0, 0);
            PageContent.Children.Add(Circuit, 0, 0);
            //PageContent.Children.Add(Blackout, 0, 0);
            //PageContent.Children.Add(Whiteout, 0, 0);
            //PageContent.Children.Add(TopContentContainer, 0, 0);
            PageContent.Children.Add(ContentContainer, 0, 0);
            //
            PageContent.Children.Add(ThinkingIndicator, 0, 0);


            Image PianoKeysImage = new Image
            {
                Source = "pianokeys.jpg",
                Aspect = Aspect.AspectFit,
                WidthRequest = Units.ScreenWidth,
                VerticalOptions = LayoutOptions.StartAndExpand,
                Margin = new Thickness(0, 0, 0, 0)
            };


            Grid TopControlsGrid = new Grid
            {

                Margin = new Thickness(0, Units.ScreenUnitL*8, 0, 0),
                HeightRequest = Units.QuarterScreenHeight
            };

            //TopControlsGrid.Children.Add(PianoKeysImage, 0, 0);

            StackLayout sourceControl = new StackLayout
            {
                Orientation = StackOrientation.Horizontal,
                HorizontalOptions = LayoutOptions.CenterAndExpand,
                WidthRequest = Units.ScreenWidth,
                BackgroundColor = Color.Transparent,
                HeightRequest = 120,
                Padding = new Thickness(24, 0),
                Children =
                {
                    PianoSelect,
                    ToySelect,
                    ToneSelect
                }
            };

            //TopControlsGrid.Children.Add(sourceControl);


            //Grid.SetColumnSpan(sourceControl, 2);
            PageContent.Children.Add(TopControlsGrid, 0, 0);


            //PageContent.Children.Add(Mask, 0, 0);


            PageContent.Children.Add(new StackLayout {
                Orientation = StackOrientation.Horizontal,
                Children =
                {
                    //LeftArea,
                    //RightArea
                }
            }, 0, 0);


            PageContent.Children.Add(ScreenBorder, 0, 0);


            Grid ControlsGrid = new Grid
            {
                HeightRequest = Units.ScreenHeight,
                WidthRequest = Units.ScreenWidth,
                Margin = new Thickness(0, 0, 0, 24)
            };

            StackLayout TopControls = new StackLayout
            {
                Orientation = StackOrientation.Vertical,
                VerticalOptions = LayoutOptions.StartAndExpand,
                Padding = new Thickness(0, SafeArea + Units.ScreenUnitM * 6, 0, 0),
                Children =
                {
                    //InvestigationLabel,
                    new StackLayout
                    {
                        Orientation = StackOrientation.Horizontal,
                        VerticalOptions = LayoutOptions.StartAndExpand,
                        HorizontalOptions = LayoutOptions.CenterAndExpand,
                        WidthRequest = Units.ScreenWidth,
                        Spacing = 4,
                        Children =
                        {
                            //InvestigationLabel
                            //SpeechToggle,
                            //VibrationToggle,
                            //SoundToggle
                        }
                    }
                }
            };

            StackLayout BottomControls = new StackLayout
            {
                Orientation = StackOrientation.Horizontal,
                VerticalOptions = LayoutOptions.EndAndExpand,
                HorizontalOptions = LayoutOptions.CenterAndExpand,
                WidthRequest = Units.ScreenWidth,
                Spacing = 4,
                Padding = new Thickness(8, 0, 8, 16),
                Children =
                {
                    
                    //MinusImage,
                    //PlusImage,
                    ClearImage,
                    //StartStopImage,
                    PowerImage,
                    HistoryImage,



                }
            };

            StackLayout LeftControls = new StackLayout
            {
                Orientation = StackOrientation.Vertical,
                VerticalOptions = LayoutOptions.CenterAndExpand,
                Spacing = Units.ScreenHeight10Percent,
                Margin = new Thickness(0, Units.ScreenHeight10Percent),
                Children =
                {
                    new StackLayout
                    {
                        Orientation = StackOrientation.Vertical,
                        VerticalOptions = LayoutOptions.StartAndExpand,
                        Spacing = Units.ScreenHeight5Percent,
                        Children =
                        {
                            Phrase1,
                            Phrase2,
                            Phrase3,
                            Phrase4
                        }
                    },
                    new StackLayout
                    {
                        Orientation = StackOrientation.Vertical,
                        VerticalOptions = LayoutOptions.EndAndExpand,
                        Spacing = Units.ScreenHeight5Percent,
                        Children =
                        {
                            //HistoryImage
                        }
                    }
                }
            };

            StackLayout RightControls = new StackLayout
            {
                Orientation = StackOrientation.Vertical,
                VerticalOptions = LayoutOptions.CenterAndExpand,
                Spacing = Units.ScreenHeight10Percent,
                Margin = new Thickness(0, Units.ScreenHeight10Percent),
                Children =
                {
                    new StackLayout
                    {
                        Orientation = StackOrientation.Vertical,
                        VerticalOptions = LayoutOptions.StartAndExpand,
                        Spacing = Units.ScreenHeight2Percent,
                        Children =
                        {
                            Tune1,
                            Tune2,
                            Tune3,
                            Tune4,
                            Tune5,
                            Tune6
                        }
                    },
                    new StackLayout
                    {
                        Orientation = StackOrientation.Vertical,
                        VerticalOptions = LayoutOptions.EndAndExpand,
                        Spacing = Units.ScreenHeight5Percent,
                        Children =
                        {
                            //InfoImage 
                        }
                    }
                }
            };

            //ControlsGrid.Children.Add(sourceControl, 0, 4);
            //Grid.SetColumnSpan(sourceControl, 5);

            ControlsGrid.Children.Add(PianoSelect, 1, 0);
            ControlsGrid.Children.Add(ToySelect, 2, 0);
            ControlsGrid.Children.Add(ToneSelect, 3, 0);

            ControlsGrid.Children.Add(LeftControls, 0, 4);
            ControlsGrid.Children.Add(RightControls, 4, 4);


            //ControlsGrid.Children.Add(Tune5, 1, 4);
            //ControlsGrid.Children.Add(Tune6, 2, 4);

            //Grid.SetRowSpan(Tune5, 8);
            //Grid.SetRowSpan(Tune6, 8);



            LeftControls.Opacity = 1;
            RightControls.Opacity = 1;

            //ControlsGrid.Children.Add(ClearImage, 0, 11);
            //ControlsGrid.Children.Add(StartStopImage, 2, 11);
            //ControlsGrid.Children.Add(GrandPiano, 0, 12);        
            //ControlsGrid.Children.Add(PowerImage, 1, 12);
            //ControlsGrid.Children.Add(ToyPiano, 2, 12);

            ControlsGrid.Children.Add(InfoImage, 0, 12);
            ControlsGrid.Children.Add(PowerImage, 1, 12);
            ControlsGrid.Children.Add(HistoryImage, 2, 12);




            Grid.SetRowSpan(PowerImage, 3);
            Grid.SetColumnSpan(PowerImage, 3);

            Grid.SetRowSpan(InfoImage, 3);
            Grid.SetColumnSpan(InfoImage, 3);

            Grid.SetRowSpan(HistoryImage, 3);
            Grid.SetColumnSpan(HistoryImage, 3);


            //ControlsGrid.Children.Add(HistoryImage, 3, 11);


            //Grid.SetRowSpan(ClearImage, 2);
            //Grid.SetRowSpan(StartStopImage, 2);
            //Grid.SetRowSpan(GrandPiano, 3);
            //Grid.SetRowSpan(PowerImage, 3);
            //Grid.SetRowSpan(ToyPiano, 3);
            //Grid.SetColumnSpan(GrandPiano, 3);
            //Grid.SetColumnSpan(PowerImage, 3);
            //Grid.SetColumnSpan(ToyPiano, 3);

            //Grid.SetRowSpan(HistoryImage, 2);

            Grid.SetRowSpan(LeftControls, 8);
            Grid.SetRowSpan(RightControls, 8);

            PageContent.Children.Add(ControlsGrid, 0, 0);

            Console.WriteLine("Lets get going");

            BrainTimer = new StoppableTimer(TimeSpan.FromMilliseconds(1000), TimedUpdate, true);
            EffectAnimTimer = new StoppableTimer(TimeSpan.FromMilliseconds(effectAnimDelay), EffectUpdate, true);
            LogoAnimTimer = new StoppableTimer(TimeSpan.FromMilliseconds(effectAnimDelay), LogoUpdate, true);
            //QuoteTimer = new StoppableTimer(TimeSpan.FromMilliseconds(quoteDelay), QuoteUpdate, true);

            

            AudioTimer = new StoppableTimer(TimeSpan.FromMilliseconds(250), AudioUpdate, true);

            ScanTimer = new StoppableTimer(TimeSpan.FromMilliseconds(250), ScanUpdate, true);

            ActivityTimer = new StoppableTimer(TimeSpan.FromSeconds(30), ActivityUpdate, true);
            ActivityTimer.Start();

            WordFadeTimer = new StoppableTimer(TimeSpan.FromMilliseconds(50), WordFadeUpdate, true);
            WordFadeTimer.Start();


            AudioTimer.Start();

            Accelerometer.ReadingChanged += Accelerometer_ReadingChanged;
            Magnetometer.ReadingChanged += Magnetometer_ReadingChanged;
            Barometer.ReadingChanged += Barometer_ReadingChanged;

            Settings.AccelerometerAvailable = false;
            Settings.MagnetometerAvailable = false;
            Settings.BarometerAvailable = false;

            try
            {
                if (!Accelerometer.IsMonitoring)
                {
                    Accelerometer.Start(SensorSpeed.UI);
                    Settings.AccelerometerAvailable = true;
                }
            } catch (FeatureNotSupportedException e) { }

            try
            {
                if (!Magnetometer.IsMonitoring)
                {
                    Magnetometer.Start(SensorSpeed.UI);
                    Settings.MagnetometerAvailable = true;
                }
            } catch (FeatureNotSupportedException e) { }

            try
            {
                if (!Barometer.IsMonitoring)
                {
                    Barometer.Start(SensorSpeed.UI);
                    Settings.BarometerAvailable = true;
                }
            }
            catch (FeatureNotSupportedException e) { }

            Console.WriteLine("ACCELEROMETER AVAILABLE: " + Settings.AccelerometerAvailable);
            Console.WriteLine("BAROMETER AVAILABLE: " + Settings.BarometerAvailable);
            Console.WriteLine("MAGNOMETER AVAILABLE: " + Settings.MagnetometerAvailable);

            StopEffect();

            UpdateElements();

            InfoContent = new StackLayout
            {
                VerticalOptions = LayoutOptions.Start
            };

            // loop through info items and populate
            foreach (KeyValuePair<string, string> pair in InfoPoints)
            {
                StackLayout itemContainer = new StackLayout { VerticalOptions = LayoutOptions.Start, Orientation = StackOrientation.Horizontal, Spacing = Units.ScreenUnitM, Padding = Units.ScreenUnitM };

                if (pair.Key.Contains(".png") || pair.Key.Contains(".jpg"))
                {
                    itemContainer.Children.Add(new Image
                    {
                        Source = pair.Key,
                        WidthRequest = 32,
                        VerticalOptions = LayoutOptions.Start
                    });
                }

                if (pair.Value.Length > 0)
                {
                    itemContainer.Children.Add(new Label
                    {
                        Text = pair.Value,
                        FontSize = 12,
                        FontFamily = Fonts.GetBoldAppFont(),
                        TextColor = Color.FromHex(YellowColour),
                        Opacity = 1.0f,
                        HorizontalOptions = LayoutOptions.StartAndExpand,
                        HorizontalTextAlignment = TextAlignment.Start,
                        VerticalOptions = LayoutOptions.Start,
                        VerticalTextAlignment = TextAlignment.Start
                    });
                }

                InfoContent.Children.Add(itemContainer);


            }



            InfoScroller = new ScrollView
            {
                //Content = WordHistory,
                Content = InfoContent,
                HorizontalOptions = LayoutOptions.CenterAndExpand,
                VerticalOptions = LayoutOptions.StartAndExpand,
                VerticalScrollBarVisibility = ScrollBarVisibility.Always
            };

            WordListContent.Children.Add(InfoScroller);
            //WordHistory);

            //WordList.Children.Add(WordHistoryLabel);
            WordList.Children.Add(WordListContent);

            WordList.Children.Add(new StackLayout
            {
                Orientation = StackOrientation.Horizontal,
                Padding = new Thickness(0, 0, 0, 40),
                Children =
                {
                   SaveHistory,
                   CloseHistory
                }
            });

            WordListContainer = new StackLayout
            {
                Spacing = 0,
                Padding = 3,
                Margin = new Thickness(Units.ScreenUnitM, Units.ScreenUnitL),
                HorizontalOptions = LayoutOptions.CenterAndExpand,
                VerticalOptions = LayoutOptions.CenterAndExpand,
                BackgroundColor = Color.LightGray,
                Children =
                {
                    WordList
                }
            };


            IsAnybodyThere = false;

            if (Numbers.GetNextRandom(0, 100) > 75)
            {
                IsAnybodyThere = true;
                SetRandomPersonality();
            }
            else
            {
                SetNobodyHome();
            }

            // re-jig layout here where necessary
            if (Device.Idiom == TargetIdiom.Tablet)
            {
                HeaderGrid.TranslateTo(0, -(Units.ScreenUnitM * 2), 0, Easing.Linear);
                LeftStrip.TranslateTo(0, -(Units.ScreenUnitM * 2), 0, Easing.Linear);
                RightStrip.TranslateTo(0, -(Units.ScreenUnitM * 2), 0, Easing.Linear);
                FooterBG.TranslateTo(0, (Units.ScreenUnitM * 2), 0, Easing.Linear);
                CurrentWord.TranslateTo(0, 0, 0, Easing.Linear);
                ThinkingIndicator.TranslateTo(0, (Units.ScreenUnitM), 0, Easing.Linear);
                ThinkingIndicator.FontSize = Units.FontSizeM;
                InvestigationLabel.FontSize = Units.DynamicFontSizeXL;
                InvestigationLabel.Margin = new Thickness(0, Units.ScreenUnitS, 0, 0);
                TopLogoOff.WidthRequest = Units.ScreenWidth * 0.8;
                TopLogoOn.WidthRequest = Units.ScreenWidth * 0.8;

                InfoImage.WidthRequest = Units.ScreenUnitM;
                PlusImage.WidthRequest = Units.ScreenUnitM;
                MinusImage.WidthRequest = Units.ScreenUnitM;
                SoundButton.WidthRequest = Units.ScreenUnitM;
                VibrationButton.WidthRequest = Units.ScreenUnitM;
                SpeechButton.WidthRequest = Units.ScreenUnitM;

                StartStopImage.WidthRequest = Units.ScreenUnitL;
                HistoryImage.WidthRequest = Units.ScreenUnitM * 1.5;
                ClearImage.WidthRequest = Units.ScreenUnitM * 1.5;
                PowerImage.WidthRequest = Units.ScreenUnitL;
                WordMemory = 5;
            }
            else
            {
                ThinkingIndicator.FontSize = Units.FontSizeM;
                InvestigationLabel.FontSize = Units.DynamicFontSizeXXL;
                InvestigationLabel.Margin = new Thickness(0, Units.ScreenUnitXS, 0, 0);
                CurrentWord.FontSize = Units.DynamicFontSizeXXXL;

                //CurrentWord.Margin = new Thickness(0, Units.ScreenUnitS, 0, 0);
                CurrentWord.TranslateTo(0, 0, 0, Easing.Linear);
                ThinkingIndicator.TranslateTo(0, -150, 0, Easing.Linear);

                PianoSelect.TranslateTo(0, Units.ScreenUnitL * 6.5, 0, Easing.Linear);
                ToySelect.TranslateTo(0, Units.ScreenUnitL * 6.5, 0, Easing.Linear);
                ToneSelect.TranslateTo(0, Units.ScreenUnitL * 6.5, 0, Easing.Linear);

                if (Units.ShortScreen)
                {
                    Tune1.FontSize = 9;
                    Tune2.FontSize = 9;
                    Tune3.FontSize = 9;
                    Tune4.FontSize = 9;
                    Tune5.FontSize = 9;
                    Tune6.FontSize = 9;

                    Phrase1.FontSize = 9;
                    Phrase2.FontSize = 9;
                    Phrase3.FontSize = 9;
                    Phrase4.FontSize = 9;

                    ThinkingIndicator.FontSize = 9;

                    LeftControls.TranslateTo(0, Units.ScreenUnitL * 2, 0, Easing.Linear);
                    RightControls.TranslateTo(0, Units.ScreenUnitL * 2, 0, Easing.Linear);
                    
                }

                WordMemory = 6;
            }

            CurrentWord.FontSize = Units.DynamicFontSizeXXXL;

            SetMood(1);
            this.Content = PageContent;
        }

        public void SetMood(int mood)
        {
            Mood = mood;
            UserInteraction++;
            switch (Mood)
            {
                case 0:
                    //Phrase1.Opacity = 1.0f;
                    //Phrase2.Opacity = OffTextOpacity;
                    //Phrase3.Opacity = OffTextOpacity;
                    //Phrase4.Opacity = OffTextOpacity;

                    Phrase1.TextColor = Color.FromHex(YellowColour);
                    Phrase2.TextColor = Color.White;
                    Phrase3.TextColor = Color.White;
                    Phrase4.TextColor = Color.White;

                    break;
                case 1:
                    //Phrase1.Opacity = OffTextOpacity;
                    //Phrase2.Opacity = 1.0f;
                    //Phrase3.Opacity = OffTextOpacity;
                    //Phrase4.Opacity = OffTextOpacity;

                    Phrase1.TextColor = Color.White;
                    Phrase2.TextColor = Color.FromHex(YellowColour);
                    Phrase3.TextColor = Color.White;
                    Phrase4.TextColor = Color.White;

                    break;
                case 2:
                    //Phrase1.Opacity = OffTextOpacity;
                    //Phrase2.Opacity = OffTextOpacity;
                    //Phrase3.Opacity = 1.0f;
                    //Phrase4.Opacity = OffTextOpacity;

                    Phrase1.TextColor = Color.White;
                    Phrase2.TextColor = Color.White;
                    Phrase3.TextColor = Color.FromHex(YellowColour);
                    Phrase4.TextColor = Color.White;

                    break;
                default:
                    //Phrase1.Opacity = OffTextOpacity;
                    //Phrase2.Opacity = OffTextOpacity;
                    //Phrase3.Opacity = OffTextOpacity;
                    //Phrase4.Opacity = 1.0f;

                    Phrase1.TextColor = Color.White;
                    Phrase2.TextColor = Color.White;
                    Phrase3.TextColor = Color.White;
                    Phrase4.TextColor = Color.FromHex(YellowColour);

                    break;


            }
        }

        public void ShowHistory()
        {
            ShowInfoBox = true;
            IsPaused = true;
            InfoType = WORD_HISTORY;
            UpdateElements();
        }

        public void HideHistory()
        {
            ShowInfoBox = false;
            IsPaused = false;
            InfoType = WORD_HISTORY;
            UpdateElements();
        }

        public void ShowGeneralInfo()
        {
            ShowInfoBox = true;
            IsPaused = true;
            InfoType = GENERAL_INFO;
            UpdateElements();
        }

        public void HideGeneralInfo()
        {
            ShowInfoBox = false;
            IsPaused = false;
            InfoType = GENERAL_INFO;
            UpdateElements();

        }

        public void SaveHistoryList()
        {
            CreateLog();
            DisplayAlert("Session Logged", "Your session has been saved to your device's clipboard. You can now paste this into an email, message, document or wherever you choose. Simply open up the app you'd like to use, then press and hold in a text area to bring up the option to paste your session.", "Ok");
            HideHistory();
        }

        public void TriggerPhrase(int phraseId, bool spiritTriggered)
        {
            IsSpeaking = true;
            playSignatureMelody = true;
            SpiritTriggeredMelody = spiritTriggered;
            if (Numbers.GetNextRandom(0, 100) > 50)
            {
                WordsToSpeak = PhraseManager.GetSessionPhrase(phraseId);
            }
            else
            {
                WordsToSpeak = PhraseManager.ReverseArrayToArray(PhraseManager.GetSessionPhrase(phraseId));
            }

            if (Numbers.GetNextRandom(0, 100) > 75)
            {
                WordsToSpeak = PhraseManager.GetSimpleSentence().Split(' ');

                if (Numbers.GetNextRandom(0, 100) > 75)
                {
                    WordsToSpeak = PhraseManager.GetHint();
                }
            }

            //WordsToSpeak = PhraseManager.GetHint();
            MelodyCount = 0;
        }
        
        public void TriggerMelody(int melodyId, int instrument, bool spiritTriggered)
        {
            LastPlayedMelody = melodyId;
            SpiritTriggeredMelody = spiritTriggered;
            IsSpeaking = false;
            playSignatureMelody = true;
            selectedInstrument = instrument;
            LastInstrumentPlayed = instrument;
            SignatureMelody = AudioManager.GetSessionMelody(melodyId, Mood);

            if (melodyId == 5)
            {
                SignatureMelody = AudioManager.GetLocationMelody();
            }

            if (melodyId == 6)
            {
                if (Mood == 0)
                {
                    SignatureMelody = AudioManager.GetMajorTune();
                }
                else if(Mood == 1)
                {
                    SignatureMelody = AudioManager.GetMinorTune();
                }
            }

            UserInteraction++;
            UserTriggeredMelodyCount++;

            if (UserTriggeredMelodyCount > UserTriggeredMelodyLimit * 4) // reset the limits, to give a natural pause in melody playback
            {
                UserTriggeredMelodyCount = 0;
                UserTriggeredMelodyLimit = Numbers.GetNextRandom(2, 8);
            }

            MelodyCount = 0;

            if (spiritTriggered) // easter eggs
            {
                //selectedInstrument = Numbers.GetNextRandom(0, 3);
                if (Numbers.GetNextRandom(0, 1000) > 950)
                {
                    EntityManager.IncreaseStimulus((int)EntityManager.StimulusTypes.Pressure, 20);
                    
                    if (Numbers.GetNextRandom(0, 100) > 75)
                    {
                        SignatureMelody = AudioManager.GetMajorTune();
                    }
                    else
                    {
                        SignatureMelody = AudioManager.GetMinorTune();
                    }
                }

                if (Numbers.GetNextRandom(0, 100) > 75)
                {
                    EntityManager.IncreaseStimulus((int)EntityManager.StimulusTypes.Random, 50);
                    selectedInstrument = (int)SoundSources.Noise;
                    SignatureMelody = AudioManager.GetSpooky();
                }

            }
            else
            {
                if (Numbers.GetNextRandom(0, 100) > 90)
                {
                    EntityManager.IncreaseStimulus((int)EntityManager.StimulusTypes.Random, 2);
                }
            }

            if (selectedInstrument == 3 && !SpiritTriggeredMelody)
            {
                selectedInstrument = Numbers.GetNextRandom(0, 2);
            }

            /*
            if (selectedInstrument == (int)SoundSources.ToyPiano)
            {
                PianoSelect.Opacity = OffTextOpacity;
                ToySelect.Opacity = 1.0f;
                ToneSelect.Opacity = OffTextOpacity;
            }
            else if (selectedInstrument == (int)SoundSources.Piano)
            {
                PianoSelect.Opacity = 1.0f;
                ToySelect.Opacity = OffTextOpacity;
                ToneSelect.Opacity = OffTextOpacity;
            }
            else if (selectedInstrument == (int)SoundSources.Tone)
            {
                PianoSelect.Opacity = OffTextOpacity;
                ToySelect.Opacity = OffTextOpacity;
                ToneSelect.Opacity = 1.0f;
            }
            else
            {
                PianoSelect.Opacity = OffTextOpacity;
                ToySelect.Opacity = OffTextOpacity;
                ToneSelect.Opacity = OffTextOpacity;
            }
            */
        }

        public void SetMode(int mode)
        {
            EVENT_SPEED_FAST = mode;

            EVENT_SPEED_MEDIUM_FAST = EVENT_SPEED_FAST * 2;
            EVENT_SPEED_MEDIUM = EVENT_SPEED_FAST * 3;
            EVENT_SPEED_MEDIUM_SLOW = EVENT_SPEED_FAST * 4;
            EVENT_SPEED_SLOW = EVENT_SPEED_FAST * 5;
            EVENT_SPEED_VERY_SLOW = EVENT_SPEED_FAST * 6;
        }

        public void SetState(int state)
        {
            CurrentEvent = (int)state;
            Console.WriteLine("STATE: " + DarkaudiousEventNames[CurrentEvent]);
        }

        public void SkipIntro()
        {

        }

        public void SwitchOn()
        {
            //Speak("On", false);
            //SpeakAudioOnly("On");
            PlaySound("click");

            startDelay = Numbers.GetNextRandom(10000, 30000);
            StartTime = DateTime.Now;
            CurrentTime = DateTime.Now;
            PowerUpTimeCheck = DateTime.Now;
            NextEventTime = StartTime.AddMilliseconds(startDelay);

            Console.WriteLine("StartTime: " + StartTime.ToLongTimeString());

            BrainTimer.Start();
            ScanTimer.Start();
            LogoAnimTimer.Start();
            PerformingEvent = false;
            SetState((int)DarkaudiousEvents.DarkaudiousStartUp);
            Blackout.Opacity = 0;
            Whiteout.Opacity = 0.125;
            Mask.Opacity = 0.6;
            UpdateElements();
            Restart();
        }


        public void SwitchOff()
        {
            //Speak("Off", false);
            CurrentWord.Text = "";

            if (LoggedWordsList != null)
            {
                LoggedWordsList.Clear();
            }
            //MoodsAvailable = 2;
            //TunesAvailable = 1;

            SwitchOffLights();
            PerformingEvent = false;
            BrainTimer.Stop();
            ScanTimer.Stop();
            LogoAnimTimer.Stop();
            Initialised = false;
            UpdateElements();
        }

        public void Pause()
        {
            InvestigationLabel.Text = "INVESTIGATION PAUSED";
            StartStopImage.Source = "start2off.png";
        }

        public void UnPause()
        {
            InvestigationLabel.Text = "INVESTIGATION ACTIVE";
            StartStopImage.Source = "start2.png";
        }


        public void SwitchOnLights()
        {
            //LightStatus = LIGHTS_ON;
            UpdateLights();
        }

        public void SwitchOffLights()
        {
            //LightStatus = LIGHTS_OFF;
            UpdateLights();
        }

        public float GetRandomEffectOpacity()
        {
            var randEffectOpacity = new Random().NextDouble() * 1;

            if (randEffectOpacity > 0.4)
            {
                randEffectOpacity = 0.4;
            }
            else
            {
                randEffectOpacity = 0.2;
            }
            return (float)randEffectOpacity;
        }


        int Sensitivity = 10;
        int SensitivitySkip = 0;

        public void ActivityUpdate()
        {
            if (IsPaused) return;
            if (!DeadboxOn) return;

            Console.WriteLine("ACT: " + EntityManager.CurrentActivityLevel + "(" + EntityManager.GetCurrentActvityLevelName() + ")");

            FlurryTriggered = false;

            if (Numbers.GetNextRandom(0, 100) > 75)
            {
                Console.WriteLine("Flurry Triggered");
                FlurryTriggered = true;
            }

            if (EntityManager.GetCurrentActvityLevel() > 3)
            {
                TriggerRapidEvents();
            }

            if (Numbers.GetNextRandom(0, 100) > 75)
            {
                TriggerPhrase(Numbers.GetNextRandom(1, 4), true);
            }

            EntityManager.ResetStimuli();

            Sensitivity = 10; // highest - default
            if (Numbers.GetNextRandom(0, 100) > 50) // reduce
            {
                Sensitivity = 1;// Numbers.GetNextRandom(0, 5);
            }

            SensitivitySkip = 10 - Sensitivity;

            if(SensitivitySkip < 0)
            {
                SensitivitySkip = 0;
            }
        }


        public void TimedUpdate()
        {
            if (IsPaused) return;
            if (!DeadboxOn) return;


            UpdateElements();

            Tick();
        }



        public void ChangeLocation()
        {
            if (Numbers.GetNextRandom(0, 100) > 75)
            {
                SetRandomPersonality();
            }
            if (Numbers.GetNextRandom(0, 100) > 75)
            {
                SetRandomScanSensitivity();
            }
            if (Numbers.GetNextRandom(0, 100) > 75)
            {

                RandomiseMood();

                if (Numbers.GetNextRandom(0, 100) > 50)
                {
                    RandomisePower();
                }
            }
        }

        public void SetRandomPersonality()
        {
            // passive / aggressive / shy

            // victim / perp
            bool victim = false;

            if (Numbers.GetNextRandom(0, 100) > 10)
            {
                victim = true;
            }

            // male / female
            bool male = false;

            if (Numbers.GetNextRandom(0, 100) > 50)
            {
                male = true;
            }

            // IQ 
            int iq = Numbers.GetNextRandom(75, 120);

            // age
            int age = Numbers.GetNextRandom(6, 90);

            // vocabulary
            int vocab = Numbers.GetNextRandom(5, 16);

            if (iq < 80)
            {
                vocab = 4;
            }

            if (iq > 100)
            {
                vocab = 16;
            }

            if (age < 8)
            {
                vocab = 4;
            }

            if (age > 80)
            {
                vocab = 5;
            }



            PhraseManager.SetMaxWordLength(vocab);

            // likes movement
            LikesMovement = true;
            if (Numbers.GetNextRandom(0, 100) > 50)
            {
                LikesMovement = false;
            }


            string isMale = (male == true) ? "Male" : "Female";

            CurrentMood = (Numbers.GetNextRandom(0, 9));
            CurrentStrength = (Numbers.GetNextRandom(0, 9));


            if (CurrentStrength < 4)
            {
                if (CurrentMood < 4)
                {
                    ScanFrequency = Numbers.GetNextRandom(8, 10);
                }
                else
                {
                    ScanFrequency = Numbers.GetNextRandom(5, 9);
                }
            }

            /*
            LikesMovement = false;
            if (Numbers.GetNextRandom(0, 100) > 50)
            {
                LikesMovement = true;
            }*/


            Console.WriteLine("GENDER: " + isMale);
            Console.WriteLine("VICTIM: " + victim);
            Console.WriteLine("AGE: " + age);
            Console.WriteLine("LIKES MOVEMENT: " + LikesMovement);
            Console.WriteLine("IQ: " + iq);
            Console.WriteLine("VOCABULARY: " + vocab);
            Console.WriteLine("PERSONALITY SET");

            if (Numbers.GetNextRandom(0, 100) > 25)
            {
                SetNobodyHome();
            }



            UpdateElements();
        }


        public void SetNobodyHome()
        {
            IsAnybodyThere = false;
            CurrentMood = 5;
            CurrentStrength = 1;
            LikesMovement = false;

            if (Numbers.GetNextRandom(0, 100) > 50)
            {
                LikesMovement = true;
            }
        }

        public void SetPersonality()
        {
            SetRandomPersonality();

            /*
            // passive / aggressive / shy

            // victim / perp

            // male / female

            // age


            // vocabulary
            PhraseManager.SetMaxWordLength(4);

            // likes movement
            LikesMovement = true;
            */


        }

        public void SetRandomScanSensitivity()
        {
            ScanSensitivity = Numbers.GetNextRandom(2, 10) / 10;
        }

        public void SetScanSensitivity(float frequency)
        {
            ScanSensitivity = frequency;
        }

        public void PowerUp()
        {
            if (Numbers.GetNextRandom(0, 100) > 50)
            {
                if (CurrentStrength < 9)
                {
                    CurrentStrength++;
                }
            }

            if (Numbers.GetNextRandom(0, 100) > 50)
            {
                MoodUp();
            }
        }

        public void PowerDown()
        {
            if (Numbers.GetNextRandom(0, 100) > 50)
            {
                if (CurrentStrength > 0)
                {
                    CurrentStrength--;
                }
            }

            if (Numbers.GetNextRandom(0, 100) > 50)
            {
                MoodDown();
            }
        }

        public void RandomisePower()
        {
            CurrentStrength = Numbers.GetNextRandom(0, 9);
        }

        public void MoodUp()
        {
            if (CurrentMood < 9)
            {
                CurrentMood++;
            }
        }


        public void MoodDown()
        {
            if (CurrentMood > 0)
            {
                CurrentMood--;
            }
        }

        public void RandomiseMood()
        {
            CurrentMood = Numbers.GetNextRandom(0, 9);
        }


        public void SpeedUpScan(bool auto)
        {
            if (ScanFrequency > 1.0f)
            {
                if (!auto)
                {
                    PlaySound("click");
                }
                ScanFrequency -= 1.0f;

                if (ScanFrequency < 1.0f)
                {
                    ScanFrequency = 1.0f;
                }
                //ScanSpeedLabel.Text = "SCAN RATE:" + (MaxFrequency - (ScanFrequency));
            }
        }

        public void SlowDownScan(bool auto)
        {
            if (ScanFrequency >= 9.0f) { return; }

            if (ScanFrequency < MaxFrequency)
            {
                if (!auto)
                {
                    PlaySound("click");
                }
                ScanFrequency += 1.0f;
                ScanSpeedLabel.Text = "SCAN RATE:" + (MaxFrequency - (ScanFrequency));
            }
        }


        static int AUDIO_SPEECH = 0;
        static int AUDIO_INSTRUMENT = 1;

        int InstrumentType = 0; // 0 piano , 1 = toypiano
        int NoteCount = 0;
        int NotesInSequence = 3;
        bool PlaySequence = false;
        bool RandomiseSequences = true;
        int AudioType = AUDIO_SPEECH; // voice


        int UpdateFrequency = 15;
        int UpdateSkip = 0;
        //int rapidActivity = 0;
        int rapidActivityLength = 4;
        int rapidActivityType = 0;
        int rapidNoteLength = 1;
        bool playSignatureMelody = false;

        public void TriggerRapidEvents()
        {
            rapidActivityType = Numbers.GetNextRandom(0, 4);
            rapidActivityLength = Numbers.GetNextRandom(2, 12);

            // TEST
            //TriggerMelody(LastPlayedMelody, (int)SoundSources.Noise, true);
        }


        public void AudioUpdate()
        {
            if (IsPaused) return;
            if (!DeadboxOn) return;

            bool alwaysPlay = false;
            if (!SpiritTriggeredMelody && rapidActivityType <= 2)
            {
                alwaysPlay = true;
            }

            if (rapidActivityLength == 0)
            {
                if (Numbers.GetNextRandom(1, 100) > 15) // nearly always skip if no rapid activity
                {
                    if (!alwaysPlay)
                    {
                        return;
                    }
                }
            }

            if (SensitivitySkip > 0)
            {
                SensitivitySkip--;
                Console.WriteLine("Sensitivity SKIP");
                if (!alwaysPlay)
                {
                    return;
                }
            }

            if (playSignatureMelody)
            {
                UpdateMelody();
                return;
            }

            if (EntityManager.GetCurrentActvityLevel() < (int)EntityManager.ActivityLevels.MediumActivity)
            {
                if (!alwaysPlay)
                {
                    return;
                }
            }

            if (rapidActivityLength == 0) // normal
            {
                if (UpdateSkip < UpdateFrequency)
                {
                    UpdateSkip++;

                    Console.WriteLine("Update Skip " + UpdateSkip);
                    if (!alwaysPlay)
                    {
                        return;
                    }
                }
            }
            else
            {
                // rapid event note
                try
                {
                    if (rapidActivityType == 4) // speech
                    {
                        rapidActivityLength = Numbers.GetNextRandom(1, 3);
                        SpeakSimple(PhraseManager.GetMuttering());
                    }
                    else
                    {
                        selectedInstrument = rapidActivityType;

                        AudioManager.SetInstrument(selectedInstrument);
                        AudioManager.SetSoundSource(selectedInstrument);
                        StartEffect();

                        if (rapidActivityType <= 2)
                        {
                            if (UserTriggeredMelodyCount > UserTriggeredMelodyLimit) // don't allow any spirit melody activity until the user has played a few melodies
                            {
                                Console.WriteLine("Rapid: " + rapidActivityLength);
                                AudioManager.PlayNote(rapidNoteLength, NotesToPlay[MelodyCount], true);
                            }
                            else
                            {
                                Console.WriteLine(UserTriggeredMelodyCount + " melody triggers is not enough user action to prompt a spirit melody");
                                return;
                            }
                        }
                        else
                        {
                            return;
                        }
                    }

                }
                catch(Exception e)
                {
                    Console.WriteLine("Note not available");
                }
            }


            int freq = 90;

            if (rapidActivityLength > 0)
            {
                //freq = 20;
                rapidActivityLength--;
            }

            if (AudioManager.IsPlaying())
            {
                return;
            }

            if (Numbers.GetNextRandom(100) > 90)
            {
                int tuneChosen = LastPlayedMelody;

                if (Numbers.GetNextRandom(0, 100) > 50)
                {
                    if (TunesAvailable == 1)
                    {
                        tuneChosen = 0;
                    }
                    else
                    {
                        tuneChosen = Numbers.GetNextRandom(0, TunesAvailable-1);
                    }
                }

                if (Numbers.GetNextRandom(0, 100) > 50)
                {
                    if (MoodsAvailable == 1)
                    {
                        Mood = 0;
                    }
                    else
                    {
                        Mood = Numbers.GetNextRandom(0, MoodsAvailable - 1);
                    }
                   // Mood = Numbers.GetNextRandom(0, MoodsAvailable);
                }

                TriggerMelody(tuneChosen, LastInstrumentPlayed, true);

                if (tuneChosen < TunesPlayed.Length)
                {
                    try
                    {
                        TunesPlayed[Mood, tuneChosen]++;
                    }
                    catch (Exception e) { }
                }

                Console.WriteLine("TUNES PLAYED");


                Console.WriteLine("MAJOR TUNE 1 PLAYED " + TunesPlayed[0, 0] + " TIMES");
                Console.WriteLine("MAJOR TUNE 2 PLAYED " + TunesPlayed[0, 1] + " TIMES");
                Console.WriteLine("MAJOR TUNE 3 PLAYED " + TunesPlayed[0, 2] + " TIMES");
                Console.WriteLine("MAJOR TUNE 4 PLAYED " + TunesPlayed[0, 3] + " TIMES");

                Console.WriteLine("MINOR TUNE 1 PLAYED " + TunesPlayed[1, 0] + " TIMES");
                Console.WriteLine("MINOR TUNE 2 PLAYED " + TunesPlayed[1, 1] + " TIMES");
                Console.WriteLine("MINOR TUNE 3 PLAYED " + TunesPlayed[1, 2] + " TIMES");
                Console.WriteLine("MINOR TUNE 4 PLAYED " + TunesPlayed[1, 3] + " TIMES");

                Console.WriteLine("RANDOM TUNE 1 PLAYED " + TunesPlayed[2, 0] + " TIMES");
                Console.WriteLine("RANDOM TUNE 2 PLAYED " + TunesPlayed[2, 1] + " TIMES");
                Console.WriteLine("RANDOM TUNE 3 PLAYED " + TunesPlayed[2, 2] + " TIMES");
                Console.WriteLine("RANDOM TUNE 4 PLAYED " + TunesPlayed[2, 3] + " TIMES");

                Console.WriteLine("SESSION TUNE 1 PLAYED " + TunesPlayed[3, 0] + " TIMES");
                Console.WriteLine("SESSION TUNE 2 PLAYED " + TunesPlayed[3, 1] + " TIMES");
                Console.WriteLine("SESSION TUNE 3 PLAYED " + TunesPlayed[3, 2] + " TIMES");
                Console.WriteLine("SESSION TUNE 4 PLAYED " + TunesPlayed[3, 3] + " TIMES");

               
                return;
            }


            if (Numbers.GetNextRandom(100) > freq)
            {
                if (AudioType == AUDIO_SPEECH)
                {
                    AudioType = AUDIO_INSTRUMENT;
                }
                else
                {
                    if (Numbers.GetNextRandom(100) > 98)
                    {
                        AudioType = AUDIO_SPEECH;
                    }
                }
            }

            UpdateSkip = 0;


            bool speakOnly = false;

            if (!speakOnly)
            {
                if (AudioType == AUDIO_SPEECH)
                {
                    SpeechUpdate(5);
                }
                else
                {
                    InstrumentUpdate();
                }
            }
            else
            {
                SpeechUpdate(5);
            }
            

        }

        public void SpeechUpdate(int hitRate)
        {
            if (IsPaused)
            {
                return;
            }

            if (!DeadboxOn)
            {
                return;
            }

            int rate = hitRate;
            if (rate > 95) { rate = 95; }

            if (Numbers.GetNextRandom(100) > (100 - rate))
            {
                StartEffect();
                SpeakSimple(PhraseManager.GetMuttering());
            }
        }

        public void UpdateMelody()
        {
            if (SignatureMelody == null)
            {
                return;
            }

            Console.WriteLine("Update melody");

            if (SpiritTriggeredMelody)
            {
                if (MelodyCount >= EntityManager.Memory)
                {
                    playSignatureMelody = false;
                    SpiritTriggeredMelody = false;
                    return;
                }
            }


            //selectedInstrument = (int)SoundSources.Piano;
            AudioManager.SetInstrument(selectedInstrument);
            AudioManager.SetSoundSource(selectedInstrument);
            
            NotesToPlay = SignatureMelody;
            

            if (!AudioManager.IsPlaying())
            {
                Console.WriteLine("Update melody " + MelodyCount + " of " + SignatureMelody.Length);
                int length = SignatureMelody.Length;

                if (IsSpeaking)
                {
                    length = WordsToSpeak.Length;

                    if (SpiritTriggeredMelody)
                    {
                        if (length > EntityManager.Memory)
                        {
                            length = EntityManager.Memory;
                        }
                    }
                }

                if (MelodyCount < length)
                {
                    //AudioManager.PlayNote(NoteLength, NotesToPlay[MelodyCount], false);
                    //AudioManager.PlayMelodyNote(NotesToPlay[MelodyCount]);
                    //SpeakSimple(PhraseManager.GetMuttering());

                    if (IsSpeaking)
                    {
                        SpeakSimple(WordsToSpeak[MelodyCount]);
                    }
                    else
                    {
                        AudioManager.PlayMelodyNote(NotesToPlay[MelodyCount]);
                    }
                    MelodyCount++;
                }
                else
                {
                    playSignatureMelody = false;
                    MelodyCount = 0;
                }
            }
        }
        
        public void InstrumentUpdate()
        {
            if (IsPaused) return;
            if (!DeadboxOn) return;

            if (playSignatureMelody)
            {
                return;
            }

            NotesToPlay = MinorTriad;

            // set up - is it time to play a melody?
            if (!PlayMelody)
            {
                int rand = (int)Numbers.GetNextRandom(1000);
                if (rand < 50)//MelodyFrequency)
                {
                    StartEffect();
                    Console.WriteLine("PLAY MELODY ");
                    MelodyCount = 0;
                    PlayMelody = true;
                    AudioManager.SetInstrument(selectedInstrument);
                    AudioManager.SetSoundSource(selectedInstrument);
                }
                else
                {
                    Console.WriteLine("SKIP EVENT");
                }
            }



            if (PlayMelody) // play melody in full, once
            {
                if (MelodyCount < NotesToPlay.Length - 1)
                {
                    AudioManager.PlayNote(NoteLength, NotesToPlay[MelodyCount], false);
                    MelodyCount++;
                }
                else
                {
                    MelodyCount = 0;
                    PlayMelody = false;
                }
            }
            else
            {

            }
        }

        bool FlurryTriggered = false;

        public void SetSimulatedSensors()
        {
            
            int triggerThreshold = Numbers.GetNextRandom(95, 99);

            if (FlurryTriggered)
            {
                triggerThreshold = Numbers.GetNextRandom(75, 99);
            }

            if (!Settings.MagnetometerAvailable)
            {
                if (Numbers.GetNextRandom(100) > triggerThreshold)
                {
                    MagnoX = Numbers.GetNextRandom(0, 10);
                    EntityManager.IncreaseStimulus((int)EntityManager.StimulusTypes.Magnetic, 1);
                }
                if (Numbers.GetNextRandom(100) > triggerThreshold)
                {
                    MagnoY = Numbers.GetNextRandom(0, 10);
                    EntityManager.IncreaseStimulus((int)EntityManager.StimulusTypes.Magnetic, 1);
                }
                if (Numbers.GetNextRandom(100) > triggerThreshold)
                {
                    MagnoZ = Numbers.GetNextRandom(0, 10);
                    EntityManager.IncreaseStimulus((int)EntityManager.StimulusTypes.Magnetic, 1);
                }
            }

            if (!Settings.BarometerAvailable)
            {
                if (Numbers.GetNextRandom(100) > triggerThreshold)
                {
                    BaroPressure = Numbers.GetNextRandom(980, 1080);
                    EntityManager.IncreaseStimulus((int)EntityManager.StimulusTypes.Pressure, 1);
                }

                if (BaroPressure > 1060)
                {
                    // TRIGGER EVENT
                    Console.WriteLine("TRIGGER PRESSURE EVENT");
                    //TriggerRapidEvents();
                    if (Numbers.GetNextRandom(0, 100) > 75)
                    {
                        SpeakSimple(PhraseManager.GetItem());
                    }
                    EntityManager.IncreaseStimulus((int)EntityManager.StimulusTypes.Pressure, 5);
                    BaroPressure = 980;

                }
            }
        }

        public void ScanUpdate()
        {
            if (IsPaused) return;
            if (!DeadboxOn) return;

            SetSimulatedSensors();

            SpiritMood = Moods[CurrentMood];
            SpiritStrength = Strengths[CurrentStrength];

            string spiritState = "SETTLED";

            if (LikesMovement)
            {
                spiritState = "RESTLESS";
            }

            if (ScanFrequency < 1.0f) { ScanFrequency = 1.0f; }

            string scanFrequencyLabel = "" + (MaxFrequency - ScanFrequency);


            ThinkingIndicator.Text = "STATE: " + spiritState  + "\n" + "MOOD: " + SpiritMood + "\n" + "STRENGTH: " + SpiritStrength + "\n" + "SCAN RATE: " + scanFrequencyLabel + "\n";

            ThinkingIndicator.Text = "AMBIENT STIMULUS: " + EntityManager.GetCurrentActvityLevelName();


            ThinkingIndicator.Text += "\n\nSPIRIT ACTIVITY: ";

            if (rapidActivityLength > 0)
            {
                ThinkingIndicator.Text += "PRESENT\n\n";
            }
            else
            {
                ThinkingIndicator.Text += "SEEKING\n\n";
            }

            //ThinkingIndicator.Text += "MONITORING\nMOVEMENT\n\n" + MoveX + ", " + MoveY + ", " + MoveZ + "\n\nSIMULATING\nPRESSURE\n\n" + BaroPressure + "\n" + "\nSIMULATING\nMAGNETIC FIELD\n\n" + MagnoX + ", " + MagnoY + ", " + MagnoZ + "\n";


            //ThinkingIndicator.Text += "MOVEMENT: "+ MoveX + ", " + MoveY + ", "+ MoveZ +"\n\nPRESSURE: " + BaroPressure + "\n" + "\nMAGNETIC FIELD: " + MagnoX + ", " + MagnoY + ", " + MagnoZ + "\n";

            ThinkingIndicator.Text += "MOVEMENT: " + MoveX + ", " + MoveY + ", " + MoveZ + "\n\nPRESSURE: " + BaroPressure + " (sim)\n" + "\nMAGNETIC FIELD: " + MagnoX + ", " + MagnoY + ", " + MagnoZ + " (sim)\n";



            //ThinkingIndicator.Text = SpiritStrength + ", " + spiritState + " &\n" + SpiritMood + "\n" + "SCAN RATE: " + ScanFrequency + "\n";


            if (thinkIndicatorCount < 12) // ScanFrequency)
            {
                thinkIndicatorCount++;
            }
            else
            {
                thinkIndicatorCount = 0;
            }

            for (int i = 0; i < thinkIndicatorCount; i++)
            {
                ThinkingIndicator.Text += "";
            }
            ThinkingIndicator.Text += "\n\n";

            

            scanSkipCount++;

            if (scanSkipCount >= (ScanFrequency+1))
            {
                scanSkipCount = 0;
            }

            int limit = (int)((ScanFrequency+1) / 2);

            if (limit < 1) { limit = 1; }

            if (scanSkipCount >= limit)
            {
                showThinking = false;
            }
            else
            {
                ThinkingIndicator.Text += "SCANNING AUDIUS";
            }

            if (showThinking)
            {
                //ThinkingIndicator.Opacity = 0.8;
                //ThinkingIndicator.Text = ".";
                //ThinkingIndicator.Text += "\n" + ScanFrequency;
            }
        }

        int LogoFadeState = 0;
        float LogoFadeRate = 0.25f;

        bool scaleUp = true;
        float maxScale = 1.01f;
        float currentScale = 1.0f;

        public void LogoUpdate()
        {
            if (IsPaused) return;
            if (!DeadboxOn) return;

            LogoFadeRate = (float)(1 / (ScanFrequency*3f));
            TopLogoOff.Opacity = 1.0f;

            if (LogoFadeState == 0)
            {
                if (TopLogoOn.Opacity < 1.0f)
                {
                    TopLogoOn.Opacity += LogoFadeRate;
                }
                else
                {
                    LogoFadeState = 1;
                }
            }
            else
            {
                if (TopLogoOn.Opacity > 0.0f)
                {
                    TopLogoOn.Opacity -= LogoFadeRate;
                }
                else
                {
                    LogoFadeState = 0;
                    TopLogoOn.Source = "damain_on.png";
                    if (Numbers.GetNextRandom(0, 100) > 85)
                    {
                        TopLogoOn.Source = "damain_on.png";
                    }
                    if (Numbers.GetNextRandom(0, 100) > 50)
                    {
                        TopLogoOn.Source = "damain_off.png";
                    }
                }
            }

            if (scaleUp)
            {
                if (currentScale < maxScale)
                {
                    currentScale += 0.00025f;
                    Circuit.Scale = currentScale;
                }
                else
                {
                    scaleUp = false;
                }
            }
            else
            {
                if (currentScale > 1.0f)
                {
                    currentScale -= 0.00025f;
                    Circuit.Scale = currentScale;
                }
                else
                {
                    scaleUp = true;
                    StopEffect();
                }
            }

        }


        public void EffectUpdate()
        {
            if (IsPaused) return;
            if (!DeadboxOn) return;

            if (EffectFade == EFFECT_FADE_IN)
            {
                if (Circuit.Opacity < EffectMaxOpacity)
                {
                    Circuit.Opacity += 0.05;
                }
                else
                {
                    /*EffectMaxOpacity = (float)GetRandomEffectOpacity();
                    if (GetNextRandom(10) < 5)
                    {
                        EffectFade = EFFECT_FADE_OUT;
                    }
                    */
                    EffectFade = EFFECT_FADE_OUT;
                }
            }
            else
            {
                if (Circuit.Opacity > 0)
                {
                    //Circuit.Opacity -= 0.005;
                    Circuit.Opacity -= 0.05;
                }
                else
                {
                    EffectFade = EFFECT_FADE_IN;

                    /*
                    if (GetNextRandom(10) < 8)
                    {
                        StopEffect();
                        return;
                    }*/
                }
            }



            string CurrentEffectName = "gl";
            int frames = 21;



            if (CurrentEffectFrame < frames)
            {
                CurrentEffectFrame++;
            }
            else
            {
                CurrentEffectFrame = 1;

            }
            

            CircuitAnimImage.Source = CurrentEffectName + CurrentEffectFrame + ".png";

            

        }

        public void StartEffect()
        {
            if (!EffectStarted)
            {
                Console.WriteLine("EFFECT STARTED");
                EffectMaxOpacity = (float)GetRandomEffectOpacity();
                EffectAnimTimer.Start();
                EffectStarted = true;
                CircuitAnimImage.Opacity = (float)GetRandomEffectOpacity();

                if(Numbers.GetNextRandom(0, 100) > 50)
                {
                    CircuitAnimImage.RotateTo(Numbers.GetNextRandom(0, 360), 0, Easing.Linear);
                    CircuitAnimImage.Scale = Numbers.GetNextRandom(2, 3);
                }
                else
                {
                    CircuitAnimImage.RotateTo(0, 0, Easing.Linear);
                    CircuitAnimImage.Scale = 1.0f;
                }
            }
        }

        public void StopEffect()
        {
            EffectAnimTimer.Stop();
            EffectStarted = false;
            CurrentEffectFrame = 1;
            CircuitAnimImage.Opacity = 0;
            Circuit.Opacity = 0.2;
        }

        public void Restart()
        {
            // BACK TO THE BEGINNING
            Console.WriteLine("BACK TO THE BEGINNING");
            SetLocationSessionData();

            


            SetState((int)DarkaudiousEvents.DarkaudiousBegin);
        }

        string wordsSpoken = "";

        private static Random random = new Random();

        public static string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        int SeekingSkip = 10;
        int SeekingSkipCount = 0;
        int ShowLimit = 0;
        public void WordFadeUpdate()
        {
            if (IsPaused) return;
            if (!DeadboxOn) return;

            if (CurrentWord != null)
            {
                if (CurrentWord.Opacity > 0)
                {
                    SeekingWord.Opacity = 0;
                    CurrentWord.Opacity -= 0.01;
                }
                else
                {

                    switch(EntityManager.CurrentActivityLevel)
                    {
                        case (int)EntityManager.ActivityLevels.NothingHappening:
                            SeekingSkip = 8;
                            SeekingWord.Opacity = 0.1f;
                            SeekingWord.FontSize = Units.DynamicFontSizeXL;
                            ShowLimit = 90;
                            break;
                        case (int)EntityManager.ActivityLevels.LowActivity:
                            SeekingSkip = 6;
                            SeekingWord.Opacity = 0.20f;
                            SeekingWord.FontSize = Units.DynamicFontSizeXL;
                            ShowLimit = 80;
                            break;
                        case (int)EntityManager.ActivityLevels.MediumActivity:
                            SeekingSkip = 5;
                            SeekingWord.Opacity = 0.3f;
                            SeekingWord.FontSize = Units.DynamicFontSizeXXL;
                            ShowLimit = 70;
                            break;
                        case (int)EntityManager.ActivityLevels.HighActivity:
                            SeekingSkip = 4;
                            SeekingWord.Opacity = 0.4f;
                            SeekingWord.FontSize = Units.DynamicFontSizeXXXL;
                            ShowLimit = 50;
                            break;
                        default:
                            SeekingSkip = 0;
                            SeekingWord.Opacity = OffTextOpacity;
                            SeekingWord.FontSize = Units.DynamicFontSizeXXXL;
                            ShowLimit = 25;
                            break;
                    }

                    


                    if (EntityManager.CurrentActivityLevel >= (int)EntityManager.ActivityLevels.LowActivity)
                    {
                        if (SeekingSkipCount < SeekingSkip)
                        {
                            SeekingSkipCount++;
                        }
                        else
                        {
                            SeekingSkipCount = 0;
                            SeekingWord.Text = RandomString(Numbers.GetNextRandom(1, 8));
                            if (Numbers.GetNextRandom(0, 100) > 50)
                            {
                                SeekingWord.Text += "\n" + RandomString(Numbers.GetNextRandom(1, 8));
                            }
                            if (Numbers.GetNextRandom(0, 100) > 50)
                            {
                                SeekingWord.Text += "\n" + RandomString(Numbers.GetNextRandom(1, 8));
                            }

                            
                        }
                    }
                    else
                    {
                        SeekingWord.Opacity = 0.0f;
                    }

                    if (Numbers.GetNextRandom(0, 100) > ShowLimit)
                    {
                        SeekingWord.Opacity = 0f;
                    }

                }
            }
            //if (LoggedWordsList != null)
            //{
            //    if (LoggedWordsList.Count > 0 && CurrentWord.Opacity == 0)
            //    {
            //        LoggedWordsList.Clear();
            //    }
            //}

            if (DisplayWordsList != null)
            {
                if (DisplayWordsList.Count > 0 && CurrentWord.Opacity == 0)
                {
                    DisplayWordsList.Clear();
                }
            }
        }


        int LocationCheck = 0;
        int LocationFails = 0;
        int PowerUpLevel = 0;

        public void PowerUpOptions()
        {
            switch (PowerUpLevel)
            {
                case 0:
                case 1:
                    TunesAvailable++;
                    SpeakSimple("NEW VIBES");
                    PowerUpLevel++;
                    break;
                case 2:
                    MoodsAvailable++;
                    SpeakSimple("NEW MOOD");
                    PowerUpLevel++;
                    break;
                case 3:
                case 4:
                    TunesAvailable++;
                    SpeakSimple("NEW VIBES");
                    PowerUpLevel++;
                    break;
                case 5:
                    MoodsAvailable++;
                    SpeakSimple("NEW MOOD");
                    PowerUpLevel++;
                    break;
                case 6:
                    TunesAvailable++;
                    SpeakSimple("NEW VIBES");
                    PowerUpLevel++;
                    break;
            }
            UpdateElements();
        }

        public bool Tick()
        {
            CurrentTime = DateTime.Now;

           
            TimeSpan diffTime = CurrentTime - PowerUpTimeCheck;

            if (diffTime.TotalMinutes > 5)
            {
                if (UserInteraction > 10)
                {
                    PowerUpOptions();
                    UserInteraction = 0;
                    PowerUpTimeCheck = DateTime.Now;
                }
                else
                {
                    Console.WriteLine("Not enough interaction");
                    PowerUpTimeCheck = DateTime.Now;
                }
            }


            Console.WriteLine("TICK");

            if (LocationFails < 10)
            {
                
                if (LocationCheck < 30)
                {
                    LocationCheck++;
                }
                else
                {
                    LocationCheck = 0;
                    if (!GotLocation)
                    {
                        LocationFails++;
                        SetLocationSessionData();
                    }
                }
            }
            

            /*
            if (Numbers.GetNextRandom(100) > 10)
            {
                Console.WriteLine("SKIP");
                return false;
            }

            if (ShowInfoBox)
            {
                return false;
            }

            if ((CurrentTime > NextEventTime) && CurrentTime > StartTime.AddMilliseconds(startDelay))
            {
                try
                {
                    switch (CurrentEvent)
                    {
                        case (int)DarkaudiousEvents.DarkaudiousLoad:
                            break;
                        case (int)DarkaudiousEvents.DarkaudiousStartUp:

                            break;
                        case (int)DarkaudiousEvents.DarkaudiousStandBy:
                            break;
                        case (int)DarkaudiousEvents.DarkaudiousSwitchOn:

                            break;
                        case (int)DarkaudiousEvents.DarkaudiousBegin:
                            DelayBetweenEvents = Numbers.GetNextRandom(1500, 2500);
                            NextEventTime = CurrentTime.AddMilliseconds(DelayBetweenEvents);
                            SetState((int)DarkaudiousEvents.DarkaudiousEvent);
                            PerformingEvent = false;
                            break;
                        case (int)DarkaudiousEvents.DarkaudiousEvent:
                            if (EventDelay != EVENT_SPEED_FAST)
                            {
                                if (scanCount < (ScanFrequency * ScanSensitivity))
                                {
                                    scanCount += 1.0f;
                                    //Console.WriteLine("skip tick");
                                    break;
                                }
                                else
                                {
                                    scanCount = 0.0f;
                                }
                            }

                            if (Numbers.GetNextRandom(0, 100) > 50)
                            {
                                ToggleAccelerometer();
                            }

                            //if (Numbers.GetNextRandom(0, 100) > 100 - RunningSpeed)
                            //{
                            //    SetState((int)DarkaudiousEvents.DarkaudiousBegin);
                            //}
                            //else
                            //{
                                if (!PerformingEvent)
                                {
                                    if (ScreenshotMode)
                                    {
                                        PerformAnEvent();
                                        //SetState((int)DarkaudiousEvents.DarkaudiousBegin);
                                    }
                                    else
                                    {
                                        if (EventDelay == EVENT_SPEED_VERY_SLOW)
                                        {
                                            if (Numbers.GetNextRandom(0, 100) > 75)
                                            {
                                                SetSpeed();
                                            }
                                            else
                                            {
                                                SetState((int)DarkaudiousEvents.DarkaudiousBegin);
                                            }
                                        }
                                        else
                                        {
                                            if (Numbers.GetNextRandom(EVENT_SPEED_VERY_SLOW) > EventDelay)
                                            {
                                                //Console.WriteLine("EVENT STARTED");
                                                PerformAnEvent();
                                            }
                                            else
                                            {
                                                SetState((int)DarkaudiousEvents.DarkaudiousBegin);
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    //Console.WriteLine("BUSY DOING SOMETHING");
                                }
                            //}
                            break;
                        case (int)DarkaudiousEvents.DarkaudiousSpoken:
                            SetState((int)DarkaudiousEvents.DarkaudiousBegin);
                            break;
                        case (int)DarkaudiousEvents.DarkaudiousRestart:
                            break;
                        case (int)DarkaudiousEvents.DarkaudiousSwitchOff:
                            break;
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine("Brain error");
                }
            }

*/

            try
            {
                if (movingDevice)
                {
                    if (Numbers.GetNextRandom(0, 1000) > 500)
                    {
                        Pulse();
                    };
                }
                else
                {
                    if (Numbers.GetNextRandom(0, 1000) > 900)
                    {
                        Pulse();
                    };
                }
            }
            catch(Exception em)
            {
                Console.WriteLine("Movement error");
            }

           
            // RANDOMISE ACTIVITY
            if (Numbers.GetNextRandom(0, 100) > 95)
            {
                if (Numbers.GetNextRandom(0, 100) > 50)
                {
                    IsAnybodyThere = true;
                    SetRandomPersonality();
                }
                else
                {
                    SetNobodyHome();
                }
            }

         
            return true;
        }

        public void SetLocationSessionData()
        {
            Device.BeginInvokeOnMainThread(async () =>
            {
                if (!GotLocation)
                {
                    if (PhraseManager.SessionDictionary.Count == 0)
                    {
                        string randKey = "";
                        for (int i = 0; i <= 24; i++)
                        {
                            randKey += "" + Numbers.GetNextRandom(1, 9);
                        }
                        PhraseManager.AddUniqueKeysFromAlphaNumericString(randKey);
                    }
                    else
                    {
                        //if (Numbers.GetNextRandom(0, 100) > 25)
                        //{
                            await GetCurrentLocation();

                            if (GetLocationInfo() != null)
                            {
                                Console.WriteLine(GetLocationInfo());
                                PhraseManager.AddUniqueKeysFromAlphaNumericString(GetLocationInfo());
                            }
                        //}
                    }
                }
            });
        }


        int FastCount = 0;
        int FastCountLimit = 2;

        bool GotLocation = false;

        public void PerformAnEvent()
        {
            PerformingEvent = true; // stop any more events being fired

            if (Numbers.GetNextRandom(0, 100) > 25)
            {
                TriggerRapidEvents();
            }

            Device.BeginInvokeOnMainThread(async () =>
            {
                SetLocationSessionData();

                if (ScreenshotMode)
                {
                    Speak(PhraseManager.GetDemoPhrase(), true); 
                }
                else
                {

                    int eventType = Numbers.GetNextRandom(0, 10000);
                    Console.WriteLine(eventType);

                    if (eventType >= 9500)
                    {
                        Vibrate();
                    }
                    else if (eventType >= 8500)
                    {
                        Pulse();
                    }

                    Speak(PhraseManager.GetWord(), true);
                }
                if (EventDelay > EVENT_SPEED_MEDIUM)
                {
                    StartEffect();
                    await Task.Delay(3000);
                    StopEffect();
                }
                SetSpeed();

                PerformingEvent = false;

           });
        }

        public void SetSpeed()
        {
            if (EventDelay == EVENT_SPEED_FAST)
            {
                if (FastCount < FastCountLimit)
                {
                    FastCount++;
                }
                else
                {
                    EventDelay = EVENT_SPEED_VERY_SLOW;
                    FastCount = 0;
                    FastCountLimit = Numbers.GetNextRandom(1, 3);
                }
            }
            else
            {
                int seed = Numbers.GetNextRandom(0, 10000);

                if (seed >= 0 && seed < 3000)
                {
                    EventDelay = EVENT_SPEED_MEDIUM_SLOW;
                }
                else if (seed >= 3000 && seed < 4000)
                {
                    EventDelay = EVENT_SPEED_SLOW;
                }
                else if (seed >= 4000 && seed < 5000)
                {
                    EventDelay = EVENT_SPEED_MEDIUM;
                }
                else if (seed >= 5000 && seed < 6000)
                {
                    EventDelay = EVENT_SPEED_MEDIUM_SLOW;
                }
                else if (seed >= 6000 && seed < 7000)
                {
                    EventDelay = EVENT_SPEED_MEDIUM_SLOW;
                }
                else if (seed >= 6000 && seed < 7000)
                {
                    EventDelay = EVENT_SPEED_VERY_SLOW;
                }
                else if (seed >= 7000 && seed < 9500)
                {
                    EventDelay = EVENT_SPEED_MEDIUM_FAST;
                }
                else
                {
                    EventDelay = EVENT_SPEED_FAST;
                }
            }

            //EventDelay = (int)(EventDelay * ScanFrequency);
        }

        public void RepeatWord(int seed)
        {
            if (LoggedWordsList != null)
            {
                if (seed >= (int)(seed * 0.95)) // start to work on a theme by using repetition occassionally
                {
                    if (LoggedWordsList.Count > 10)
                    {
                        if (Numbers.GetNextRandom(0, 100) > 50)
                        {
                            Speak(LoggedWordsList[Numbers.GetNextRandom(0, LoggedWordsList.Count - 1)], true);
                        }
                        else
                        {
                            if (Numbers.GetNextRandom(0, 100) > 75)
                            {
                                RepeatLastWord();
                            }
                            else
                            {
                                RepeatRecentWord();
                            }
                        }
                    }
                }
            }
        }

        public string GetLastWord()
        {
            try
            {
                return LoggedWordsList[LoggedWordsList.Count - 1];
            }
            catch (Exception e)
            {
            }
            return "";
        }

        public void RepeatRecentWord()
        {
            Speak(LoggedWordsList[Numbers.GetNextRandom(LoggedWordsList.Count/3, LoggedWordsList.Count - 1)], true);
        }

        public void RepeatLastWord()
        {
            Speak(LoggedWordsList[LoggedWordsList.Count - 1], true);
        }

        public void Pulse()
        {
            int limit = 1000 - (CurrentStrength * 25);

            if (Numbers.GetNextRandom(0, 100) > 50) { StartEffect(); }

            //if (CurrentStrength > 5)
            //{
            if (Numbers.GetNextRandom(0, 1000) > limit) { PlayInterference(); }
            //}
            

            if (!VibrateOn) { return;  }
            if (Numbers.GetNextRandom(0, 100) >= 2) { return;  } // limit pulsing


            try
            {
                var duration = TimeSpan.FromMilliseconds(Numbers.GetNextRandom(0, 100));

                Device.BeginInvokeOnMainThread(async () =>
                {
                    Vibration.Vibrate(duration);
                    await Circuit.FadeTo(0.75, 10, Easing.Linear);
                    await Circuit.FadeTo(0.25, 100, Easing.Linear);
                });
            }
            catch (FeatureNotSupportedException ex)
            {
                // Feature not supported on device
            }
            catch (Exception ex)
            {
                // Other error has occurred.
            }
        }

        public void Vibrate()
        {

            return;
            try
            {
                // Use default vibration length
                //Vibration.Vibrate();

                // Or use specified time
                var duration = TimeSpan.FromMilliseconds(Numbers.GetNextRandom(50, 500));

                Device.BeginInvokeOnMainThread(async () =>
                {
                    Vibration.Vibrate(duration);
                    await Circuit.FadeTo(0.75, 50, Easing.Linear);
                    await Circuit.FadeTo(0.25, 500, Easing.Linear);

                });
            }
            catch (FeatureNotSupportedException ex)
            {
                // Feature not supported on device
            }
            catch (Exception ex)
            {
                // Other error has occurred.
            }
        }

        public void SimulateMovement()
        {

        }

        public string GetDisplayWords(int limit)
        {
            string loggedWords = "";
            int start = 0;
            int end = 0;

            if (DisplayWordsList == null)
            {
                DisplayWordsList = new List<string>();
            }

            if (DisplayWordsList.Count <= limit)
            {
                foreach (string word in DisplayWordsList)
                {
                    loggedWords += word + "\n";
                }
            }
            else
            {
                end = DisplayWordsList.Count - 1;
                start = end - (limit - 1);

                int count = 0;
                foreach (string word in DisplayWordsList)
                {
                    if (count >= start && count <= end)
                    {
                        loggedWords += word + "\n";
                    }
                    count++;
                }
            }

            return loggedWords;
        }
        


        public string GetLoggedWords(int limit)
        {
            string loggedWords = "";
            int start = 0;
            int end = 0;

            if (LoggedWordsList == null)
            {
                LoggedWordsList = new List<string>();
            }

            if (LoggedWordsList.Count <= limit)
            {
                foreach (string word in LoggedWordsList)
                {
                    loggedWords += word + "\n";
                }
            }
            else
            {
                end = LoggedWordsList.Count - 1;
                start = end - (limit - 1);

                int count = 0;
                foreach (string word in LoggedWordsList)
                {
                    if (count >= start && count <= end)
                    {
                        loggedWords += word + "\n";
                    }
                    count++;
                }
            }

            return loggedWords;
        }

        public void WakeyWakey()
        {
            //Speak("Wakey Wakey");
        }

        int speakDelay = 5;
        public void SpeakSimple(string word)
        {
            //if (SpeechOn)
            //{
            //Device.BeginInvokeOnMainThread(async () =>
            //{
            //    PlaySound("beep");
            //    await Task.Delay(1750);
            //    Console.WriteLine("SPEAK: " + word);
            //    await TextToSpeech.SpeakAsync(word.ToLower());
            //});
            //}

            /*
            if (speakDelay == 0)
            {
                TextToSpeech.SpeakAsync(word.ToLower());
                speakDelay = 5;
            }
            else
            {
                speakDelay--;
                Console.WriteLine("Speak delay " + speakDelay);
            }*/

            TimeSpan diffTime = CurrentTime - StartTime;

            if (diffTime.TotalSeconds < 180)
            {
                Console.WriteLine("Too early for speech");
                return;
            }

            try
            {
                TextToSpeech.SpeakAsync(word.ToLower());
                if (LoggedWordsList == null)
                {
                    LoggedWordsList = new List<string>();
                }

                if (DisplayWordsList == null)
                {
                    DisplayWordsList = new List<string>();
                }

                string toSpeak = word.ToUpper();

                if (toSpeak != "NEW VIBES" && toSpeak != "NEW MOOD")
                {
                    LoggedWordsList.Add(toSpeak);
                }
                DisplayWordsList.Add(toSpeak);


                //CurrentWord.Text = word;

                WordMemory = 3;
                //toSpeak = GetLoggedWords(WordMemory);
                //CurrentWord.Text = GetLoggedWords(WordMemory).ToLower().Replace(" ", "\n");

                toSpeak = GetDisplayWords(WordMemory);
                CurrentWord.Text = GetDisplayWords(WordMemory).ToLower().Replace(" ", "\n");
                
                //CurrentWord.Text = toSpeak;
                CurrentWord.Opacity = 1.0f;

            }
            catch(Exception e)
            {
                TextToSpeech.SpeakAsync("help");
            }
        }

        public void Speak(string word, bool allowBeep)
        {
            SpeakSimple(word);

            



            //CurrentWord.Text = toSpeak.ToLower().Replace(" ", "\n");
            //GetLoggedWords(WordMemory);

            /*
            if (CurrentEvent == (int)DarkaudiousEvents.DarkaudiousSpoken)
            {
                
                return; // already spoken, wait...
            }
            else
            {

                if (LoggedWordsList == null)
                {
                    LoggedWordsList = new List<string>();
                }
                string toSpeak = word.ToUpper();
                LoggedWordsList.Add(toSpeak);


                SwitchOnLights();
                UpdateLights();

                Device.BeginInvokeOnMainThread(async () =>
                {
                    //CurrentWord.Text = GetLoggedWords(WordMemory);
                    CurrentWord.Text = toSpeak;

                    //CurrentWord.Text = toSpeak.ToLower().Replace(" ", "\n");
                    //GetLoggedWords(WordMemory);

                    if (allowBeep)
                    {
                        PlaySound("beep");
                        if (SoundOn)
                        {
                            await Task.Delay(1750);
                        }
                    }


                    //CurrentWord.Text = toSpeak.ToLower().Replace(" ", "\n"); GetLoggedWords(WordMemory);

                    if (SpeechOn)
                    {
                        await TextToSpeech.SpeakAsync(toSpeak.ToLower());
                    }

                    Console.WriteLine(toSpeak.ToLower());
                });

                Console.WriteLine("=========== WORD LIST ===========");
                string allWords = "";
                foreach (string w in LoggedWordsList)
                {
                    //Console.WriteLine(w);
                   
                    allWords += w + ", ";
                }
                Console.WriteLine(allWords);
                UpdateElements();
                CurrentEvent = (int)DarkaudiousEvents.DarkaudiousSpoken;
            }

            if (Numbers.GetNextRandom(0, 100) > 75)
            {
                if (ScanFrequency < 7)
                {
                    ScanFrequency--;
                }
            }
            if (Numbers.GetNextRandom(0, 100) > 80)
            {
                RandomiseMood();

                if (Numbers.GetNextRandom(0, 100) > 60)
                {
                    RandomisePower();

                    if (Numbers.GetNextRandom(0, 100) > 50)
                    {
                        SetRandomPersonality();
                    }
                }
                
            }

            WordHistory.Text = GetLoggedWords(20000);


            WordHistory.Text = GetLog();
            */
        }



        public void ToggleMainOnOff()
        {
            DeadboxOn = !DeadboxOn;
            PlaySound("switch");

            if (DeadboxOn)
            {
                SwitchOn();
            }
            else
            {
                SwitchOff();
            }

            UpdateElements();
        }

        public void ToggleStartStop()
        {
            IsPaused = !IsPaused;
            PlaySound("switch");

            if (IsPaused)
            {
                Pause();
            }
            else
            {
                UnPause();
            }

            UpdateElements();
        }

        public void ToggleLogOnOff()
        {
            /*
            if (AliceOn)
            {
                PlaySound("switch_light");
                LogOn = !LogOn;

                if (LogOn)
                {
                    DisplayAlert("Logging Enabled", "The session will now be logged.", "Ok");

                }
                else
                {
                    if (LoggedWordsList != null)
                    {
                        LoggedWordsList.Clear();
                        DisplayAlert("Logging Disabled", "The log has been cleared.", "Ok");
                    }
                }

                UpdateElements();
            }*/
        }

        public void ToggleViewOnOff()
        {
            /*
            if (AliceOn)
            {
                PlaySound("switch_light");
                TypeWriterView = !TypeWriterView;

                UpdateElements();

                if (TypeWriterView)
                {
                    SetAliceBackground(false);
                    SetTypewriterBackground(false);
                    SetTypewriterBackground(true);
                }
                else
                {
                    SetTypewriterBackground(false);
                    SetAliceBackground(false);
                    SetAliceBackground(true);
                }


            }*/
        }

        public void ToggleSpeechOnOff()
        {
            /*
            if (AliceOn)
            {
                PlaySound("switch_light");
                SpeechOn = !SpeechOn;

                UpdateElements();
            }*/
        }

        bool fadeOut = true;

        public void UpdateElements()
        {

            try
            {
                ClearImage.Opacity = 0.25;
                HistoryImage.Source = "copy_off.png";
                //InfoImage.Opacity = 0.25;
                InfoImage.Source = "info_off.png";

                //Phrase1.Opacity = 0.25f;
                //Phrase2.Opacity = 0.25f;
                //Phrase3.Opacity = 0.25f;
                //Phrase4.Opacity = 0.25f;
                //Tune1.Opacity = 0.25f;
                //Tune1.Opacity = 0.25f;
                //Tune1.Opacity = 0.25f;
                //Tune1.Opacity = 0.25f;

                Tune1.TextColor = Color.White;
                Tune2.TextColor = Color.White;
                Tune3.TextColor = Color.White;
                Tune4.TextColor = Color.White;
                Tune5.TextColor = Color.White;
                Tune6.TextColor = Color.White;
                PianoSelect.TextColor = Color.White;
                ToySelect.TextColor = Color.White;
                ToneSelect.TextColor = Color.White;

                PianoSelect.Opacity = 1.0f;
                ToySelect.Opacity = 1.0f;
                ToneSelect.Opacity = 1.0f;


                if (selectedInstrument == (int)SoundSources.Piano)
                {
                    PianoSelect.TextColor = Color.FromHex(YellowColour);
                }

                if (selectedInstrument == (int)SoundSources.ToyPiano)
                {
                    ToySelect.TextColor = Color.FromHex(YellowColour);
                }

                if (selectedInstrument == (int)SoundSources.Tone)
                {
                    ToneSelect.TextColor = Color.FromHex(YellowColour);
                }

                InvestigationLabel.Text = "SLEEPING";

                if (DeadboxOn)
                {
                    InvestigationLabel.Text = "ACTIVE";
                    
                    Phrase1.IsVisible = true;
                    Phrase2.IsVisible = true;
                    Phrase3.IsVisible = true;
                    Phrase4.IsVisible = true;

                    Tune1.IsVisible = true;
                    Tune2.IsVisible = true;
                    Tune3.IsVisible = true;
                    Tune4.IsVisible = true;
                    Tune5.IsVisible = true;
                    Tune6.IsVisible = true;

                    Tune1.Opacity = OffTextOpacity;
                    Tune2.Opacity = OffTextOpacity;
                    Tune3.Opacity = OffTextOpacity;
                    Tune4.Opacity = OffTextOpacity;
                    Tune5.Opacity = OffTextOpacity;
                    Tune6.Opacity = OffTextOpacity;

                    Phrase1.Opacity = OffTextOpacity;
                    Phrase2.Opacity = OffTextOpacity;
                    Phrase3.Opacity = OffTextOpacity;
                    Phrase4.Opacity = OffTextOpacity;



                    if (TunesAvailable > 0)
                    {
                        Tune1.Opacity = 1.0f;
                    }
                    if (TunesAvailable > 1)
                    {
                        Tune2.Opacity = 1.0f;
                    }
                    if (TunesAvailable > 2)
                    {
                        Tune3.Opacity = 1.0f;
                    }
                    if (TunesAvailable > 3)
                    {
                        Tune4.Opacity = 1.0f;
                    }
                    if (TunesAvailable > 4)
                    {
                        if (GotLocation)
                        {
                            Tune5.Opacity = 1.0f;
                        }
                    }
                    if (TunesAvailable > 5)
                    {
                        if (Mood <= 1)
                        {
                            Tune6.Opacity = 1.0f;
                        }
                    }

                    if (MoodsAvailable > 0)
                    {
                        Phrase1.Opacity = 1.0f;   
                    }
                    if (MoodsAvailable > 1)
                    {
                        Phrase2.Opacity = 1.0f;
                    }
                    if (MoodsAvailable > 2)
                    {
                        Phrase3.Opacity = 1.0f;
                    }
                    if (MoodsAvailable > 3)
                    {
                        Phrase4.Opacity = 1.0f;
                    }


                    if (AudioManager.IsPlaying())
                    {
                        Color selectedColour = Color.FromHex(YellowColour);

                        if (SpiritTriggeredMelody)
                        {
                            selectedColour = Color.FromHex(RedColour);
                        }

                        if (LastPlayedMelody == 1)
                        {
                            Tune1.TextColor = selectedColour;
                        }
                        if (LastPlayedMelody == 2)
                        {
                            Tune2.TextColor = selectedColour;
                        }
                        if (LastPlayedMelody == 3)
                        {
                            Tune3.TextColor = selectedColour;
                        }
                        if (LastPlayedMelody == 4)
                        {
                            Tune4.TextColor = selectedColour;
                        }
                        if (LastPlayedMelody == 5)
                        {
                            Tune5.TextColor = selectedColour;
                        }
                        if (LastPlayedMelody == 6)
                        {
                            Tune6.TextColor = selectedColour;
                        }

                    }






                    SeekingWord.IsVisible = true;

                    PowerImage.Source = "power_on.png";//"power2.png";
                    StartStopImage.Opacity = 1.0f;
                    Mask.Opacity = 0.75f;
                    ThinkingIndicator.Opacity = 0.75f;
                    InvestigationLabel.Opacity = 0.75f;
                    //InfoImage.Opacity = 0.75;
                    InfoImage.Opacity = 1.0f;
                    InfoImage.Source = "info_on.png";
                    if (LoggedWordsList != null)
                    {
                        if (LoggedWordsList.Count > 0)
                        {
                            ClearImage.Opacity = 1.0;
                            HistoryImage.Source = "copy_on.png";
                        }
                    }

                    PlusImage.Opacity = 0.75f;
                    MinusImage.Opacity = 0.75f;

                    if (CurrentStrength < 5)
                    {  
                        //PlusImage.Opacity = 0.25;
                        MinusImage.Opacity = 0.25;
                    }

                    if (ScanFrequency >= 9)
                    {
                        PlusImage.Opacity = 0.25;
                    }

                    if (ScanFrequency < 2)
                    {
                        MinusImage.Opacity = 0.25;
                    }

                    if (IsPaused)
                    {
                        TopLogoOn.Opacity = 0.0f;
                        TopLogoOff.Opacity = 1.0f;
                    }

                    if (SpeechOn)
                    {
                        SpeechButton.Opacity = 0.75f;
                    }
                    else
                    {
                        SpeechButton.Opacity = 0.25f;
                    }

                    if (VibrateOn)
                    {

                        VibrationButton.Opacity = 0.75f;
                    }
                    else
                    {
                        VibrationButton.Opacity = 0.25f;
                    }

                    if (SoundOn)
                    {
                        SoundButton.Opacity = 0.75f;
                    }
                    else
                    {
                        SoundButton.Opacity = 0.25f;
                    }

                    

                }
                else
                {
                    PowerImage.Source = "power_off.png";// "power2off.png";
                    TopLogoOn.Opacity = 0.0f;
                    TopLogoOff.Opacity = 0.25f;
                    StartStopImage.Opacity = 0.25f;
                    Mask.Opacity = 0.15;
                    ThinkingIndicator.Opacity = 0.0f;
                    InvestigationLabel.Opacity = 0.25f;
                    ClearImage.Opacity = 0.25f;
                    //HistoryImage.Opacity = 0.25f;

                    HistoryImage.Source = "copy_off.png";


                    SoundButton.Opacity = 0.25f;
                    SpeechButton.Opacity = 0.25f;
                    VibrationButton.Opacity = 0.25f;

                    PlusImage.Opacity = 0.25f;
                    MinusImage.Opacity = 0.25f;
                    //InfoImage.Opacity = 0.25;

                    InfoImage.Source = "info_off.png";

                    Phrase1.IsVisible = false;
                    Phrase2.IsVisible = false;
                    Phrase3.IsVisible = false;
                    Phrase4.IsVisible = false;
                    Tune1.IsVisible = false;
                    Tune2.IsVisible = false;
                    Tune3.IsVisible = false;
                    Tune4.IsVisible = false;
                    Tune5.IsVisible = false;
                    Tune6.IsVisible = false;
                    SeekingWord.IsVisible = false;
                }

                if (DeadboxOn)
                {
                    if (ShowInfoBox)
                    {
                        WordHistory.Text = GetLoggedWords(20000);
                        WordHistory.Text = GetLog();
                        WordList.Children.Clear();

                        WordList.Children.Add(WordListContent);
                        if (InfoType == WORD_HISTORY)
                        {
                            //WordListContent.Children.Clear();
                            InfoScroller.Content = WordHistory;
                            //WordListContent.Children.Add(InfoScroller);
                            WordList.Children.Add(new StackLayout
                            {
                                Orientation = StackOrientation.Horizontal,
                                Padding = new Thickness(0, 0, 0, 40),
                                Children =
                                {
                                   SaveHistory,
                                   CloseHistory
                                }
                            });
                            //WordList.Children.Add(SaveHistory);
                            //WordList.Children.Add(CloseHistory);
                        }
                        else
                        {
                            //WordListContent.Children.Clear();
                            InfoScroller.Content = InfoContent;
                            //WordListContent.Children.Add(InfoScroller);
                            WordList.Children.Add(new StackLayout
                            {
                                Orientation = StackOrientation.Horizontal,
                                Padding = new Thickness(0, 0, 0, 40),
                                Children =
                                {
                                   CloseHistory
                                }
                            });
                            //WordList.Children.Add(CloseHistory);
                        }
                        PageContent.Children.Add(WordListContainer);
                    }
                    else
                    {
                        PageContent.Children.Remove(WordListContainer);
                    }
                }
            }
            catch(Exception e) { }

            if (Mask != null)
            {
                if (fadeOut)
                {
                    if (Mask.Opacity > 1)
                    {
                        Mask.Opacity += 0.05;
                    }
                    else
                    {
                        fadeOut = true;
                    }
                }
                else
                {
                    if (Mask.Opacity > 0)
                    {
                        Mask.Opacity -= 0.05;
                    }
                    else
                    {
                        fadeOut = false;
                    }
                }
            }



        }

        public void UpdateLights()
        {
            /*
            switch (LightStatus)
            {
                case LIGHTS_ON:
                    OrangeLight.Source = "orangelight1.png";
                    GreenLight.Source = "greenlight1.png";
                    break;
                case LIGHTS_OFF:
                    OrangeLight.Source = "greylight1.png";
                    GreenLight.Source = "greylight1.png";
                    break;
                case GREEN_LIGHT_ON:
                    OrangeLight.Source = "greylight1.png";
                    GreenLight.Source = "greenlight1.png";
                    LightStatus = ORANGE_LIGHT_ON;
                    break;
                case ORANGE_LIGHT_ON:
                    OrangeLight.Source = "orangelight1.png";
                    GreenLight.Source = "greylight1.png";
                    LightStatus = GREEN_LIGHT_ON;
                    break;
            }*/
        }


        public void PlayInterference()
        {
            //StartEffect();

            if (!SoundOn) { return; }

            int id = Numbers.GetNextRandom(1, 9);
            string file = "wn" + id;

            PlaySound(file, Numbers.GetNextRandom(10, 100));
        }

        public void PlaySound(string file, int volume)
        {
            if (!SoundOn) { return; }

            try
            {
                //AudioPlayer.Volume = volume;
                AudioPlayer.Load(file + ".mp3");
                AudioPlayer.Play();
            }
            catch (Exception e)
            {

            }
        }


        public void PlaySound(string file)
        {
            if (!SoundOn) { return; }

            try
            {
                AudioPlayer.Load(file + ".mp3");
                AudioPlayer.Play();
            }
            catch (Exception e)
            {

            }
        }

        public void CreateLog()
        {
            /*string wordList = "";

            wordList +=
                "Darkaudious Session Log\n\n" +
                "Date: " +
                DateTime.Now.ToLongDateString() +
                "\n" +
                "Time: " +
                DateTime.Now.ToShortTimeString() +
                "\n\n" + "Darkaudious received the following:\n\n";


            if (LoggedWordsList != null)
            {
                foreach (string word in this.LoggedWordsList)
                {
                    wordList += word + "\n";
                }
            }*/
            Clipboard.SetTextAsync(GetLog());
        }

        public string GetLog()
        {
            string wordList = "";

            wordList +=
                "Darkaudious Session Log\n\n" +
                "Date: " +
                DateTime.Now.ToLongDateString() +
                "\n" +
                "Time: " +
                DateTime.Now.ToShortTimeString() +
                "\n\n" + "Darkaudious received the following:\n\n";


            if (LoggedWordsList != null)
            {
                foreach (string word in this.LoggedWordsList)
                {
                    wordList += word + "\n";
                }
            }
            return wordList;
        }



        // LOCATION STUFF
        public string GetLocationInfo()
        {
            return LocInfo;
        }

        string LocInfo = "";
        CancellationTokenSource cts;
        Location LastLocation;


        public Location GetMockLocation()
        {
            int mockLong1 = 180 - (Numbers.GetNextRandom(0, 360));
            int mockLong2 = Numbers.GetNextRandom(0, 1000000);

            int mockLat1 = 90 - (Numbers.GetNextRandom(0, 180));
            int mockLat2 = Numbers.GetNextRandom(0, 1000000);

            Location newLocation = new Location
            {
                Altitude = Numbers.GetNextRandom(0, 200),
                Longitude = double.Parse(mockLong1 + "." + mockLong2),
                Latitude = double.Parse(mockLat1 + "." + mockLat2)
            };

            return newLocation;
        }

        Location SessionLocation = null;

        async Task GetCurrentLocation()
        {
            try
            {
                await Task.Delay(50);
                var location = GetMockLocation();

                if (!UseMockLocation)
                {
                    var request = new GeolocationRequest(GeolocationAccuracy.Medium, TimeSpan.FromSeconds(10));
                    cts = new CancellationTokenSource();
                    location = await Geolocation.GetLocationAsync(request, cts.Token);
                }

                if (location != null)
                {
                    if (LastLocation == null)
                    {
                        LastLocation = location;
                        SessionLocation = LastLocation;
                        AudioManager.UpdateLocationMelody(SessionLocation);
                        GotLocation = true;

                    }
                    else
                    {
                        double diff = Location.CalculateDistance(LastLocation, location, DistanceUnits.Miles);
                        if (diff > 0.01)
                        {
                            Console.WriteLine(diff);

                            ChangeLocation();

                            if (Numbers.GetNextRandom(0, 100) > 50)
                            {
                                PhraseManager.ResetCurrentFeeling();
                            }
                        }
                        else
                        {
                            Console.WriteLine(diff);
                            //Speak("We're in the same location");
                        }

                    }
                    LocInfo = "" + location.Latitude + location.Longitude + location.Altitude;
                    Console.WriteLine($"Latitude: {location.Latitude}, Longitude: {location.Longitude}, Altitude: {location.Altitude}");
                }
            }
            catch (FeatureNotSupportedException fnsEx)
            {
                // Handle not supported on device exception
            }
            catch (FeatureNotEnabledException fneEx)
            {
                // Handle not enabled on device exception
            }
            catch (PermissionException pEx)
            {
                // Handle permission exception
            }
            catch (Exception ex)
            {
                // Unable to get location
            }
        }

        protected override void OnDisappearing()
        {
            if (cts != null && !cts.IsCancellationRequested)
                cts.Cancel();
            base.OnDisappearing();
        }
        // END LOCATION


        void Accelerometer_ReadingChanged(object sender, AccelerometerChangedEventArgs e)
        {
            if (!DeadboxOn) { return; }

            if (MovementSensorTriggerCount < 100)
            {
                MovementSensorTriggerCount++;
            }
            else
            {
                var data = e.Reading;
                //Console.WriteLine($"Reading: X: {data.Acceleration.X}, Y: {data.Acceleration.Y}, Z: {data.Acceleration.Z}");

                MoveX = (int)data.Acceleration.X;
                MoveY = (int)data.Acceleration.Y;
                MoveZ = (int)data.Acceleration.Z;


                // Process Acceleration X, Y, and Z
                if (data.Acceleration.X > 0.5 || data.Acceleration.Y > 0.75 || data.Acceleration.Z < 0.5)
                {
                    movingDevice = true;

                    if (LikesMovement)
                    {
                        if (Numbers.GetNextRandom(0, 100) < 10)
                        {
                            Speak(PhraseManager.GetPositiveMoveResponse(), true);
                            //SpeedUpScan(true);
                            PowerUp();
                        }
                        
                    }
                    else
                    {
                        if (Numbers.GetNextRandom(0, 100) < 10)
                        {
                            Speak(PhraseManager.GetNegativeMoveResponse(), true);
                            //SlowDownScan(true);
                            PowerDown();
                        }
                    }

                    if (Numbers.GetNextRandom(0, 1000) < 30)
                    {
                        Pulse();
                    };

                }
                else
                {
                    movingDevice = false;

                    if (LikesMovement)
                    {
                        if (Numbers.GetNextRandom(0, 100) < 2)
                        {
                            Speak(PhraseManager.GetNegativeStillResponse(), true);
                            //SlowDownScan(true);
                            PowerDown();

                        }
                    }
                    else
                    {
                        if (Numbers.GetNextRandom(0, 100) < 2)
                        {
                            Speak(PhraseManager.GetPositiveStillResponse(), true);
                            //SpeedUpScan(true);
                            PowerUp();
                        }
                    }

                    if (Numbers.GetNextRandom(0, 1000) < 10)
                    {
                        Pulse();
                    };

                }

                MovementSensorTriggerCount = 0;
            }
        }

        public void ToggleAccelerometer()
        {
            try
            {
                if (Accelerometer.IsMonitoring)
                    Accelerometer.Stop();
                else
                    Accelerometer.Start(MovementSensorSpeed);
            }
            catch (FeatureNotSupportedException fnsEx)
            {
                // Feature not supported on device
            }
            catch (Exception ex)
            {
                // Other error has occurred.
            }
        }
        // END ACCELEROMETER



        

        void Magnetometer_ReadingChanged(object sender, MagnetometerChangedEventArgs e)
        {
            var data = e.Reading;
            // Process MagneticField X, Y, and Z
            Console.WriteLine($"Reading: X: {data.MagneticField.X}, Y: {data.MagneticField.Y}, Z: {data.MagneticField.Z}");
            MagnoX = data.MagneticField.X;
            MagnoY = data.MagneticField.Y;
            MagnoZ = data.MagneticField.Z;
        }

        public void ToggleMagnetometer()
        {
            try
            {
                if (Magnetometer.IsMonitoring)
                    Magnetometer.Stop();
                else
                    Magnetometer.Start(MagnoSpeed);
            }
            catch (FeatureNotSupportedException fnsEx)
            {
                // Feature not supported on device
            }
            catch (Exception ex)
            {
                // Other error has occurred.
            }
        }

        void Barometer_ReadingChanged(object sender, BarometerChangedEventArgs e)
        {
            var data = e.Reading;
            // Process Pressure
            Console.WriteLine($"Reading: Pressure: {data.PressureInHectopascals} hectopascals");

            BaroPressure = (float)data.PressureInHectopascals;
        }

        public void ToggleBarometer()
        {
            try
            {
                if (Barometer.IsMonitoring)
                    Barometer.Stop();
                else
                    Barometer.Start(BaroSpeed);
            }
            catch (FeatureNotSupportedException fnsEx)
            {
                // Feature not supported on device
            }
            catch (Exception ex)
            {
                // Other error has occurred.
            }
        }
        
    }
}
