# MAUI4Hybrid
A simple solution to run your HTML5 program inside MAUI app which support cross platform running on multiple major OS like Windows, Android and iOS. This project has been build for any HTML5 developer but want to run their project as a mobile application.

# Why MAUI4Hybrid
I use Cordova a lot few years ago, but since the cordova updates, most of the core plugin has been removed. And to write those plugin, you will need dual-language experts, in Kotlin/Java for Android and Swift/Objective-C for iOS. 

So I developed a simple HTTP server base on almighty .NET framework and magical nuget packages, I succeedingly run our telecommunication app (which include WebRTC, Contacts, Local Storage, Location and Notification) in the app which seems easier than cordova plugin implementation.

# How to use?
There are 3 folders in this directory,
1. mauiapp - A MAUI App with a single project
2. maui-seperate-app - A Maui App with multiple platform project
3. class-library - A portable class library to run the HTTP server anywhere in .net C# project.

For number 1 & 2, you can choose either one, copy/paste and rename to you project. Paste all codes inside wwww folders and set all your content to `Embedded Resource`.

# Different to Cordova?
In cordova project, the www is stored as app's assets which it took few second for hacker to extract our HTML code. But in this MAUI4Hybrid, all codes are stored as Embedded Resource which it might took sometime to extract our site from the application.

And since the www stored as Embedded Resource, so we only need a single www folder in MAUI project itself rather than put www folder on every platform (Android, iOS etc.). 
