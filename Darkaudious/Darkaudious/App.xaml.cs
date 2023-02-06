using System;
using Darkaudious.Helpers;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Darkaudious
{
    public partial class App : Application
    {
        MainPage mainPage;
        public App(int width, int height, int pixelWidth, int pixelHeight, int statusBarHeight)
        {
            InitializeComponent();

            DeviceDisplay.KeepScreenOn = true;
            Fonts.Init();
            Units.Init(width, height, pixelWidth, pixelHeight, statusBarHeight);

            mainPage = new MainPage();
            MainPage = mainPage;

        }

        public void SayAwake()
        {
            Device.BeginInvokeOnMainThread(async () =>
            {
                mainPage.WakeyWakey();
            });
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}

