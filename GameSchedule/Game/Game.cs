using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameSchedule.Game
{
    public class Game
    {
        public long Id { get; set; }
        public bool Over {  get; set; } = false;

        public string Name { get; set; } = string.Empty;

        public long PlayerId { get; set; }
        [InverseProperty(nameof(Player.Game))]
        public ICollection<Player> Players { get; set; } = new List<Player>();

        public long GameScheduleId { get; set; }
        [ForeignKey(nameof(GameScheduleId))]
        [InverseProperty(nameof(GameSchedule.Game))]
        public GameSchedule? GameSchedule { get; set; }
    }
}
