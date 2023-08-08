using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Essentials;
using System.Diagnostics;

namespace Semana13Movil
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
            btnNet.Clicked += BtnNet_Clicked;
            btnMap.Clicked += BtnMap_Clicked;
            btnMail.Clicked += BtnMail_Clicked;
            ButtonHablar.Clicked += ButtonHablar_Clicked;
        }

        private async void ButtonHablar_Clicked(object sender, EventArgs e)
        {
            //SpeakNowDefaultSettings();
            await TextToSpeech.SpeakAsync(EntryText.Text, new SpeechOptions
            {
                Volume = (float)SliderVolume.Value
            }
            );
        }

            private void BtnMail_Clicked(object sender, EventArgs e)
        {
            String Body = "Este es el contenido del mensaje";
            List<string> Mailers = new List<string>();
            Mailers.Add(EntryEmail.Text);
            //Mailers.Add("elcorreo@server.com");
            SendEmail(EntrySubject.Text, "El Body", Mailers);
        }

        public async Task SendEmail(string subject, string body, List<string> recipientes)
        {
            try
            {
                var message = new EmailMessage
                {
                    Subject = subject,
                    Body = body,
                    To = recipientes,
                    //Cc = ccRecipientes,
                    //Bcc = bccRecipientes
                    //BodyFormat = EmailBodyFormat.Html
                    //BodyFormat = EmailBodyFormat.PlainText
                };
                await Email.ComposeAsync(message);
            }
            catch (FeatureNotSupportedException fbsEx)
            {
                // Email no soportado en este dispositivo
            }
            catch (Exception ex)
            {
                // Error diferente
            }
        }


        private void BtnNet_Clicked(object sender, EventArgs e)
        {
            ObtenerUbica();
        }

        private void BtnMap_Clicked(object sender, EventArgs e)
        {
            if (!double.TryParse(EntryLatitude.Text, out double lat)) { return; }
            if (!double.TryParse(EntryLongitud.Text, out double lng)) { return; }
            Map.OpenAsync(lat, lng, new MapLaunchOptions
            {
                Name = EntryName.Text,
                NavigationMode = NavigationMode.None
            });
        }


        private void HayRed()
        {
            var current = Connectivity.NetworkAccess;

            if (current == NetworkAccess.Internet) 
            { lblStatus.Text = "Conectado"; 
            }else { lblStatus.Text = "SIN Internet"; }
        }

        private async void ObtenerUbica()
        {
            try
            {
                var location = await Geolocation.GetLastKnownLocationAsync();
                if (location == null)
                {
                    location = await Geolocation.GetLocationAsync(new GeolocationRequest
                    {
                        DesiredAccuracy = GeolocationAccuracy.Medium,
                        Timeout = TimeSpan.FromSeconds(30)
                    });
                }
                if (location == null)
                {
                    lblStatus.Text = "No hay GPS";
                }
                else
                {
                    lblStatus.Text = $"{location.Latitude}{location.Longitude}";
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Algo sale mal :{ex.Message}");
            }
        }

    }
}
