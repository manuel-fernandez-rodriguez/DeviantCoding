namespace Registerly.Samples.AdvancedRegistration.Animals;

public interface IAnimal { string Name => GetType().Name; }

internal class Cat : IAnimal { }
internal class Dog : IAnimal { }
internal class Spider : IAnimal { }
internal class Alligator : IAnimal { }
