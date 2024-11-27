
using System.ComponentModel.DataAnnotations.Schema;

namespace GameSchedule.Schedule;

public class ClientSet
{
    public long Id { get; set; }

    [InverseProperty(nameof(Client.ClientSet))]
    public ICollection<Client> Clients { get; set; } = new List<Client>();
}
