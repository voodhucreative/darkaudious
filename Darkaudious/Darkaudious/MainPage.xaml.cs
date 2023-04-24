using System;
using System.Collections.Generic;
using System.IO;
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

        bool PerformingEvent;

        ISimpleAudioPlayer AudioPlayer;

        List<string> LoggedWordsList;

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

        Label Phrase1;
        Label Phrase2;
        Label Phrase3;
        Label Phrase4;



        public MainPage()
        {
            CurrentEvent = (int)DarkaudiousEvents.DarkaudiousLoad;

            PhraseManager = new PhraseManager();
            PhraseManager.PopulateDictionary();

            if (Device.RuntimePlatform == Device.Android)
            {
                UseMockLocation = true;
            }

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


            InfoPoints.Add("", "CONTROLS");
            InfoPoints.Add("power2off.png", "POWER ON/OFF - switches Darkaudious System on and off. Switching off will clear all data");
            InfoPoints.Add("start2off.png", "START/PAUSE INVESTIGATION - starts and pauses the Darkaudious system");
            InfoPoints.Add("copypaste.png", "COPY/SAVE - copies the session to the device clipboard for sharing. You can paste copied sessions into any program or app you like!");
            InfoPoints.Add("eraser.png", "CLEAR - clears the current session and deletes all messages");

            InfoPoints.Add("sound.png", "SOUND ON/OFF - switches sound on and off");
            InfoPoints.Add("mouth.png", "SPEECH ON/OFF - switches speech on and off");
            InfoPoints.Add("vibrate.png", "VIBRATION ON/OFF - switches vibration on and off");

            InfoPoints.Add("info.png", "INFORMATION - opens this page");
            InfoPoints.Add("more.png", "INCREASE SCAN RATE - turns up the frequency of the scanning system. This can only be done if the power of the spirit connection is strong enough");
            InfoPoints.Add("less.png", "DECREASE SCAN RATE - turns down the frequency of the scanning system. This will reduce the possibilty of contact, but may increase the accuracy of the messages");

            InfoPoints.Add("info", "USAGE TIPS\n\na settled spirit doesn't like too much movement so try and keep still to encourage them to respond\n\na restless spirit likes movement so try moving your device around to encourage them to respond\n\nsome spirits are very mischeivious and might try and throw you off guard with some very random phrases!\n\nif you're not having much luck, try moving to a different location... don't worry, Darkaudious will know you've moved!");


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
                HeightRequest = Units.ScreenUnitL * 5.5,
                //HeightRequest = 120,
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
                Text = "INVESTIGATION ACTIVE",
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
                Source = "switchOff.png",//"power2off.png",
                WidthRequest = 80,
                HeightRequest = 80,
                Aspect = Aspect.AspectFit,
                HorizontalOptions = LayoutOptions.CenterAndExpand,
                VerticalOptions = LayoutOptions.EndAndExpand,
                Margin = 8
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
                Source = "copypaste.png",
                WidthRequest = Units.ScreenUnitM * 2,
                HeightRequest = Units.ScreenUnitM * 2,
                Aspect = Aspect.AspectFit,
                HorizontalOptions = LayoutOptions.CenterAndExpand,
                VerticalOptions = LayoutOptions.EndAndExpand,
                Margin = 8
            };

            InfoImage = new Image
            {
                //Source = "historybtn.png",
                Source = "info.png",
                WidthRequest = Units.ScreenUnitL,
                HorizontalOptions = LayoutOptions.CenterAndExpand,
                VerticalOptions = LayoutOptions.StartAndExpand,
                Margin = new Thickness(0, 0, Units.ScreenUnitM, Units.ScreenUnitM),
                Opacity = 0.25f
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
                Text = "EAR 1",
                FontSize = Units.DynamicFontSizeXL,
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
                              IsSpeaking = false;
                              playSignatureMelody = true;
                              SignatureMelody = AudioManager.GetSessionMelody(1);
                              MelodyCount = 0;
                          }
                      })
                 }
             );

            Phrase1 = new Label
            {
                Text = "SAY 1",
                FontSize = Units.DynamicFontSizeXL,
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
                             IsSpeaking = true;
                             playSignatureMelody = true;
                             WordsToSpeak = PhraseManager.GetSessionPhrase(1);
                             MelodyCount = 0;
                         }
                     })
                 }
             );

            // SET 2
            Tune2 = new Label
            {
                Text = "EAR 2",
                FontSize = Units.DynamicFontSizeXL,
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
                             IsSpeaking = false;
                             playSignatureMelody = true;
                             SignatureMelody = AudioManager.GetSessionMelody(2);
                             MelodyCount = 0;
                         }
                     })
                 }
             );

            Phrase2 = new Label
            {
                Text = "SAY 2",
                FontSize = Units.DynamicFontSizeXL,
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
                             IsSpeaking = true;
                             playSignatureMelody = true;
                             WordsToSpeak = PhraseManager.GetSessionPhrase(2);
                             MelodyCount = 0;
                         }
                     })
                 }
             );

            // SET 3
            Tune3 = new Label
            {
                Text = "EAR 3",
                FontSize = Units.DynamicFontSizeXL,
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
                             IsSpeaking = false;
                             playSignatureMelody = true;
                             SignatureMelody = AudioManager.GetSessionMelody(3);
                             MelodyCount = 0;
                         }
                     })
                 }
             );

            Phrase3 = new Label
            {
                Text = "SAY 3",
                FontSize = Units.DynamicFontSizeXL,
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
                             IsSpeaking = true;
                             playSignatureMelody = true;
                             WordsToSpeak = PhraseManager.GetSessionPhrase(3);
                             MelodyCount = 0;
                         }
                     })
                 }
             );

            // SET 4
            Tune4 = new Label
            {
                Text = "EAR 4",
                FontSize = Units.DynamicFontSizeXL,
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
                             IsSpeaking = false;
                             playSignatureMelody = true;
                             SignatureMelody = AudioManager.GetSessionMelody(4);
                             MelodyCount = 0;
                         }
                     })
                 }
             );

            Phrase4 = new Label
            {
                Text = "SAY 4",
                FontSize = Units.DynamicFontSizeXL,
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
                             IsSpeaking = true;
                             playSignatureMelody = true;
                             WordsToSpeak = PhraseManager.GetSessionPhrase(4);
                             MelodyCount = 0;
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
                VerticalOptions = LayoutOptions.EndAndExpand,
                VerticalTextAlignment = TextAlignment.End
            };

            GrandPiano.GestureRecognizers.Add(
                 new TapGestureRecognizer()
                 {
                     Command = new Command(() =>
                     {

                         if (DeadboxOn)
                         {
                             selectedInstrument = (int)SoundSources.Noise;
                             AudioManager.SetInstrument(selectedInstrument);
                             AudioManager.SetSoundSource(selectedInstrument);
                             TriggerRapidEvents();
                         }
                     })
                 }
             );

            CurrentWord = new Label
            {
                Text = "",
                FontSize = Units.DynamicFontSizeXXXXL,
                FontFamily = Fonts.GetBoldAppFont(),
                TextColor = Color.FromHex("fbb515"),
                Opacity = 1.0f,
                HorizontalOptions = LayoutOptions.Center,
                HorizontalTextAlignment = TextAlignment.Center,
                VerticalOptions = LayoutOptions.Center,
                VerticalTextAlignment = TextAlignment.Center,
                HeightRequest = Units.ScreenHeight,
                Margin = new Thickness(0, 80, 0, 0)
            };

            WordHistory = new Label
            {
                Text = "",
                FontSize = Units.DynamicFontSizeXXXL,
                FontFamily = Fonts.GetBoldAppFont(),
                TextColor = Color.FromHex("86eb2e"),
                Opacity = 1.0f,
                HorizontalOptions = LayoutOptions.CenterAndExpand,
                HorizontalTextAlignment = TextAlignment.Center,
                VerticalOptions = LayoutOptions.Start,
                VerticalTextAlignment = TextAlignment.Start
            };

            ThinkingIndicator = new Label
            {
                Text = "",
                FontSize = Units.DynamicFontSizeM,
                FontFamily = Fonts.GetBoldAppFont(),
                TextColor = Color.White,//FromHex("86eb2e"),
                Opacity = 0.5,
                Margin = new Thickness(0, 0, 0, 0),
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

            StackLayout ContentContainer = new StackLayout
            {
                Orientation = StackOrientation.Vertical,

                Padding = new Thickness(0, 16),
                Children =
                {
                    //ControlsLayout,
                    CurrentWord,
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
                Margin = new Thickness(0, 180, 0, 0),
                HeightRequest = Units.QuarterScreenHeight
            };

            TopControlsGrid.Children.Add(PianoKeysImage, 0, 0);
            

            Grid.SetColumnSpan(PianoKeysImage, 2);
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
                VerticalOptions = LayoutOptions.EndAndExpand,
                //BackgroundColor = Color.Red,
                Spacing = 16,
                Children =
                {
                    Phrase1,
                    Phrase2,
                    Phrase3,
                    Phrase4,
                    SoundButton,
                    SpeechButton,
                    VibrationButton
                }
            };

            StackLayout RightControls = new StackLayout
            {
                Orientation = StackOrientation.Vertical,
                VerticalOptions = LayoutOptions.EndAndExpand,
                //BackgroundColor = Color.Blue,
                Spacing = 16,
                Children =
                {
                    //new Grid {WidthRequest = Units.ScreenWidth10Percent, HeightRequest = Units.ScreenWidth10Percent},
                    Tune1,
                    Tune2,
                    Tune3,
                    Tune4,
                    InfoImage,
                    MinusImage,
                    PlusImage
                }
            };

            ControlsGrid.Children.Add(LeftControls, 0, 4);
            ControlsGrid.Children.Add(RightControls, 4, 4);

            LeftControls.Opacity = 1;
            RightControls.Opacity = 1;

            //ControlsGrid.Children.Add(ClearImage, 0, 11);
            //ControlsGrid.Children.Add(StartStopImage, 2, 11);
            ControlsGrid.Children.Add(GrandPiano, 0, 12);        
            ControlsGrid.Children.Add(PowerImage, 1, 12);
            ControlsGrid.Children.Add(ToyPiano, 2, 12);

            //ControlsGrid.Children.Add(HistoryImage, 3, 11);


            //Grid.SetRowSpan(ClearImage, 2);
            //Grid.SetRowSpan(StartStopImage, 2);
            Grid.SetRowSpan(GrandPiano, 3);
            Grid.SetRowSpan(PowerImage, 3);
            Grid.SetRowSpan(ToyPiano, 3);
            Grid.SetColumnSpan(GrandPiano, 3);
            Grid.SetColumnSpan(PowerImage, 3);
            Grid.SetColumnSpan(ToyPiano, 3);

            //Grid.SetRowSpan(HistoryImage, 2);




            Grid.SetRowSpan(LeftControls, 8);
            Grid.SetRowSpan(RightControls, 8);


            ControlsGrid.GestureRecognizers.Add(
                  new TapGestureRecognizer()
                  {
                      Command = new Command(() =>
                      {

                          //AudioManager.SetInstrument(0);
                          AudioManager.PlayNote(0, Numbers.GetNextRandom(0, 8), true);
                      })
                  }
              );

            PageContent.Children.Add(ControlsGrid, 0, 0);

            Console.WriteLine("Lets get going");

            BrainTimer = new StoppableTimer(TimeSpan.FromMilliseconds(1000), TimedUpdate, true);
            EffectAnimTimer = new StoppableTimer(TimeSpan.FromMilliseconds(effectAnimDelay), EffectUpdate, true);
            LogoAnimTimer = new StoppableTimer(TimeSpan.FromMilliseconds(effectAnimDelay), LogoUpdate, true);
            //QuoteTimer = new StoppableTimer(TimeSpan.FromMilliseconds(quoteDelay), QuoteUpdate, true);

            ScanTimer = new StoppableTimer(TimeSpan.FromMilliseconds(250), ScanUpdate, true);

            AudioTimer = new StoppableTimer(TimeSpan.FromMilliseconds(250), AudioUpdate, true);
            AudioTimer.Start();

            Accelerometer.ReadingChanged += Accelerometer_ReadingChanged;
            Magnetometer.ReadingChanged += Magnetometer_ReadingChanged;
            Barometer.ReadingChanged += Barometer_ReadingChanged;

            Settings.AccelerometerAvailable = false;
            Settings.MagnetometerAvailable = false;
            Settings.BarometerAvailable = false;

            try
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    //Accelerometer.Start(SensorSpeed.UI);
                    //Settings.AccelerometerAvailable = true;
                });
            }
            catch (FeatureNotSupportedException e)
            {

                Console.WriteLine("ERROR STARTING ACCELEROMETER ");
            }


            try
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    //Magnetometer.Start(SensorSpeed.UI);
                    //Settings.MagnetometerAvailable = true;
                });
            }
            catch (FeatureNotSupportedException fnsEx)
            {
                // Feature not supported on device
                Console.WriteLine("MAGNOMETER NOT SUPPORTED");
            }
            catch (Exception ex)
            {
                // Other error has occurred.
                Console.WriteLine("ERROR STARTING MAGNOMETER ");
            }

            try
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    //Barometer.Start(SensorSpeed.UI);
                    //Settings.BarometerAvailable = true;
                });
            }
            catch (FeatureNotSupportedException fnsEx)
            {
                // Feature not supported on device
                Console.WriteLine("BAROMETER NOT SUPPORTED");
            }
            catch (Exception ex)
            {
                // Other error has occurred.
                Console.WriteLine("ERROR STARTING BAROMETER ");
            }


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
                        FontSize = Units.DynamicFontSizeXXXL,
                        FontFamily = Fonts.GetBoldAppFont(),
                        TextColor = Color.FromHex("86eb2e"),
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
                ThinkingIndicator.FontSize = Units.DynamicFontSizeM;
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
                ThinkingIndicator.FontSize = Units.DynamicFontSizeM;
                InvestigationLabel.FontSize = Units.DynamicFontSizeXXL;
                InvestigationLabel.Margin = new Thickness(0, Units.ScreenUnitXS, 0, 0);
                CurrentWord.FontSize = Units.DynamicFontSizeXXXL;

                //CurrentWord.Margin = new Thickness(0, Units.ScreenUnitS, 0, 0);
                CurrentWord.TranslateTo(0, 0, 0, Easing.Linear);
                ThinkingIndicator.TranslateTo(0, -160, 0, Easing.Linear);
                WordMemory = 6;
            }

            CurrentWord.FontSize = Units.DynamicFontSizeXXXL;

            this.Content = PageContent;
        }

        public void ShowHistory()
        {
            ShowInfoBox = true;
            InfoType = WORD_HISTORY;
            UpdateElements();
        }

        public void HideHistory()
        {
            ShowInfoBox = false;
            InfoType = WORD_HISTORY;
            UpdateElements();
        }

        public void ShowGeneralInfo()
        {
            ShowInfoBox = true;
            InfoType = GENERAL_INFO;
            UpdateElements();
        }

        public void HideGeneralInfo()
        {
            ShowInfoBox = false;
            InfoType = GENERAL_INFO;
            UpdateElements();

        }

        public void SaveHistoryList()
        {
            CreateLog();
            DisplayAlert("Session Logged", "Your session has been saved to your device's clipboard. You can now paste this into an email, message, document or wherever you choose. Simply open up the app you'd like to use, then press and hold in a text area to bring up the option to paste your session.", "Ok");
            HideHistory();
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
            Speak("Off", false);
            CurrentWord.Text = "";

            if (LoggedWordsList != null)
            {
                LoggedWordsList.Clear();
            }

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




        public void TimedUpdate()
        {
            if (IsPaused) return;




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


        int UpdateFrequency = 5;
        int UpdateSkip = 0;
        //int rapidActivity = 0;
        int rapidActivityLength = 4;
        int rapidActivityType = 0;
        int rapidNoteLength = 1;
        bool playSignatureMelody = false;

        public void TriggerRapidEvents()
        {
            rapidActivityType = Numbers.GetNextRandom(0, 3);
            rapidActivityLength = Numbers.GetNextRandom(2, 12);
        }




        public void AudioUpdate()
        {
            if (playSignatureMelody)
            {
                UpdateMelody();
                return;
            }

            if (rapidActivityLength == 0) // normal
            {
                if (UpdateSkip < UpdateFrequency)
                {
                    UpdateSkip++;

                    Console.WriteLine("Update Skip " + UpdateSkip);
                    return;
                }
            }
            else
            {
                // rapid event note
                try
                {
                    switch (rapidActivityType)
                    {
                        // voice event
                        case 0:
                            selectedInstrument = (int)SoundSources.ToyPiano;
                            AudioManager.SetInstrument(selectedInstrument);
                            AudioManager.SetSoundSource(selectedInstrument);
                            break;
                        // noise event
                        case 1:
                            selectedInstrument = (int)SoundSources.Piano;
                            AudioManager.SetInstrument(selectedInstrument);
                            AudioManager.SetSoundSource(selectedInstrument);
                            break;
                        case 2:
                            AudioManager.SetSoundSource((int)SoundSources.Noise);
                            StartEffect();
                            SpeakSimple(PhraseManager.GetMuttering());
                            break;
                        default:
                            AudioManager.SetSoundSource((int)SoundSources.Noise);
                            StartEffect();
                            SpeakSimple(PhraseManager.GetMuttering());
                            break;
                            // music event
                    }

                    if (rapidActivityType < 2)
                    {
                        Console.WriteLine("Rapid: " + rapidActivityLength);
                        AudioManager.PlayNote(rapidNoteLength, NotesToPlay[MelodyCount], true);
                    }

                }
                catch(Exception e)
                {
                    Console.WriteLine("Note not available");
                }
            }

            if (IsPaused)
            {
                return;
            }

            if (!DeadboxOn)
            {
                return;
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

            if (Numbers.GetNextRandom(100) > freq)
            {
                if (AudioType == AUDIO_SPEECH)
                {
                    AudioType = AUDIO_INSTRUMENT;
                }
                else
                {
                    if (Numbers.GetNextRandom(100) > 90)
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

            //SpeechUpdate();
        }

        public void SpeechUpdate(int hitRate)
        {
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
            Console.WriteLine("Update melody");
            selectedInstrument = (int)SoundSources.Piano;
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

        public void SetSimulatedSensors()
        {
            if (Numbers.GetNextRandom(100) > 98)
            {
                BaroPressure = Numbers.GetNextRandom(980, 1080);
            }
            if (Numbers.GetNextRandom(100) > 98)
            {
                MagnoX = Numbers.GetNextRandom(0, 10);
            }
            if (Numbers.GetNextRandom(100) > 98)
            {
                MagnoY = Numbers.GetNextRandom(0, 10);
            }
            if (Numbers.GetNextRandom(100) > 98)
            {
                MagnoZ = Numbers.GetNextRandom(0, 10);
            }

            if (BaroPressure > 1060)
            {
                // TRIGGER EVENT
                Console.WriteLine("TRIGGER PRESSURE EVENT");
                TriggerRapidEvents();
                BaroPressure = 980;

            }
        }

        public void ScanUpdate()
        {
            if (IsPaused) return;

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


            ThinkingIndicator.Text = "SPIRIT ACTIVITY: ";
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

        

        public bool Tick()
        {
            CurrentTime = DateTime.Now;

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

            /*
            if (!PerformingEvent)
            {
                if (CurrentWord.Text == "-")
                {
                    showThinking = !showThinking;

                    if (showThinking)
                    {
                        CurrentWord.Opacity = 0.8;
                    }
                    else
                    {
                        CurrentWord.Opacity = 0;
                    }
                } 
            }
            else
            {
                //CurrentWord.Text = "-";
                CurrentWord.Opacity = 0.8;
                showThinking = true;
            }
            */

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

            /*
            if (ScanFrequency > 4)
            {
                TopLogo.Source = "dcmainlogo_off.png";
            }
            else
            {
                if (ScanFrequency <= 2)
                {
                    TopLogo.Source = "dcmainlogo_on.png";
                }
                else
                {
                    if (DateTime.Now.Millisecond % 3 == 0 || DateTime.Now.Millisecond % 7 == 0 || DateTime.Now.Millisecond % 10 == 0)
                    {
                        TopLogo.Source = "dcmainlogo_on.png";
                    }
                }
            }


            TopLogo.Source = "dcmainlogo_off.png";
            if (DateTime.Now.Millisecond % 3 == 0 || DateTime.Now.Millisecond % 7 == 0 || DateTime.Now.Millisecond % 10 == 0)
            {
                TopLogo.Source = "dcmainlogo_on.png";
            }
            */
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
                        if (Numbers.GetNextRandom(0, 100) > 25)
                        {
                            await GetCurrentLocation();

                            if (GetLocationInfo() != null)
                            {
                                Console.WriteLine(GetLocationInfo());
                                PhraseManager.AddUniqueKeysFromAlphaNumericString(GetLocationInfo());
                            }
                        }
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

                    /*
                    if (eventType >= 0 && eventType < 2000)
                    {
                        //await Task.Delay(1000);
                        if (Numbers.GetNextRandom(0, 100) > 25)
                        {
                            Speak(PhraseManager.GetFeeling(), true);
                        }
                        else
                        {
                            Speak(PhraseManager.GetCurrentFeeling(), true);
                        }

                    }
                    else if (eventType >= 2000 && eventType < 5000)
                    {
                        //await Task.Delay(1000);
                        Speak(PhraseManager.GetWord(), true);
                    }
                    else if (eventType >= 5000 && eventType < 8000)
                    {
                        //await Task.Delay(1000);
                        Speak(PhraseManager.GetItem(), true);
                    }
                    else if (eventType >= 8000 && eventType < 8500)
                    {
                        // fairly commoon
                        //await Task.Delay(1000);
                        Speak(PhraseManager.GetSombodysItem(), true);
                    }
                    else if (eventType >= 8500 && eventType < 9000)
                    {
                        //await Task.Delay(1000);
                        Speak(PhraseManager.GetDescriptiveItem(), true);
                    }
                    else if (eventType >= 9000 && eventType < 9500)
                    {
                        // fairly commoon
                        //await Task.Delay(1000);
                        Speak(PhraseManager.GetSombodysItem(), true);
                    }
                    else if (eventType >= 9500 && eventType < 9750)
                    {
                        // rare
                        //await Task.Delay(1000);
                        Speak(PhraseManager.GetFeelingPhrase(), true);

                    }
                    else if (eventType >= 9750)
                    {
                        // rare
                        //await Task.Delay(1000);
                        Speak(PhraseManager.GetSimpleSentence(), true);
                    }


                    // just stupid - don't do this
                    //await Task.Delay(1000);
                    //Speak(PhraseManager.GetComplexSentence());

                    RepeatWord(eventType);*/

                }
                if (EventDelay > EVENT_SPEED_MEDIUM)
                {
                    StartEffect();
                    await Task.Delay(3000);
                    StopEffect();
                }
                SetSpeed();

                /*
                CurrentWord.Text = "";
                string newWord = PhraseManager.GetWord();
                wordsSpoken += newWord + "\n";
                Speak(newWord);
                Console.WriteLine(wordsSpoken);
                Console.WriteLine("EVENT FINISHED");
                */

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
            try
            {
                TextToSpeech.SpeakAsync(word.ToLower());
            }
            catch(Exception e)
            {
                TextToSpeech.SpeakAsync("help");
            }
        }

        public void Speak(string word, bool allowBeep)
        {
           
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
                HistoryImage.Opacity = 0.25;
                InfoImage.Opacity = 0.25;
                if (DeadboxOn)
                {
                    PowerImage.Source = "switchOn.png";//"power2.png";
                    StartStopImage.Opacity = 1.0f;
                    Mask.Opacity = 0.75f;
                    ThinkingIndicator.Opacity = 0.75f;
                    InvestigationLabel.Opacity = 0.75f;
                    InfoImage.Opacity = 0.75;
                    if (LoggedWordsList != null)
                    {
                        if (LoggedWordsList.Count > 0)
                        {
                            ClearImage.Opacity = 1.0;
                            HistoryImage.Opacity = 1.0;
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
                    PowerImage.Source = "switchOff.png";// "power2off.png";
                    TopLogoOn.Opacity = 0.0f;
                    TopLogoOff.Opacity = 0.25f;
                    StartStopImage.Opacity = 0.25f;
                    Mask.Opacity = 0.15;
                    ThinkingIndicator.Opacity = 0.0f;
                    InvestigationLabel.Opacity = 0.25f;
                    ClearImage.Opacity = 0.25f;
                    HistoryImage.Opacity = 0.25f;

                    SoundButton.Opacity = 0.25f;
                    SpeechButton.Opacity = 0.25f;
                    VibrationButton.Opacity = 0.25f;

                    PlusImage.Opacity = 0.25f;
                    MinusImage.Opacity = 0.25f;
                    InfoImage.Opacity = 0.25;
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
                    //Console.WriteLine($"Latitude: {location.Latitude}, Longitude: {location.Longitude}, Altitude: {location.Altitude}");
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
