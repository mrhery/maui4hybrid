# MAUI4Hybrid
A simple solution to run your HTML5 program inside MAUI app which support cross platform running on multiple major OS like Windows, Android and iOS. This project has been build for any HTML5 developer but want to run their project as a mobile application.

# Why MAUI4Hybrid
I use Cordova a lot few years ago, but since the cordova updates, most of the core plugin has been removed. And to write those plugin, you will need dual-language experts, in Kotlin/Java for Android and Swift/Objective-C for iOS. 

So I developed a simple HTTP server base on almighty .NET framework and magical nuget packages, I succeedingly run our telecommunication app (which include WebRTC, Contacts, Local Storage, Local DB SQLite, Location and Notification) in the app which seems easier than cordova plugin implementation.

# How to use?
There are 3 folders in this directory,
1. mauiapp - A MAUI App with a single project
2. maui-seperate-app - A Maui App with multiple platform project
3. class-library - A portable class library to run the HTTP server anywhere in .net C# project.

For number 1 & 2, you can choose either one, copy/paste and rename to you project. Paste all codes inside wwww folders and set all your content to `Embedded Resource`.

# Different to Cordova?
In cordova project, the www is stored as app's assets which it took few second for hacker to extract our HTML code. But in this MAUI4Hybrid, all codes are stored as Embedded Resource which it might took sometime to extract our site from the application.

And since the www stored as Embedded Resource, so we only need a single www folder in MAUI project itself rather than put www folder on every platform (Android, iOS etc.).

#  Usage Warning
1. Always make sure all files under www folder are set it build action to "Embedded Resource".
2. Every filename cannot has dual dot (.) like `jquery.min.js`, `aaa.bbb.html`. Must always change to single dot like `jquery.js`, `aaabbb.html` and so on.
3. Filename in www folder must cannot contains "-" character.
4. To run the apps, in android must add `android:usesCleartextTraffic="true"` in `<application ...>` tag.

# Usage

## 1. Start the HTTP Server
To start the the HTTP server, you can call the `init` and the `start` methods in one `Task.Run()` wrapper:

```
Task.Run(() => {
	M4HttpServer.Init();
	M4HttpServer.Start();
});
```
You can specify a custom port number to be use (default port is 45100):
```
...
M4HttpServer.Init(10000);
...
```

## 2. Parse data from C# to Web File
If you have variable with values that need to parse to HTML or JS (or any files that in `www` folder), you can use the static `Data` variable (`Dictionary<string, string>`):

```
...
M4HttpServer.Data.Add("key", "value");
M4HttpServer.Data.Add("name", "hery");
M4HttpServer.Data.Add("encrypted_text", "some-encrypted-text");

// or

M4HttpServer.Data = new Dictionary<string, string>();
...
```

To call it in HTML or JS file:
```
<!-- index.html -->
...
<h1>Welcome, {{name}}!</h1>
...

<script>
var api_key = "{{key}}";
...
var encrypted_text = "{{encrypted_text}}";
</script>

```

## 3. HTML include
If you need to include any files inside a HTML file, you may use `[[include:path-here]]`:
```
<!-- index.html -->
...
<body>
	[[inclue:widgets/header.html]]
	
	<div class="main-content">
		Some content here
	</div>
	
	[[inclue:widgets/footer.html]]
</body>
...

```