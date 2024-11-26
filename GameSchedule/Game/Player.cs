
using System.ComponentModel.DataAnnotations.Schema;

namespace GameSchedule.Game
{
    public class Player
    {
        public long Id { get; set; }
        public short Num { get; set; }

        public long GameId { get; set; }
        [ForeignKey(nameof(GameId))]
        [InverseProperty(nameof(Game.Players))]
        public Game? Game { get; set; }

        public long SaveId { get; set; }
        [ForeignKey(nameof(SaveId))]
        [InverseProperty(nameof(Save.Player))]
        public Save? SaveData { get; set; }
    }
}
