# Strategy Coordinator

This dotnet standard 2.0 library makes the chaining and coordination of strategies simple for dotnet applications.

## Nuget
https://www.nuget.org/packages/StrategyCoordinator/ 


## Strategies
Strategies are a pattern of breaking often large sections of domain logic into small and testable components with strongly defined side effects. There are a bunch of nifty advantages:

* Long pieces of code can be broken into small pieces.
* Stategies that encapsulate domain logic can be easily tested.
* Increase the reuse of domain logic.
* Simple strategies can be combined in any order to achieve complex tasks.

A real world example of a strategy might be in a cooking recipe:
 
 1. An oven must be pre heated to some temperature.
 1. Eggs, flour, sugar, and butter must be mixed in a bowl.
 1. The mixture must be separated into small pieces
 1. The small pieces must be put in the oven.
 1. The pieces must be taken out of the oven after 25 minutes
 1. The pieces must be allowed to cool for 10 minutes.
 1. the pieces must be put on a plate.

In the above recipe the cook reads the recipe, performs actions, and those actions have side effects on the domain of the cook (kitchen), ultimately resulting in cookies (the result).

 A computer program that coordinates all of these instructions but only makes cookies does not allow easy reuse of complicated components.

 Ideally the program could be configured to cook a recipe rather than a single program being required for each new recipe.

## Coordinator Configuration

The above recipe can be looked at as domain (kitchen) specific actions that require integration with the kitchen such as pressing the button to activate the mixer, and the side effects that are happening that result in cookies such as combining the ingredients to make cookie batter. Mixing is the action, cookie batter is the side effect.

A cookie maker coordinator might look like this:

```
var cookieCoordinatorFactory = new StrategyCoordinatorFactory<Ingredients, Cookies, Kitchen>(new KitchenFactory());
```

Where ```Ingredients``` are the input type, ```Cookies``` are the output type, and the ```Kithen``` type is where all the side effects take place.

The first action of heating the oven might look like this:

```
cookieCoordinatorFactory.UseAsync((Kitchen, Next)=>{
    Kitchen.Oven.SetTemp(350);

    return Next.InvokeAsync();
});
```

or 

```
var preheatStrategy = new CookiePreheatStrategy();
cookieCoordinatorFactory.UseAsync(preheatStrategy);
```

Then the second action might look like this:

```
cookieCoordinatorFactory.UseAsync(async (Kitchen, Next)=>{
    Kitchen.MixingBowl.Add(Kitchen.Eggs);
    Kitchen.MixingBowl.Add(Kitchen.Butter);
    Kitchen.MixingBowl.Add(Kitchen.Milk);
    Kitchen.MixingBowl.Add(Kitchen.Flour);
    Kitchen.MixingBowl.Add(Kitchen.Mix);

    await Kitchen.MixingBowl.Mix();
});
```

At this point in the application we would have created the cookie batter.

## Strategy Configuration

It would pretty neat if we could use the preheat oven strategy for other recipes that require the oven to be preheated such as baked potatoes, but that require a different temperature.

A smart developer might inject the temperature in the constructor like this:

```
public class PreheatOvenStrategy: StrategyCoordinator.Core.IAsyncProcessStrategy<Kitchen>{

    private readonly int _temperature;

    public PreheatOvenStrategy(int temperature){
        this._temperature = temperature;
    }

    public async System.Threading.Tasks.Task ProcessAsync(Kitchen kitchen, StrategyCoordinator.Core.IInvokeable next)
    {
        Kitchen.Oven.SetTemp(this._temperature);

        await next.InvokeAsync();
    }
}
```

then the pre heating strategy can be configured like this:

```
var preheatStrategy = new PreheatOvenStrategy(350);
cookieCoordinatorFactory.UseAsync(preheatStrategy);
```

## Processing Input and retrieving Results

After the cookie recipe strategies have been loaded in, the cookies are ready to be made, or in this case the ingredients are ready to be processed.

First an instance of the cookie coordinator must be built, this can be thought of as a kitchen being created and a copy of the recipe being given to the cook.  

Then the ingredients can be given to the waiting kitchen and cook.

```
var cookieCoordinator = cookieCoordinatorFactory.Build();

var cookies = await cookieCoordinator.ProcessAsync(ingredients);
```
