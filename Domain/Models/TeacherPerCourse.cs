namespace Domain.Models
{
    public partial class TeacherPerCourse
    {
        public TeacherPerCourse()
        {
            ClassEnrollments = new HashSet<ClassEnrollment>();
            TeacherPerCoursePerSessionTimes = new HashSet<TeacherPerCoursePerSessionTime>();
        }

        public long Id { get; set; }
        public long TeacherId { get; set; }
        public long CourseId { get; set; }

        public virtual Course Course { get; set; } = null!;
        public virtual User Teacher { get; set; } = null!;
        public virtual ICollection<ClassEnrollment> ClassEnrollments { get; set; }
        public virtual ICollection<TeacherPerCoursePerSessionTime> TeacherPerCoursePerSessionTimes { get; set; }
    }
}
