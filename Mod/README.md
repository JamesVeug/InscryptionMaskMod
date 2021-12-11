# Mask API
API to support loading custom masks into the game.

If you want to add your own mask. 

## But i want to make my own mask!
Put a .custommask file in your mod directory and point the `TextureOverridePath` to local directory.
Look at the .custommask files in a text editor as examples. 

```json
{
"MaskType": "Trapper",  // Enum type (Prospector/Woodcarver/Angler/Trapper/Trader/Doctor)
"MaskName": "Emoji",  // Name that the user wants to call this mask
"TextureOverridePath": "Masks/angryface.png", // Optional Texture that we want to override the model with
"BundlePath": "Bundles/custommask", // What Model should this mask use?
"PrefabName": "CustomMask" // Name of the GameObject within the bundle that the mask will use
}
```

## Can I contribute to this mod?
Yes. Either:
1. Make a Pull request on github
2. @JamesGames on the inscription discord with the image and ask him nicely to add it

## I want to put my own 3d model in
1. Make your custom model
2. Load it into unity
3. Make an asset bundle
4. Put the asset bundle in your mod folder (see MaskAPI/Masks/custommask)
5. Change your .custommask `BundlePath` to point to your new bundle
6. Change your .custommask `PrefabName` to be the same as the prefab name that you made in Unity