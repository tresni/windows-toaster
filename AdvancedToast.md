# Introduction #

Toaster supports some advanced settings such as custom icons for the application, notification types, or individual notifications, as well as the ability to set the timeout or sticky option where that's supported.

You don't have to worry about the correct image type or how Growl or Snarl wants them.  Thanks to the wonders of .Net, Toaster support all major image types and will convert them into the PNG format that both Growl and Snarl can use on the fly.

# Details #

## Icons ##

Icons are wrapped in a `ToasterIcon` class which is hidden from public view.  All you need is either a System.Drawing.Image resource or a path to a valid image file.  With either of those you can add an icon to an application, notification type (Bread), or individual notification (Toast).

### Setting the Icon for your Application ###
```
Toaster.Toaster toaster = new Toaster.Toaster("A New Toaster", Properties.Resources.Icon1.ToBitmap());

Toaster.Toaster toaster = new Toaster.Toaster("A New Toaster", @"C:\image.jpg");
```

### Setting the Icon for a Bread ###
```
Toaster.Bread wheat = new Toaster.Bread("Fresh Wheat Toast", Properties.Resources.Icon1.ToBitmap());

Toaster.Bread wheat = new Toaster.Bread("Fresh Wheat Toast", @"C:\image.jpg");
```

### Setting the Icon for a Toast ###
```
toaster.Toast(wheat, "Yummy!", "I <3 wheat toast with butter and honey!", Properties.Resources.Icon1.ToBitmap());

toaster.Toast(wheat, "Yummy!", "I <3 wheat toast with butter and honey!", @"C:\image.jpg");
```

## Timeouts ##
Setting the "doneness" of your toast allows you to determine how long the notification will remain on screen.  Available options are:
| `ToastDoneness.TOAST_LIGHT`  | 3 seconds |
|:-----------------------------|:----------|
| `ToastDoneness.TOAST_MEDIUM` | 5 seconds |
| `ToastDoneness.TOAST_DARK`   | 7 seconds |
| `ToastDoneness.TOAST_BURNT`  | sticky/until dismissed by the user |

### A note about Growl ###
Growl does not allow you to set the timeout per notification type or individual notification.  This is controlled by the Growl server and based on user configuration.  However, you can use `ToastDoneness.TOAST_BURNT` for a sticky notification that is only dismissed when the user clicks on it.

### Setting Doneness ###
Doneness is configured on your Bread object.  TOAST\_MEDIUM is the default.
```
Toaster.Bread wheat = new Toaster.Bread("Fresh Wheat Toast", Toaster.ToastDoneness.TOAST_LIGHT);
```

This can be combined with the custom icon as follows:
```
Toaster.Bread wheat = new Toaster.Bread("Fresh Wheat Toast", Properties.Resources.Icon1.ToBitmap(), Toaster.ToastDoneness.TOAST_DARK);
```

Currently this can not be configured on an individual Toast but that will likely change.