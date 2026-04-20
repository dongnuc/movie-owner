namespace Movie_StructureCode.Domain.Entities
{
    public enum StatusSeat
    {
        Active = 1,
        Broken = 2,
        None = 3
    }

    public class Seat : BaseEntities
    {
        public int RowIndex { get; set; } // do cot va hang cua room quyet dinh
        public int ColIndex { get; set; }
        public string RowName { get; set; }
        public int SeatNumber { get; set; }

        public Guid? GroupId { get; set; }

        public StatusSeat StatusSeat { get; set; } = StatusSeat.Broken;

        // foreign key

        public Guid SeatTypeId { get; set; }
        public SeatType SeatType { get; set; }

        public Guid RoomId { get; set; }
        public Room? Room { get; set; }
    }
}
