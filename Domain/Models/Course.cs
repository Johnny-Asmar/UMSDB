using NpgsqlTypes;

namespace Domain.Models
{
    public partial class Course
    {
        public Course()
        {
            TeacherPerCourses = new HashSet<TeacherPerCourse>();
        }

        public long Id { get; set; }
        public string? Name { get; set; }
        public int? MaxStudentsNumber { get; set; }
        public NpgsqlRange<DateOnly>? EnrolmentDateRange { get; set; }

        public virtual ICollection<TeacherPerCourse> TeacherPerCourses { get; set; }
    }
}
