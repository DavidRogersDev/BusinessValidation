using Demo.Console.Domain;

namespace Demo.Console.Services
{
    public interface IConsultationService
    {
        Lecturer GetLecturerForUnit(string unitNumber);
    }
}
