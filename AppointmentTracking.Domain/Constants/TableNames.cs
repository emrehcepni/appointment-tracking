using AppointmentTracking.Domain.Entities;

namespace AppointmentTracking.Domain.Constants;

public static class TableNames
{
    private const string suffix = "s";

    public const string Appointments = nameof(Appointment) + suffix;
    public const string Candidates = nameof(Candidate) + suffix;
    public const string Instructors = nameof(Instructor) + suffix;
    public const string Lessons = nameof(Lesson) + suffix;
    public const string Vehicles = nameof(Vehicle) + suffix;
    public const string VehicleDetails = nameof(VehicleDetail) + suffix;
}
