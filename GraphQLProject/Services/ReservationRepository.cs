using GraphQLProject.Data;
using GraphQLProject.Interfaces;
using GraphQLProject.Models;
using Microsoft.EntityFrameworkCore;

namespace GraphQLProject.Services
{
    public class ReservationRepository(GraphQLDBContext dbContext) : IReservationRepository
    {
        public async Task<Reservation> AddReservation(Reservation reservation)
        {
            dbContext.Reservations.Add(reservation);
            await dbContext.SaveChangesAsync();
            return reservation;

        }

        public async Task<List<Reservation>> GetReservations()
        {
            return await dbContext.Reservations.ToListAsync();
        }
    }
}
