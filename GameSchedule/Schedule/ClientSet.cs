
namespace GameSchedule.Schedule;

public class ClientSet
{
    public long Id { get; set; }

    public ICollection<Client> Clients { get; set; } = new List<Client>();
}
