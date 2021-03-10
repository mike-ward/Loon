# Loon

Loon is a cross platform desktop twitter client. It's minimal design mimics gadget style applications. I use it to monitor current events and announcements in the developer community.

![screen shot](https://i.imgur.com/kqitYEa.png)

### Work in Progress

This is a port of my Windows desktop client [Tweetz](https://github.com/mike-ward/tweetz) using the [Avalonia](http://avaloniaui.net/) framework. 

It's a work in progress and many, many things don't work. However, you can actually login to twitter and get the home timeline with images.

I've run this client successfully on both Windows and Linux. I don't own a Mac.

### How to Build

`dotnet build ./src/Loon.sln`

Visual Studio:
- Install the [Avalonia extension](https://marketplace.visualstudio.com/items?itemName=AvaloniaTeam.AvaloniaforVisualStudio)
- Clone, load, F5.


Linux and Mac OS:
- Rider IDE supports Avalonia development