# AmplifyLayerCompiler

This is a proof of concept for Unity Amplify Shader Editor to create stackable texture layers.

A stackable layer is to set up a reusable function with all the logic for one super cool layer, and then stacking them. This is currently not possible with Amplify, because each function is global, so if a function e.g. imports a texture or uses a shader keyword, this is global for all usages of the function. Thus all layers will have the same textures and the same settings.

The solution is for each layer to get unique keywords and property names. so you set up e.g. a keyword "_ENABLELAYER", and the layer compiler replaces that with "_ENABLELAYER", "_ENABLELAYER1", "_ENABLELAYER2" etc.

After installing the example, right-click Test Layer Template and select Amplify Layer Compiler -> Compile To 4 Layers. Observe how these 4 layers are used in Outer Shader. And observe how AmplifyLayerCompilerGUI is able to control the settings for each layer.

This is a dumb little layered shader that just added the textures together.

And this is a proof of concept. The correct solution would be for Amplify to either compile these out automatically, or even better, if each usage of a function could get a number that can then be appended to all properties or keywords used within the function.
