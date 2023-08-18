namespace Soda.Ice.Abstracts;

public interface IViewModel
{
    Guid Id { get; set; }
}

public abstract record ViewModel : IViewModel
{
    public Guid Id { get; set; }
}