namespace PotapkinPrac1.Errors;

public class NotFoundException : Exception
{
    public NotFoundException(string message) : base(message)
    {
    }
}
