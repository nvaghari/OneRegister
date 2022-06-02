using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using OneRegister.Data.Context;
using OneRegister.Data.Entities.EDuit;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace OneRegister.Domain.Repository
{
    public class EDuitRepository
    {
        private readonly EDuitContext _context;

        public EDuitRepository(EDuitContext context)
        {
            _context = context;
        }

        public bool IsConnected()
        {

            return _context.Database.CanConnect();
        }

        public void ConfirmStudent(StudentConfirmDataModel model)
        {
            var result = new SqlParameter
            {
                ParameterName = "@Result",
                SqlDbType = SqlDbType.NVarChar,
                Direction = ParameterDirection.Output,
                Size = 500
            };
            _context.Database.ExecuteSqlInterpolated($@"
                exec [Ext].[ConfirmStudent]
		            @Result = {result} OUT,
                    @SchoolName = {model.SchoolName},
                    @StudentId = {model.IdentityNumber},
                    @FirstName = {model.Name},
                    @MiddleName = {null},
                    @LastName = {null},
		            @Birthday = {model.Birthday},
		            @Gender = {model.Gender},
		            @Grade = {0},
		            @Year = {model.Year},
		            @OneRegisterId = {model.Id},
		            @StudentNumber = {model.StudentNumber},
		            @IdentityType = {model.IdentityType},
		            @Nationality = {model.Nationality},
		            @ClassName = {model.ClassName},
		            @ClassLabel = {model.ClassLabel},
		            @HomeRoom = {model.HomeRoomName},
		            @ParentName = {model.ParentName},
		            @ParentPhone = {model.ParentPhone},
		            @ParentAddress = {model.Address},
                    @DmsRef1 = {model.DmsRef}
                ");

            if (result.Value.ToString().CompareTo("SUCCESSFUL") != 0)
            {
                throw new ApplicationException(result.Value.ToString());
            }


        }

        public List<EDuitSchool> GetSchools()
        {
            return _context.EDuitSchools.FromSqlRaw("EXEC [Ext].[Student_School_Get]").ToList();
        }

        public void AddSchool(EDuitSchool eDuitSchool)
        {
            _context.Database.ExecuteSqlInterpolated(@$"
               EXEC [Ext].[Student_School_Add]
                @code = {eDuitSchool.Code},
                @name = {eDuitSchool.ShortName}
                ");
        }
    }
}
