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
        GrowlConnector growlConnector = new GrowlConnector();
        ToasterIcon Icon;

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
            this.Icon = new ToasterIcon(icon);
        }

        public Toaster(string AppName, string password)
            : this(AppName)
        {
            growlConnector.Password = password;
        }

        public Toaster(string AppName, string Password, System.Drawing.Image icon)
            : this(AppName, Password)
        {
            this.Icon = new ToasterIcon(icon);
        }

        public Toaster(string AppName, string Password, string iconPath)
            : this(AppName, Password)
        {
            this.Icon = new ToasterIcon(iconPath);
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
                    M_RESULT res = SnarlConnector.RegisterConfig(IntPtr.Zero, this.appName, 0, this.Icon.ToString());

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
                    Growl.Connector.Application app = new Growl.Connector.Application(this.appName);
                    if (this.Icon != null)
                        app.Icon = this.Icon.ToString();
                    growlConnector.Register(app, types);
                    break;
                case ToasterType.TOASTER_CUSTOM:
                    break;
                default:
                    this.registered = false;
                    break;
            }
            return this.registered;
        }
        
        [Obsolete("Calling Toast with string 'type' is superceded by using Bread 'type'")]
        public long Toast(string type, string title, string text)
        {
            return Toast(new Bread(type), title, text);
        }

        public long Toast(Bread type, string title, string text)
        {
            return Toast(type, title, text, type.Icon);
        }

        public long Toast(Bread type, string title, string text, System.Drawing.Image icon)
        {
            return Toast(type, title, text, new ToasterIcon(icon));
        }

        public long Toast(Bread type, string title, string text, string iconFilename)
        {
            return Toast(type, title, text, new ToasterIcon(iconFilename));
        }

        long Toast(Bread type, string title, string text, ToasterIcon icon)
        {
            long id;

            if (!this.IsToasterWorking() || !this.registered) return 0;
            if (icon == null) icon = new ToasterIcon();

            switch (this.WhichToaster())
            {
                case ToasterType.TOASTER_GROWL:
                    id = DateTime.Now.Ticks;
                    growlConnector.Notify(new Notification(this.appName, type.ToString(), id.ToString(), title, text, icon.ToString(), (type.Doneness == ToastDoneness.TOAST_BURNT ? true : false), Priority.Normal, null));
                    return id;
                case ToasterType.TOASTER_SNARL:
                    id = SnarlConnector.ShowMessageEx(type.ToString(), title, text, (int)type.Doneness, icon.ToString(), IntPtr.Zero, 0, "");
                    return id;
                case ToasterType.TOASTER_CUSTOM:
                    ToastForm toast = new ToastForm(title, text, icon.Icon, type.Doneness);
                    return (long)toast.Handle;
                default:
                    return 0;
            }
        }
    }

    public class Bread
    {
        string name;
        internal ToasterIcon Icon;
        ToastDoneness duration = ToastDoneness.TOAST_MEDIUM;

        public string Name { get { return name; } }
        public ToastDoneness Doneness
        {
            set
            {
                duration = value;
            }
            get { return duration; }
        }
        
        public Bread(string name)
        {
            this.name = name;
        }

        public Bread(string name, string iconPath)
            : this(name)
        {
            this.Icon = new ToasterIcon(iconPath);
        }

        public Bread(string name, System.Drawing.Image icon)
            : this(name)
        {
            this.Icon = new ToasterIcon(icon);
        }

        public Bread(string name, ToastDoneness duration)
            : this(name)
        {
            this.duration = duration;
        }

        public Bread(string name, string iconPath, ToastDoneness duration)
            : this(name, iconPath)
        {
            this.duration = duration;
        }

        public Bread(string name, System.Drawing.Image icon, ToastDoneness duration)
            : this(name, icon)
        {
            this.duration = duration;
        }


        public override string ToString()
        {
            return this.Name;
        }

        protected internal Growl.Connector.NotificationType ToGrowlNotificationType()
        {
            Growl.Connector.NotificationType nt = new Growl.Connector.NotificationType(this.Name);
            if (this.Icon != null && this.Icon.HasIcon())
                nt.Icon = this.Icon.ToString();
            return nt;
        }
    }

    internal class ToasterIcon
    {
        System.Drawing.Image icon = null;
        string iconPath = null;

        public ToasterIcon(string iconPath)
        {
            this.IconFilename = iconPath;
        }
        public ToasterIcon(System.Drawing.Image icon)
        {
            this.Icon = icon;
        }
        public ToasterIcon() { }

        ~ToasterIcon()
        {
            if (iconPath != null)
                System.IO.File.Delete(iconPath);
        }

        public System.Drawing.Image Icon
        {
            get { return icon; }
            set
            {
                icon = value;
                if (value != null)
                {
                    // Clean up
                    if (iconPath != null)
                        System.IO.File.Delete(iconPath);

                    //iconPath = System.IO.Path.GetTempFileName();
                    iconPath = System.IO.Path.GetTempPath() + Guid.NewGuid().ToString() + ".png";
                    // Snarl requires PNG
                    icon.Save(iconPath, System.Drawing.Imaging.ImageFormat.Png);
                }
                else
                    iconPath = null;
            }
        }
        public string IconFilename
        {
            get { return iconPath; }
            set
            {
                if (value != null)
                {
                    // This will make sure we always get a PNG
                    Icon = System.Drawing.Image.FromFile(value);
                }
                else
                {
                    icon = null;
                    iconPath = value;
                }
            }
        }

        public override string ToString()
        {
            return iconPath;
        }
        public bool HasIcon()
        {
            return icon != null;
        }
    }

    public enum ToastDoneness
    {
        TOAST_LIGHT = 3000,
        TOAST_MEDIUM = 5000,
        TOAST_DARK = 7000,
        TOAST_BURNT = 0
    }
}
