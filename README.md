# Loon

Loon is a cross platform desktop twitter client. It's minimal design mimics gadget style applications. I use it to monitor current events and announcements in the developer community.

### Work in Progress

This is a port of my Windows desktop client [Tweetz](https://github.com/mike-ward/tweetz) using the [Avalonia](http://avaloniaui.net/) framework. 

It's a work in progress and many, many things don't work. However, you can actually login to twitter and get the home timeline with images.

Links and hash tags don't work. I'm waiting on the Avalonia team to implement a few more features (TextBlock inlines for one).

There's also the issue of creating a media player. The windows version relies on WPF's built-in functionality.

I've run this client successfully on both Windows and Linux. I don't own a Mac.

### Building

You'll need Visual Studio with the [Avalonia extension](https://marketplace.visualstudio.com/items?itemName=AvaloniaTeam.AvaloniaforVisualStudio) installed.

Clone the project, load it into Visual Studio, press F5.

