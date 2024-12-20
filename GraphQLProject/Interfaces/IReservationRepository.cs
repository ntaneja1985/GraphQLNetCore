using GraphQLProject.Models;

namespace GraphQLProject.Interfaces
{
    public interface IReservationRepository
    {
        Task<List<Reservation>> GetReservations();
        Task<Reservation> AddReservation(Reservation reservation);
    }
}
