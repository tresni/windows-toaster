using System;
using System.Collections.Generic;
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

        public bool Register(Bread[] notifications)
        {
            if (!this.IsToasterWorking()) return false;
            this.registered = true;

            switch (this.WhichToaster())
            {
                case ToasterType.TOASTER_SNARL:
                    M_RESULT res = SnarlConnector.RegisterConfig(IntPtr.Zero, this.appName, 0);

                    foreach (Bread notification in notifications)
                    {
                        SnarlConnector.RegisterAlert(this.appName, notification.ToString());
                    }
                    this.registered = (res == M_RESULT.M_OK);
                    break;
                case ToasterType.TOASTER_GROWL:
                    Growl.Connector.NotificationType[] types = new NotificationType[notifications.Length];
                    for (int i = 0; i < notifications.Length; i++)
                    {
                        types[i] = notifications[i].ToGrowlNotificationType();
                    }
                    growlConnector.Register(new Growl.Connector.Application(this.appName), types);
                    break;
                case ToasterType.TOASTER_CUSTOM:
                    break;
                default:
                    this.registered = false;
                    break;
            }
            return this.registered;
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
                    ToastForm toast = new ToastForm(title, text, (System.Drawing.Image)icon);
                    toast.Show();
                    if (hasMutex)
                        ToastMutex.ReleaseMutex();
                    return (long)toast.Handle;
                default:
                    return 0;
            }
        }
    }

    public class Bread
    {
        string name;
        public string Name { get { return name; } }
        public Bread(string name)
        {
            this.name = name;
        }

        public override string ToString()
        {
            return this.Name;
        }

        protected internal Growl.Connector.NotificationType ToGrowlNotificationType()
        {
            return new Growl.Connector.NotificationType(this.Name);
        }
    }
}
