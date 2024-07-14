using Lombeo.Api.Authorize.DTO.DoctorDTO;
using Lombeo.Api.Authorize.Infra;
using Lombeo.Api.Authorize.Infra.Constants;
using Lombeo.Api.Authorize.Infra.Entities;
using Lombeo.Api.Authorize.Services.CacheService;
using Microsoft.CodeAnalysis.FlowAnalysis.DataFlow;
using Microsoft.EntityFrameworkCore;

namespace Lombeo.Api.Authorize.Services.ChildcareService
{
    public interface IChildcareService
    {
        Task<int> SaveDoctor(SaveDoctorDTO model);
        Task<int> DeleteDoctor(int id);
        Task<List<Doctor>> GetAllDoctor();
        Task<Doctor> GetDoctorById(int id);
        Task<List<Booking>> GetAllBooking();
        Task<Booking> GetBookingById(int id);
        Task<bool> DeleteBooking(int id);
        Task<int> SwitchBookingStatus(SwitchStatusDTO model);
        Task<int> SaveBooking(SaveBookingDTO model);
    }

    public class ChildcareService : IChildcareService
    {
        private readonly LombeoAuthorizeContext _context;
        private readonly ICacheService _cacheService;

        public ChildcareService(LombeoAuthorizeContext context, ICacheService cacheService)
        {
            _context = context;
            _cacheService = cacheService;
        }

        public async Task<int> DeleteDoctor(int id)
        {
            var doctor = await _context.Doctors.FirstOrDefaultAsync(t => t.Id == id);
            if (doctor == null)
            {
                throw new ApplicationException(Message.CommonMessage.NOT_FOUND);
            }

            doctor.Deleted = true;
            _context.Update(doctor);

            await _context.SaveChangesAsync();

            _ = _cacheService.DeleteAsync(RedisCacheKey.LIST_DOCTOR);

            return doctor.Id;
        }

        public async Task<List<Doctor>> GetAllDoctor()
        {
            string cacheKey = RedisCacheKey.LIST_DOCTOR;
            var data = await _cacheService.GetAsync<List<Doctor>>(cacheKey);

            if (data == null)
            {
                data = await _context.Doctors.Where(t => !t.Deleted).ToListAsync();
                _ = _cacheService.SetAsync(cacheKey, data);
            }

            return data;
        }

        public async Task<Doctor> GetDoctorById(int id)
        {
            var data = await GetAllDoctor();
            var doctor = data.FirstOrDefault(t => t.Id == id);
            if (doctor != null)
            {
                return doctor;
            }
            throw new ApplicationException(Message.CommonMessage.NOT_FOUND);
        }

        public async Task<int> SaveDoctor(SaveDoctorDTO model)
        {
            var entity = new Doctor()
            {
                Id = 0,
                CreatedAt = DateTime.Now,
                Deleted = false
            };

            if (model.Id != 0)
            {
                entity = await _context.Doctors.FirstOrDefaultAsync(t => !t.Deleted && t.Id == model.Id);

                if (entity == null)
                {
                    throw new ApplicationException(Message.CommonMessage.NOT_FOUND);
                }
            }

            entity.ProfilePic = model.ProfilePic;
            entity.Name = model.Name;
            entity.BriefInfo = model.BriefInfo;
            entity.Room = model.Room;
            entity.Location = model.Location;
            entity.Shift = model.Shift;
            entity.Price = model.Price;
            entity.Info = model.Info;

            if (model.Id != 0)
            {
                _context.Update(entity);
            }
            else
            {
                await _context.AddAsync(entity);
            }

            await _context.SaveChangesAsync();

            _ = _cacheService.DeleteAsync(RedisCacheKey.LIST_DOCTOR);

            return entity.Id;
        }

        public async Task<int> SaveBooking(SaveBookingDTO model)
        {
            var entity = new Booking()
            {
                Id = 0,
                CreatedAt = DateTime.UtcNow,
                Deleted = false
            };

            if (model.Id != 0)
            {
                entity = await _context.Bookings.FirstOrDefaultAsync(t => !t.Deleted && t.Id == model.Id);

                if (entity == null)
                {
                    throw new ApplicationException(Message.CommonMessage.NOT_FOUND);
                }
            }

            entity.FullName = model.FullName;
            entity.Email = model.Email;
            entity.PhoneNumber = model.PhoneNumber;
            entity.Gender = model.Gender;
            entity.Address = model.Address;
            entity.Dob = model.Dob;
            entity.Note = model.Note;
            entity.DoctorId = model.DoctorId;
            entity.ShiftTo = model.Shift;
            entity.Status = 0;

            if (model.Id != 0)
            {
                _context.Update(entity);
            }
            else
            {
                await _context.AddAsync(entity);
            }

            await _context.SaveChangesAsync();

            _ = _cacheService.DeleteAsync(RedisCacheKey.LIST_BOOKING);

            return entity.Id;
        }

        public async Task<List<Booking>> GetAllBooking()
        {
            string cacheKey = RedisCacheKey.LIST_BOOKING;
            var data = await _cacheService.GetAsync<List<Booking>>(cacheKey);

            if (data == null)
            {
                data = await _context.Bookings.Where(t => !t.Deleted).ToListAsync();
                _ = _cacheService.SetAsync(cacheKey, data);
            }

            return data;
        }

        public async Task<Booking> GetBookingById(int id)
        {
            var data = await GetAllBooking();
            var booking = data.FirstOrDefault(t => t.Id == id);
            if (booking == null)
            {
                throw new ApplicationException(Message.CommonMessage.NOT_FOUND);
            }
            return booking;
        }

        public async Task<bool> DeleteBooking(int id)
        {
            var booking = await _context.Bookings.FirstOrDefaultAsync(t => t.Id == id);
            if (booking == null)
            {
                throw new ApplicationException(Message.CommonMessage.NOT_FOUND);
            }

            booking.Deleted = true;

            _context.Update(booking);

            await _context.SaveChangesAsync();

            _ = _cacheService.DeleteAsync(RedisCacheKey.LIST_BOOKING);

            return true;
        }

        public async Task<int> SwitchBookingStatus(SwitchStatusDTO model)
        {
            var booking = await _context.Bookings.FirstOrDefaultAsync(t => t.Id == model.Id);
            if(booking != null)
            {
                booking.Status = model.Status;
                _context.Update(booking);
                await _context.SaveChangesAsync();
                _ = _cacheService.DeleteAsync(RedisCacheKey.LIST_BOOKING);
                return booking.Status;
            }
            throw new ApplicationException(Message.CommonMessage.NOT_FOUND);
        }
    }
}
