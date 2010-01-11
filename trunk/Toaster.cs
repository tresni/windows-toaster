using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Text;
using Growl.Connector;
using Growl.CoreLibrary;
using Snarl;


namespace Toaster
{
    public class Toaster
    {
        Mutex ToastMutex = new Mutex(false, "Microsoft.Messenger.ToastSemaphore");
        GrowlConnector growlConnector = new GrowlConnector();
        System.Drawing.Image icon = null;

        string appName = "";
        bool registered = false;
        public bool Registered { get { return registered; } }
        public bool UseCustom = true;

        public enum ToasterType {
            TOASTER_NONE,
            TOASTER_GROWL,
            TOASTER_SNARL,
            TOASTER_CUSTOM
        };

        public Toaster(string AppName)
        {
            this.appName = AppName;
            growlConnector.EncryptionAlgorithm = Cryptography.SymmetricAlgorithmType.AES;
        }

        public Toaster(string AppName, System.Drawing.Image icon)
            : this(AppName)
        {
            this.icon = icon;
        }

        public Toaster(string AppName, string password)
            : this(AppName)
        {
            growlConnector.Password = password;
        }

        public Toaster(string AppName, string Password, System.Drawing.Image icon)
            : this(AppName, Password)
        {
            this.icon = icon;
        }

        public bool IsToasterWorking() {
            return (growlConnector.IsGrowlRunning() || SnarlConnector.GetSnarlWindow() != IntPtr.Zero || this.UseCustom);
        }

        public ToasterType WhichToaster() {
            if (growlConnector.IsGrowlRunning())
                return ToasterType.TOASTER_GROWL;
            if (SnarlConnector.GetSnarlWindow() != IntPtr.Zero)
                return ToasterType.TOASTER_SNARL;
            if (this.UseCustom)
                return ToasterType.TOASTER_CUSTOM;
            return ToasterType.TOASTER_NONE;
        }

        public bool Register(ToasterNotificationType[] notifications)
        {
            if (!this.IsToasterWorking()) return false;
            this.registered = true;
            switch (this.WhichToaster())
            {
                case ToasterType.TOASTER_SNARL:
                    M_RESULT res = SnarlConnector.RegisterConfig(IntPtr.Zero, this.appName, 0);

                    foreach (ToasterNotificationType notification in notifications)
                    {
                        SnarlConnector.RegisterAlert(this.appName, notification.ToString());
                    }
                    return res == M_RESULT.M_OK;
                case ToasterType.TOASTER_GROWL:
                    growlConnector.Register(new Growl.Connector.Application(this.appName), (NotificationType[])notifications);
                    return true;
                case ToasterType.TOASTER_CUSTOM:
                    return true;
                default:
                    this.registered = false;
                    return false;
            }
        }

        public long Toast(string title, string text, string type)
        {
            if (!this.IsToasterWorking() || !this.registered) return 0;
            long id;
            switch (this.WhichToaster())
            {
                case ToasterType.TOASTER_GROWL:
                    id = DateTime.Now.Ticks;
                    growlConnector.Notify(new Notification(this.appName, type, id.ToString(), title, text));
                    return id;
                case ToasterType.TOASTER_SNARL:
                    id = SnarlConnector.ShowMessageEx(type, title, text, 0, "", IntPtr.Zero, 0, "");
                    return id;
                case ToasterType.TOASTER_CUSTOM:
                    bool hasMutex = ToastMutex.WaitOne(new TimeSpan(0, 0, 60));
                    new Form1(title, text, (System.Drawing.Image)icon).Show();
                    if (hasMutex)
                        ToastMutex.ReleaseMutex();
                    return 0;
                default:
                    return 0;
            }
        }
    }

    public class ToasterNotificationType : NotificationType
    {
        public ToasterNotificationType(string name) : base(name) { }
        public override string ToString()
        {
            return this.Name;
        }
    }
}
