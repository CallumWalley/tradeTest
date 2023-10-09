### Object specification.

`prefab_NameOfObject` for prefabs. Only use of mixed camel nad snake.

`UINameOfElement` for UI elements.
`UINameOfParentNameOfElement` For inheriting.

`Init()` called before entering scene.
`_Ready()` method called at object entering scene.

If an object can be added in editor, _Ready will be called but not `Init()`. 
All params needed for `_Ready()` must be set in Init().