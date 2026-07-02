# Exfal
An XNA abstraction layer library that adds object oriented implementation of basic rendering system, input handling, time management, and even coroutines to simplify 
creation of graphical applications and games when using XNA-compatible frameworks since it is based on Microsoft.Xna.Framework namespace.

## Features

- Object-oriented rendering system
- Multiple cameras
- Input handling
- Time management
- Coroutine system (StepTask)
- Extensions for Vector2 and numerics
- Works with MonoGame and other XNA-compatible frameworks

## Installation
1. Download the latest release from https://github.com/sonesoul/Exfal/releases  
2. Add Exfal.dll to your XNA or MonoGame project as a Project Reference (make sure your project is in .NET 6.0 or higher)
3. Add usings
```csharp
using Exfal;                 // StepTask, Time, Asset ...
using Exfal.Drawing;         // Drawer, Camera, DrawContext ...
using Exfal.InputHandling;   // Input, Key
using Exfal.Extensions;      // Vector2Extensions, NumericExtensions ...
```
4. You're done!

> [!TIP]
> Copy Exfal.dll into your project directory and commit it to source control.
This ensures the exact library version is tracked with the project and prevents dependency issues across machines.

## Quick Start
No one wants to read through tones of text just to get started. Here's a quick start guide!

### 1. Initialize
Initialize `Drawer` and `Asset`.

```csharp
private Point WindowSize { get; } = new(512, 512);

private GraphicsDeviceManager _graphics;
private SpriteBatch _spriteBatch;
private Drawer _drawer; 

public Game1() 
{
    _graphics = new GraphicsDeviceManager(this);
    Content.RootDirectory = "Content";
    Asset.Content = Content;
}

protected override void LoadContent()
{
    _spriteBatch = new SpriteBatch(GraphicsDevice);
    _drawer = new(_spriteBatch, _graphics, WindowSize);

    base.LoadContent();
}
```
### 2. Register
Register rendering and input handlers.

```csharp
bool _pressed = false;

protected override void Initialize()
{
    _drawer.OutputCamera.Register(DrawRect);
    Input.KeyPressed += k => _pressed = true;
    Input.KeyReleased += k => _pressed = false;

    base.Initialize();
}
void DrawRect(DrawContext draw)
{    
    draw.Rectangle(
      new Rectangle(0, 0, 100, 100),
      _pressed ? Color.Green : Color.Red);
}
```

### 3. Update
Update all the essential systems (Time, Input, StepTask).

```csharp 
protected override void Update(GameTime gameTime)
{
    Time.Update(gameTime);
    Input.Update(gameTime);
    StepTask.Update();

    // your logic here

    base.Update(gameTime);
}
```
### 4. Draw
Render the frame.

```csharp
protected override void Draw(GameTime gameTime) 
{
    _drawer.Draw();
    base.Draw(gameTime);
}
```
You're done! This code draws a rectangle on the top left corner. It's red when nothing is pressed and is green when anything is pressed.


# How To Use?
Depending on your needs you can use specified modules of xfal. Basic modules are: 

[Rendering](#rendering)  
[StepTask (coroutine)](#steptask)  
[Time](#time)  
[Input](#input)  

Mostly they are all separate. If you don't need a module - just ignore it unless otherwise expected.

# Rendering
To draw something, create a proper function.
```csharp
void Draw(DrawContext draw)
{
    // draw a rectangle on 0,0 position with width and height 10
    draw.Rectangle(new Rectangle(0, 0, 10, 10), Color.White);
} 
```
> [!NOTE]
> `DrawContext` has different kinds of methods for drawing. You can still use `SpriteBatch.Draw(...)`, but **DO NOT** use `SpriteBatch.Begin(...)` or `SpriteBatch.End(...)`.

For `draw.Texture(...)` and some others there is the `DrawOptions` structure in arguments. It is used to combine several parameters in one object, so consider it as just a simpler way to give arguments: 

```csharp
DrawOptions opts = new DrawOptions() { 
    position = new Vector2(x, y),
    origin = new Vector2(x, y),
    scale = new Vector2(width, height),
    color = Color.White,
    rotationRad = Deg2Rad(45) 
};

//you can store it as long as you need and change at any time
opts.position = Vector2.Zero;

drawer.DrawTexture(texture, opts);
```

Then, you'll have to register the method:
```csharp
//registering for rendering
//0 - default camera
drawer.Cameras[0].Register(Draw);
```
You can also use more than one camera:
```csharp
drawer.Cameras.Add(1, new Camera(renderSource, new Point(1920, 1080)));
drawer.Cameras[1].Register(Draw);
```
> [!NOTE]
> Each camera handles only its own registered draws.

Call `drawer.Draw()` in your draw-cycle. 

# StepTask 
StepTask is a kind of coroutine. It helps with organizing independent running methods: interpolations, delays, etc. 
```csharp
IEnumerator MyCoroutine() 
{ 
    yield return StepTask.Yields.WaitForSeconds(3);
}

StepTask task = StepTask.Run(MyCoroutine(3)); 
```
`StepTask.Yields` has some methods to manage your waiting time in the coroutine.
> [!IMPORTANT]
> `StepTask.Yields.WaitForSeconds(...)` and `StepTask.Yields.WaitForRealSeconds(...)` depend on `Exfal.Time.Delta` and `Exfal.Time.RealDelta`, so if you're using them, `Exfal.Time` should be updated before

It is easily extendable by making an extension class and overriding YieldInstruction.
```csharp
//classical extensions class
static class YieldInstructionExtensions 
{ 
    IEnumerator MyExtension(this YieldInstruction _) { yield return null; }
}

//then you can call it
yield return StepTask.Yields.MyExtension();
```
To make all your coroutines work, call `StepTask.Update()` in a main loop. Each update makes only **one call** in all coroutines straight to the next `yield return`.
By calling `StepTask.Run(...)` the coroutine starts automatically, but you have full control of how and when your coroutines run and stop. 
```csharp
task.Start();    // starts the coroutine from the beginning
task.Break();    // finishes coroutine, no event invocation
task.Complete(); // finishes coroutine and invokes Completed event
```
By creating it from the constructor, you'll have to call `Start()` yourself.

# Time
For managing time, there is `Exfal.Time` or just `Time` class. It's static. Call `Time.Update(...)` before any time-dependant updates. 
```csharp
var delta = Time.Delta;       // depends on Time.TimeScale
var rdelta = Time.RealDelta;  // doesn't depend on Time.TimeScale
var fdelta = Time.FixedDelta; // always the same. Equals 1/60 by default

Time.TimeScale = 0.5f;       // time (Time.Delta) is 2 times slower
Time.FixedDelta = 1.0f / 30; // making bigger steps, can cause less accuracy in some physics engines 
```

# Input
Exfal has `Input` class which combines mouse handling and keyboard handling. It uses `Exfal.Key` enum, which also includes both keyboard and mouse buttons. Needs to be updated through `Input.Update()` to work properly.
```csharp
if (Input.IsKeyDown(Key.MouseLeft))
  DoSomething(); 

//or you can use events
void HandleKey(Key k)
{
    if (k == MouseLeft)
        DoSomething();
}

Input.KeyPressed += HandleKey;
Input.KeyReleased += HandleKey;
``` 
> [!NOTE]
> There is `Input.MousePosition` property. It returns position on the screen. Since cameras have different resolution, you'll have to use `drawer.ScreenToWorldPoint(screenPos, camera)` to get position based on the viewport of the camera.

# Tips
Rendering module makes resolution of the application static. If you want to use this ✨ _fancy_ ✨ rendering system but don't want to deal with static resolution, there is an easy fix: 
```csharp
Window.ClientSizeChanged += (obj, args) =>
{
    drawer.Canvas.Size = Window.ClientBounds.Size;
    drawer.OutputCamera.Size = Window.ClientBounds.Size;
};
```
> [!NOTE]
> If you have multiple cameras, you'll have to update them all.
---

Each `Camera` has its own properties for drawing. You can make its background transparent which can be really useful when you have a camera used only for UI rendering.
```csharp
drawer.GetCamera(UI_CAMERA_ORDER).BackgroundColor = Color.Transparent; 
```
---

When `Graphics.Viewport` changes, `Drawer` automatically adjusts output (in `DrawCanvas(...)` method) using specific scaling function - and you can make and use your own one!
This could be done the same way as mentioned in `YieldInstruction overriding`. 
```csharp
static class RectScalerExtensions
{
    public static Rectangle MyScale(this RectScaler _, in Point source, in Rectangle target)
    {
        //do some calculations here
    }
}

//set it
drawer.ScaleFunc = Drawer.OutputScaler.MyScale;
```
---
Exfal has LOTS of extensions. The most detailed ones are extensions for Vector2. 
```csharp
//they are all returning values and DO NOT change the vector

v.Floored();               // Math.Floor(...) on both
v.Ceiled();                // Math.Ceiling(...) on both

v.Abs();                   // Math.Abs(...) on both
v.AbsX();                  // Math.Abs(...) on X
v.AbsY();                  // Math.Abs(...) on Y
v.Truncated();             // (int) on both
v.Rounded(digits: 0);      // MathF.Round(...) on both

v.TakeX();                 // set v.Y to 0 
v.TakeY();                 // set v.X to 0

v.WithX(123);             // set X to 123
v.WithY(123);             // set Y to 123
v.WithX(x => x + 123);    // add 123 to X
v.WithY(y => y + 123);    // add 123 to Y

v.Min();                   // minimum value between X and Y
v.Max();                   // maximum value between X and Y

v.Perpendicular();         // set X to Y and Y to -X

v.Normalized();            // vector in range of 1
v.Clamped(0, 123);         // limit X and Y between 0 and 123

v.Dot(v2)                  // dot product
v.Cross(v2);               // cross product
v.DistanceTo(v2);          // distance between vectors
v.RotatedAround(v2, r);    // rotated around v2 by r radians

// supported operators: >, >=, <, <=, ==, !=
bool anyAxisLess = vec.Any() < 123; //X < 123 || Y < 123 

// also supports +, -, *, /
bool bothAxesEqual = v.Both() == 123; // X == 123 && Y == 123
```
