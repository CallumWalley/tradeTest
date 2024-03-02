# UI Design

## In this folder

### Elements
Primitives of commonly used elements to be extended

#### UIWindow
Moveable closeable, etc.
Should receive `EFrameUI` Signal

#### UIPanel (currently just panel container)

Should be self contained, but still neets to have `Update()` called upon it to update data.

#### UIAccordian
Moveable closeable, etc.

#### UIDropDown

#### UIList

### Screen
UI elements for main screen.

### Game
Implimentations of 'Elements' as found in game. Organised by parent.

#### Listables
Elements implimenting the 'listable' interface.


## Message passing

When going down (e.g. parent to child) call function on child.

When going up (e.g. child to parent) use signals. Ideally target of the signal should be set in the `Init()` (as opposed to hard paths.)


## Naming children.

Where `named` is a element with a special name (e.g. `Cancel`), and `default` has been left the class name (e.g. `VBoxContainer`).

> named1
>> default1
>>> named2

is called `named1Named2`

If chosen element is not named, use full path since last named element.

> named1
>> default1
>>> default2

is called `named1Default1Default2`.
