# Rules for GUI

## Subdirectories

### Elements

For re-usable elements. Mostly intended for extensions.

Should be named `UI{Describe Element}.cs`

### Game

UI elements broken down by the game elements they represent.

Should be named `UI{component name}{game object name}.cs`


### Screen

Layout of elements in viewport.


## 

Should call down tree, 

Use signals to call back up.