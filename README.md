## SimplestUnityDI

A library to make using dependency injection simple in Unity. It resolves the dependencies of MonoBehaviours on special
methods and the dependencies of normal classes on constructors.

### Installation

1) Copy the contents of SimplestUnityDI to Unity project, or use `dotnet build` to create a dll and then copy it to you
   Unity project.
2) Extend the class `DiSetup`, override the method `SetupDependencies`, and add it to a GameObject.

### Usage

To add dependencies to your container, use the `DiContainer` object in the `SetupDependencies` method.

- First, call `Register` and specify the type of service to add, and the implementation of that service as type params.
  You can omit the second one if they are the same. You'll get back a **builder object to configure the service**.
- Then, use a method to tell where this service is coming from. If it's not specified, the object will be **instantiated
  using the declared constructor**. The options are:
    - `FromGameObject(string name)`: Gets a component of the GameObject with the specified name.
    - `FromResource(string path)`: Uses Unity's Resources.Load to get an object from the Resources folder.
    - `FromFunction(Func<DiContainer, TConcrete> function)`: Calls a function to receive an object.
    - `FromInstance(TConcrete instance)`: Just uses the provided instance.
- **(optional)** You can specify an id using `WithId(string id)`. In case 2 dependencies are registered with the same
  type, the container chooses the one whose Id is equal to the name of the method's parameter. Case Insensitive. Useful
  for common types like `Transform`.
- **(optional)** You can call `Permanent()` to say that a dependency shouldn't be discarded when switching scenes.
- Finally, end the chain by calling one of the methods:
    - `AsTransient()`: Declares that the container should get a new object every time.
    - `AsSingleton()`: Declares that the container should get a new object from the source only once and cache the
      result. **It's the most common**.

```c#
protected override void SetupDependencies(DiContainer diContainer)
{
    diContainer.Register<MyClass>().AsTransient(); // Instantiates MyClass using the constructor
    diContainer.Register<ComputeShader>().FromResource("Shaders/StarsShader").AsSingleton(); // Loads a compute shader on Resources/Shaders/StarsShader.compute
    diContainer.Register<PlayerController>().FromGameObject("Player").AsSingleton(); // Gets the PlayerController component from Player
    
    // Same Types but different ids
    diContainer.Register<Transform>().FromGameObject("Level").WithId("LevelTransform").AsSingleton(); // Gets the Transform from Level
    diContainer.Register<Transform>().FromGameObject("Boss").WithId("BossTransform").AsSingleton(); // Gets the Transform from Boss
    
    diContainer.Register<IGlobalData, GlobalData>().FromInstance(new GlobalData()).Permanent().AsSingleton(); // Same instance will persist in all scenes
    diContainer.Register<Guid>().FromFunction(_ => Guid.NewGuid()).AsTransient(); // Will generate a new Guid every time its called
    diContainer.Register<string>().FromInstance("My Cool Game").WithId("GameName").AsTransient(); // Transient will have no effect because it's from an instance
    
    // Transation example
    diContainer.Register<string>().FromInstance("en").WithId("UserLanguage").Permanent().AsSingleton();
    diContainer.Register<FileSystem>().AsTransient();
    diContainer.Register<IDictionary<string, string>, Dictionary<string, string>>.FromFunction(di => // di : DiContainer
        di.Resolve<FileSystem>().LoadTranslations(di.Resolve<string>(id: "USERlanguage"))).AsSingleton();
}
```

To access the dependencies:

1) Directly from the `DiContainer` using `Resolve<T>(string id="")`.
2) Extending DiMonoBehaviour and declaring a method named `Setup`.

```c#
public class BossFightController : DiMonoBehaviour
{
    private Transform _bossTransform;
    private PlayerController _playerController
    
    public void Setup(Transform bossTransform, PlayerController playerController)
    {
        _bossTransform = bossTransform; // The Boss transform is injected because (ignoring case) "bossTransform" == "BossTransform" (registered id)
        _playerController = playerController;
    }
    
    protected override void AfterInjection() { } // Use this insted of Awake
    
    private void Update() // Everything else stays the same
    {
        // Epic boss fight
    }
}
```

3) From a class's constructor

```c#
public class FileSystem 
{
    public FileSystem(IGlobalData globalData) { }
}
```

### Benefits
- Increases modularity
- Speeds up development
- Allows you to use interface more effectively
- Makes testing easy (because you can create another container just for testing)
- Allows you to follow Dependency Inversion Principle (part of SOLID acronym)

#### Customization

To add a new type of dependency, you can implement the `IProvider` interface and use extension methods to add it to the builder.

#### Notes

- Never use `DontDestroyOnLoad` on a `DiSetup`
- It's usually a good idea to have a `DiSetup` on each scene
- This project uses baking to improve performance

#### Limitations

- Circular dependencies cause StackOverflowException