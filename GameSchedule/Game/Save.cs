
using System.ComponentModel.DataAnnotations.Schema;

namespace GameSchedule.Game
{
    public class Save
    {
        public long Id { get; set; }

        public string Data { get; set; } = string.Empty;

        public long PlayerId { get; set; }
        [InverseProperty(nameof(Player.SaveData))]
        public Player? Player { get; set; }
    }
}
