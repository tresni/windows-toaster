# Do I want a new toaster? #

I hated that there was no easy way to support both [Snarl](http://www.fullphat.net) & [Growl for Windows](http://www.growlforwindows.com).  It seemed to be a one or the other, and what if I want toast notifications for my application and the user doesn't have either of them installed?

The idea of Toaster is to implement Toast notifications using either application if available, and fall back to notifications that will play nice with other applications doing their own toast messages and supporting the [Coordinating Toast Pop-ups Between Multiple Clients](http://msdn.microsoft.com/en-us/library/ms632289(VS.85).aspx) documentation.


# A Simple Implementation #
```
Toaster.Toaster toaster = new Toaster.Toaster("A New Toaster");

Toaster.Bread wheat = new Toaster.Bread("Fresh Wheat Toast");
toaster.Register(new Bread[] { wheat });

toaster.Toast(wheat, "Yummy!", "I <3 wheat toast with butter and honey!");
```

## In Steps ##
  1. Add a reference to Toaster.dll in your project
  1. Create a new Toaster object by using one of the constructors available
```
Toaster.Toaster toaster = new Toaster.Toaster("A New Toaster");
```
  1. Register the type of Bread you want
```
Toaster.Bread wheat = new Toaster.Bread("Fresh Wheat Toast");
toaster.Register(new Bread[] { wheat });
```
  1. Pop your toast
```
toaster.Toast(wheat, "Yummy!", "I <3 wheat toast with butter and honey!");
```