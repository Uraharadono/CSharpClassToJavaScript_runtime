
# CSharp (C#) class(es) to various JavaScript classes functions

This project in it's true form should contain .Net class library which will handle conversion of C# classes to JavaScript classes:
* Vanilla
* ECMA 6
* ECMA 6 with knockout
(for now.)

I will be actively working on creating user interface for better user experience, but will keep functionalities separately so they can be used independently for other purposes.


## Disclaimer: 

This project was technically inspired by [this repo](https://github.com/castle-it/sharp2Js), but with wish to improve it, make it more generic and give it wider range of purposes. 
A lot of stuff was modified heavily to suit my needs, and will probably be changed (actually mostly is at the moment) or removed completely.

## Details
Reflection, reflection everywhere. 
I started to implement this idea by parsing content of the file, but I found it very error prone, or just bad solution overall.
You can take a look "younger brother" of this project [here](https://github.com/Uraharadono/CSharpClassToJavaScript_runtime) (it surprisingly still does its purpose nicely).

## TL;DR
User interface with file picker. When you pick file ( C# class) it will auto generate one of JavaScript classes for you.

