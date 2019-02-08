# unity_timed_type
A simple implementation of timed type entry for UnityEngine.UI.Text fields.

PURPOSE:

Editor configurable script to update a Unity UI Text field with a delay between letters. Meant to give the appearance of typing.


FEATURES:

-Array based text entries. You don't have to have just one.

-Editor configurable continue inputs.

-Continue inputs will skip to the end of a text sequence if already printing.

-Editor configurable delay between letter outputs

-Optional editor configurable random delay to give the appearance of someone physically typing

-Option to clear between text sequences or append.


USE:

Grab the TimedTextEntry.cs file and put it somewhere in your project. Add the component to a game object. Drag the target text field and configure the rest of the component in the Unity Editor. All very standard stuff.


NOTES:

It really isn't necessary to clone or download the whole project unless you'd like to make changes or want to check out the included sample scene.


NOT IMPLEMENTED:

-Auto exansion and scroll of text and scroll view RectTransforms in sample scene.
