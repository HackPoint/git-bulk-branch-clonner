using testing.Application.Common.Interfaces;

namespace testing.Infrastructure.Services;

public class DateTimeService : IDateTime {
    public DateTime Now => DateTime.Now;
}