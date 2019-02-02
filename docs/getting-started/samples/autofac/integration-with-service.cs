public interface IHelloWorld
{
    string GetHelloWorld();
}

public class HelloWorld : IHelloWorld

    [Cache]
    public string GetHelloWorld() =>
        return $"Hello World at {DateTime.Now}!";
}