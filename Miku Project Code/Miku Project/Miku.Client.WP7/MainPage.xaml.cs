using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Notification;
using System.Diagnostics;
using System.Windows.Threading;
using System.Windows.Media.Imaging;
using System.IO;
using System.Xml.Linq;
using System.IO.IsolatedStorage;
using System.Collections.ObjectModel;

namespace PushNotifications
{
    public partial class MainPage : PhoneApplicationPage
    {
        private HttpNotificationChannel httpChannel;
        const string channelName = "RemoteConnectChannel";
        const string fileName = "PushNotificationsSettings.dat";
        const int pushConnectTimeout = 30;

        // Constructor
        public MainPage()
        {
            InitializeComponent();
            if (!TryFindChannel())
                DoConnect();
        }

        #region Tracing and Status Updates
        private void UpdateStatus(string message)
        {
            txtStatus.Text = message;
        }

        #endregion

        #region Misc logic
        private void DoConnect()
        {
            try
            {
                //First, try to pick up existing channel
                httpChannel = HttpNotificationChannel.Find(channelName);

                if (null != httpChannel)
                {

                    SubscribeToChannelEvents();

                    
                    SubscribeToService();

                    
                    SubscribeToNotifications();

                    Dispatcher.BeginInvoke(() => UpdateStatus("Channel recovered"));
                }
                else
                {
                    
                    //Create the channel
                    httpChannel = new HttpNotificationChannel(channelName);//, "HOLWeatherService");
                    

                    SubscribeToChannelEvents();

                    
                    httpChannel.Open();
                    Dispatcher.BeginInvoke(() => UpdateStatus("Channel open requested"));
                }
            }
            catch (Exception ex)
            {
                Dispatcher.BeginInvoke(() => UpdateStatus("Channel error: " + ex.Message));
            }
        }

        private void ParseRAWPayload(Stream e, out string weather, out string location, out string temperature)
        {
            XDocument document;

            using (var reader = new StreamReader(e))
            {
                string payload = reader.ReadToEnd().Replace('\0',
                  ' ');
                document = XDocument.Parse(payload);
            }

            location = (from c in document.Descendants("WeatherUpdate")
                        select c.Element("Location").Value).FirstOrDefault();
            

            temperature = (from c in document.Descendants("WeatherUpdate")
                           select c.Element("Temperature").Value).FirstOrDefault();
            

            weather = (from c in document.Descendants("WeatherUpdate")
                       select c.Element("WeatherType").Value).FirstOrDefault();
        }
        #endregion

        #region Subscriptions
        private void SubscribeToChannelEvents()
        {
            //Register to UriUpdated event - occurs when channel successfully opens
            httpChannel.ChannelUriUpdated += new EventHandler<NotificationChannelUriEventArgs>(httpChannel_ChannelUriUpdated);

            //Subscribed to Raw Notification
            httpChannel.HttpNotificationReceived += new EventHandler<HttpNotificationEventArgs>(httpChannel_HttpNotificationReceived);

            //general error handling for push channel
            httpChannel.ErrorOccurred += new EventHandler<NotificationChannelErrorEventArgs>(httpChannel_ExceptionOccurred);

            //subscrive to toast notification when running app    
            httpChannel.ShellToastNotificationReceived += new EventHandler<NotificationEventArgs>(httpChannel_ShellToastNotificationReceived);
        }

        private void SubscribeToService()
        {
            //Hardcode for solution - need to be updated in case the REST WCF service address change
            string baseUri = "http://localhost:8000/RegirstatorService/Register?userName={0}&uri={1}";
            string theUri = String.Format(baseUri, "gh",httpChannel.ChannelUri.ToString());
            WebClient client = new WebClient();
            client.DownloadStringCompleted += (s, e) =>
            {
                if (null == e.Error)
                    Dispatcher.BeginInvoke(() => UpdateStatus("Registration succeeded"));
                else
                    Dispatcher.BeginInvoke(() => UpdateStatus("Registration failed: " + e.Error.Message));
            };
            client.DownloadStringAsync(new Uri(theUri));
        }
        private void SubscribeToNotifications()
        {
            //////////////////////////////////////////
            // Bind to Toast Notification 
            //////////////////////////////////////////
            try
            {
                if (httpChannel.IsShellToastBound == true)
                {
                    
                }
                else
                {
                    
                    httpChannel.BindToShellToast();
                }
            }
            catch (Exception ex)
            {
                // handle error here
            }

            //////////////////////////////////////////
            // Bind to Tile Notification 
            //////////////////////////////////////////
            try
            {
                if (httpChannel.IsShellTileBound == true)
                {
                    
                }
                else
                {
                    

                    // you can register the phone application to receive tile images from remote servers [this is optional]
                    Collection<Uri> uris = new Collection<Uri>();
                    uris.Add(new Uri("http://jquery.andreaseberhard.de/pngFix/pngtest.png"));

                    httpChannel.BindToShellTile(uris);
                }
            }
            catch (Exception ex)
            {
                //handle error here
            }
        }

        #endregion

        #region Channel event handlers
        void httpChannel_ChannelUriUpdated(object sender, NotificationChannelUriEventArgs e)
        {
            
            Dispatcher.BeginInvoke(() => SaveChannelInfo());

            
            SubscribeToService();
            SubscribeToNotifications();

            Dispatcher.BeginInvoke(() => UpdateStatus("Channel created successfully"));
        }

        void httpChannel_ExceptionOccurred(object sender, NotificationChannelErrorEventArgs e)
        {
            Dispatcher.BeginInvoke(() => UpdateStatus(e.ErrorType + " occurred: " + e.Message));
        }

        void httpChannel_HttpNotificationReceived(object sender, HttpNotificationEventArgs e)
        {

            
            

            string weather, location, temperature;
            ParseRAWPayload(e.Notification.Body, out weather, out location, out temperature);

            //Dispatcher.BeginInvoke(() => this.textBlockListTitle.Text = location);
            //Dispatcher.BeginInvoke(() => this.txtTemperature.Text = temperature);
            //Dispatcher.BeginInvoke(() => this.imgWeatherConditions.Source = new BitmapImage(new Uri(@"Images/" + weather + ".png", UriKind.Relative)));
            

            
        }

        void httpChannel_ShellToastNotificationReceived(object sender, NotificationEventArgs e)
        {
            
            
            foreach (var key in e.Collection.Keys)
            {
                string msg = e.Collection[key];

                
                Dispatcher.BeginInvoke(() => UpdateStatus("Toast/Tile message: " + msg));
            }

            
        }
        #endregion

        #region Loading/Saving Channel Info
        private bool TryFindChannel()
        {
            bool bRes = false;

            
            using (IsolatedStorageFile isf = IsolatedStorageFile.GetUserStoreForApplication())
            {

                if (isf.FileExists(fileName))
                {

                    using (IsolatedStorageFileStream isfs = new IsolatedStorageFileStream(fileName, FileMode.Open, isf))
                    {
                        using (StreamReader sr = new StreamReader(isfs))
                        {
                            string uri = sr.ReadLine();

                            httpChannel = HttpNotificationChannel.Find(channelName);

                            if (null != httpChannel)
                            {
                                if (httpChannel.ChannelUri.ToString() == uri)
                                {
                                    Dispatcher.BeginInvoke(() => UpdateStatus("Channel retrieved"));
                                    SubscribeToChannelEvents();
                                    SubscribeToService();
                                    bRes = true;
                                }

                                sr.Close();
                            }
                        }
                    }
                }
                else
                {
                    //Trace("Channel data not found");
                }
            }

            return bRes;
        }

        private void SaveChannelInfo()
        {
            
            using (IsolatedStorageFile isf = IsolatedStorageFile.GetUserStoreForApplication())
            {
                
                using (IsolatedStorageFileStream isfs = new IsolatedStorageFileStream(fileName, FileMode.Create, isf))
                {
                    using (StreamWriter sw = new StreamWriter(isfs))
                    {
                        
                        sw.WriteLine(httpChannel.ChannelUri.ToString());
                        sw.Close();
                        
                    }
                }
            }
        }
        #endregion
    }
}