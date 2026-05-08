namespace GNS.Contracts.Requests.Interfaces
{
    public interface ITimeSpanRequest
    {
        DateTime DateTimeStart { get; set; }
        DateTime DateTimeEnd { get; set; }
    }
}