using Onion.Domain.Entities;
using Onion.Domain.Services;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace Onion.Infrastructure.MockDbService.Services
{
    public class MockDbService : IStudentDbService
    {
        private static readonly ICollection<Student> _students = new List<Student>
        {
            //new Student{IdStudent=1, FirstName="Jan", LastName="Kowalski"},
            //new Student{IdStudent=2, FirstName="Anna", LastName="Malewski"},
            //new Student{IdStudent=3, FirstName="Andrzej", LastName="Maciejewski"}

        };

        public bool EnrollStudent(Student newStudent, int semestr)
        {
            

            _students.Add(newStudent);
            return true;
        }

        public IEnumerable<Student> GetStudents()
        {
            var connectionString = "Data Source=db-mssql;Initial Catalog=s14893;Integrated Security=True";
            var queryString = "SELECT * FROM Student";
            using (SqlConnection connection = new SqlConnection(
           connectionString))
            {
                SqlCommand cmd = new SqlCommand(queryString, connection);
                cmd.Connection.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            Student p = new Student();
                            p.IdStudent = reader.GetInt32(reader.GetOrdinal("IdStudent"));
                            p.FirstName = reader.GetString(reader.GetOrdinal("FirstName"));
                            int middleNameIndex = reader.GetOrdinal("LastName");
                            if (!reader.IsDBNull(middleNameIndex))
                            {
                                p.LastName = reader.GetString(middleNameIndex);
                            }
                            p.LastName = reader.GetString(reader.GetOrdinal("LastName"));
                            _students.Add(p);
                        }
                    }
                }
            }
            return _students;
        }
    }
}
