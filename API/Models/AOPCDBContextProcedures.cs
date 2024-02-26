﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using API.Models;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading;
using System.Threading.Tasks;

namespace API.Models
{
    public partial class AOPCDBContext
    {
        private IAOPCDBContextProcedures _procedures;

        public virtual IAOPCDBContextProcedures Procedures
        {
            get
            {
                if (_procedures is null) _procedures = new AOPCDBContextProcedures(this);
                return _procedures;
            }
            set
            {
                _procedures = value;
            }
        }

        public IAOPCDBContextProcedures GetProcedures()
        {
            return Procedures;
        }

        protected void OnModelCreatingGeneratedProcedures(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Corporate_SPResult>().HasNoKey().ToView(null);
            modelBuilder.Entity<Get_PrivilegeisVIPResult>().HasNoKey().ToView(null);
            modelBuilder.Entity<SP_BusinessHotelListResult>().HasNoKey().ToView(null);
            modelBuilder.Entity<SP_DiscoverFilterbyBtypeAndBIDResult>().HasNoKey().ToView(null);
            modelBuilder.Entity<SP_GetAllPrivilegeCardbyUserResult>().HasNoKey().ToView(null);
            modelBuilder.Entity<SP_GetBFByBIDResult>().HasNoKey().ToView(null);
            modelBuilder.Entity<SP_GetOfferingFilteredbyVIDResult>().HasNoKey().ToView(null);
            modelBuilder.Entity<SP_GetOfferingFilterListResult>().HasNoKey().ToView(null);
            modelBuilder.Entity<SP_GetPFByUserAndBTypeResult>().HasNoKey().ToView(null);
            modelBuilder.Entity<SP_GetPrivilegeByPIDResult>().HasNoKey().ToView(null);
            modelBuilder.Entity<SP_GetPrivilegeFbyUandBtypeResult>().HasNoKey().ToView(null);
            modelBuilder.Entity<SP_GetVendorOfferingsResult>().HasNoKey().ToView(null);
            modelBuilder.Entity<SP_OfferingListResult>().HasNoKey().ToView(null);
            modelBuilder.Entity<SP_PrivilegeCardListResult>().HasNoKey().ToView(null);
            modelBuilder.Entity<SP_UserInfoResult>().HasNoKey().ToView(null);
            modelBuilder.Entity<SP_UserInfoListResult>().HasNoKey().ToView(null);
        }
    }

    public partial class AOPCDBContextProcedures : IAOPCDBContextProcedures
    {
        private readonly AOPCDBContext _context;

        public AOPCDBContextProcedures(AOPCDBContext context)
        {
            _context = context;
        }

        public virtual async Task<List<Corporate_SPResult>> Corporate_SPAsync(OutputParameter<int> returnValue = null, CancellationToken cancellationToken = default)
        {
            var parameterreturnValue = new SqlParameter
            {
                ParameterName = "returnValue",
                Direction = System.Data.ParameterDirection.Output,
                SqlDbType = System.Data.SqlDbType.Int,
            };

            var sqlParameters = new []
            {
                parameterreturnValue,
            };
            var _ = await _context.SqlQueryAsync<Corporate_SPResult>("EXEC @returnValue = [dbo].[Corporate_SP]", sqlParameters, cancellationToken);

            returnValue?.SetValue(parameterreturnValue.Value);

            return _;
        }

        public virtual async Task<List<Get_PrivilegeisVIPResult>> Get_PrivilegeisVIPAsync(string EmployeeID, string BusinessTypeName, OutputParameter<int> returnValue = null, CancellationToken cancellationToken = default)
        {
            var parameterreturnValue = new SqlParameter
            {
                ParameterName = "returnValue",
                Direction = System.Data.ParameterDirection.Output,
                SqlDbType = System.Data.SqlDbType.Int,
            };

            var sqlParameters = new []
            {
                new SqlParameter
                {
                    ParameterName = "EmployeeID",
                    Size = 255,
                    Value = EmployeeID ?? Convert.DBNull,
                    SqlDbType = System.Data.SqlDbType.VarChar,
                },
                new SqlParameter
                {
                    ParameterName = "BusinessTypeName",
                    Size = 255,
                    Value = BusinessTypeName ?? Convert.DBNull,
                    SqlDbType = System.Data.SqlDbType.VarChar,
                },
                parameterreturnValue,
            };
            var _ = await _context.SqlQueryAsync<Get_PrivilegeisVIPResult>("EXEC @returnValue = [dbo].[Get_PrivilegeisVIP] @EmployeeID, @BusinessTypeName", sqlParameters, cancellationToken);

            returnValue?.SetValue(parameterreturnValue.Value);

            return _;
        }

        public virtual async Task<List<SP_BusinessHotelListResult>> SP_BusinessHotelListAsync(OutputParameter<int> returnValue = null, CancellationToken cancellationToken = default)
        {
            var parameterreturnValue = new SqlParameter
            {
                ParameterName = "returnValue",
                Direction = System.Data.ParameterDirection.Output,
                SqlDbType = System.Data.SqlDbType.Int,
            };

            var sqlParameters = new []
            {
                parameterreturnValue,
            };
            var _ = await _context.SqlQueryAsync<SP_BusinessHotelListResult>("EXEC @returnValue = [dbo].[SP_BusinessHotelList]", sqlParameters, cancellationToken);

            returnValue?.SetValue(parameterreturnValue.Value);

            return _;
        }

        public virtual async Task<List<SP_DiscoverFilterbyBtypeAndBIDResult>> SP_DiscoverFilterbyBtypeAndBIDAsync(string BusinessTypeName, string BusinessID, OutputParameter<int> returnValue = null, CancellationToken cancellationToken = default)
        {
            var parameterreturnValue = new SqlParameter
            {
                ParameterName = "returnValue",
                Direction = System.Data.ParameterDirection.Output,
                SqlDbType = System.Data.SqlDbType.Int,
            };

            var sqlParameters = new []
            {
                new SqlParameter
                {
                    ParameterName = "BusinessTypeName",
                    Size = -1,
                    Value = BusinessTypeName ?? Convert.DBNull,
                    SqlDbType = System.Data.SqlDbType.VarChar,
                },
                new SqlParameter
                {
                    ParameterName = "BusinessID",
                    Size = -1,
                    Value = BusinessID ?? Convert.DBNull,
                    SqlDbType = System.Data.SqlDbType.VarChar,
                },
                parameterreturnValue,
            };
            var _ = await _context.SqlQueryAsync<SP_DiscoverFilterbyBtypeAndBIDResult>("EXEC @returnValue = [dbo].[SP_DiscoverFilterbyBtypeAndBID] @BusinessTypeName, @BusinessID", sqlParameters, cancellationToken);

            returnValue?.SetValue(parameterreturnValue.Value);

            return _;
        }

        public virtual async Task<List<SP_GetAllPrivilegeCardbyUserResult>> SP_GetAllPrivilegeCardbyUserAsync(string EmployeeID, OutputParameter<int> returnValue = null, CancellationToken cancellationToken = default)
        {
            var parameterreturnValue = new SqlParameter
            {
                ParameterName = "returnValue",
                Direction = System.Data.ParameterDirection.Output,
                SqlDbType = System.Data.SqlDbType.Int,
            };

            var sqlParameters = new []
            {
                new SqlParameter
                {
                    ParameterName = "EmployeeID",
                    Size = 255,
                    Value = EmployeeID ?? Convert.DBNull,
                    SqlDbType = System.Data.SqlDbType.VarChar,
                },
                parameterreturnValue,
            };
            var _ = await _context.SqlQueryAsync<SP_GetAllPrivilegeCardbyUserResult>("EXEC @returnValue = [dbo].[SP_GetAllPrivilegeCardbyUser] @EmployeeID", sqlParameters, cancellationToken);

            returnValue?.SetValue(parameterreturnValue.Value);

            return _;
        }

        public virtual async Task<List<SP_GetBFByBIDResult>> SP_GetBFByBIDAsync(string BusinessID, OutputParameter<int> returnValue = null, CancellationToken cancellationToken = default)
        {
            var parameterreturnValue = new SqlParameter
            {
                ParameterName = "returnValue",
                Direction = System.Data.ParameterDirection.Output,
                SqlDbType = System.Data.SqlDbType.Int,
            };

            var sqlParameters = new []
            {
                new SqlParameter
                {
                    ParameterName = "BusinessID",
                    Size = 255,
                    Value = BusinessID ?? Convert.DBNull,
                    SqlDbType = System.Data.SqlDbType.VarChar,
                },
                parameterreturnValue,
            };
            var _ = await _context.SqlQueryAsync<SP_GetBFByBIDResult>("EXEC @returnValue = [dbo].[SP_GetBFByBID] @BusinessID", sqlParameters, cancellationToken);

            returnValue?.SetValue(parameterreturnValue.Value);

            return _;
        }

        public virtual async Task<List<SP_GetOfferingFilteredbyVIDResult>> SP_GetOfferingFilteredbyVIDAsync(string VendorID, OutputParameter<int> returnValue = null, CancellationToken cancellationToken = default)
        {
            var parameterreturnValue = new SqlParameter
            {
                ParameterName = "returnValue",
                Direction = System.Data.ParameterDirection.Output,
                SqlDbType = System.Data.SqlDbType.Int,
            };

            var sqlParameters = new []
            {
                new SqlParameter
                {
                    ParameterName = "VendorID",
                    Size = 255,
                    Value = VendorID ?? Convert.DBNull,
                    SqlDbType = System.Data.SqlDbType.VarChar,
                },
                parameterreturnValue,
            };
            var _ = await _context.SqlQueryAsync<SP_GetOfferingFilteredbyVIDResult>("EXEC @returnValue = [dbo].[SP_GetOfferingFilteredbyVID] @VendorID", sqlParameters, cancellationToken);

            returnValue?.SetValue(parameterreturnValue.Value);

            return _;
        }

        public virtual async Task<List<SP_GetOfferingFilterListResult>> SP_GetOfferingFilterListAsync(string BusinessTypeName, OutputParameter<int> returnValue = null, CancellationToken cancellationToken = default)
        {
            var parameterreturnValue = new SqlParameter
            {
                ParameterName = "returnValue",
                Direction = System.Data.ParameterDirection.Output,
                SqlDbType = System.Data.SqlDbType.Int,
            };

            var sqlParameters = new []
            {
                new SqlParameter
                {
                    ParameterName = "BusinessTypeName",
                    Size = 255,
                    Value = BusinessTypeName ?? Convert.DBNull,
                    SqlDbType = System.Data.SqlDbType.VarChar,
                },
                parameterreturnValue,
            };
            var _ = await _context.SqlQueryAsync<SP_GetOfferingFilterListResult>("EXEC @returnValue = [dbo].[SP_GetOfferingFilterList] @BusinessTypeName", sqlParameters, cancellationToken);

            returnValue?.SetValue(parameterreturnValue.Value);

            return _;
        }

        public virtual async Task<List<SP_GetPFByUserAndBTypeResult>> SP_GetPFByUserAndBTypeAsync(string BusinessTypeName, string EmployeeID, OutputParameter<int> returnValue = null, CancellationToken cancellationToken = default)
        {
            var parameterreturnValue = new SqlParameter
            {
                ParameterName = "returnValue",
                Direction = System.Data.ParameterDirection.Output,
                SqlDbType = System.Data.SqlDbType.Int,
            };

            var sqlParameters = new []
            {
                new SqlParameter
                {
                    ParameterName = "BusinessTypeName",
                    Size = 255,
                    Value = BusinessTypeName ?? Convert.DBNull,
                    SqlDbType = System.Data.SqlDbType.VarChar,
                },
                new SqlParameter
                {
                    ParameterName = "EmployeeID",
                    Size = 255,
                    Value = EmployeeID ?? Convert.DBNull,
                    SqlDbType = System.Data.SqlDbType.VarChar,
                },
                parameterreturnValue,
            };
            var _ = await _context.SqlQueryAsync<SP_GetPFByUserAndBTypeResult>("EXEC @returnValue = [dbo].[SP_GetPFByUserAndBType] @BusinessTypeName, @EmployeeID", sqlParameters, cancellationToken);

            returnValue?.SetValue(parameterreturnValue.Value);

            return _;
        }

        public virtual async Task<List<SP_GetPrivilegeByPIDResult>> SP_GetPrivilegeByPIDAsync(string PrivilegeID, OutputParameter<int> returnValue = null, CancellationToken cancellationToken = default)
        {
            var parameterreturnValue = new SqlParameter
            {
                ParameterName = "returnValue",
                Direction = System.Data.ParameterDirection.Output,
                SqlDbType = System.Data.SqlDbType.Int,
            };

            var sqlParameters = new []
            {
                new SqlParameter
                {
                    ParameterName = "PrivilegeID",
                    Size = 255,
                    Value = PrivilegeID ?? Convert.DBNull,
                    SqlDbType = System.Data.SqlDbType.VarChar,
                },
                parameterreturnValue,
            };
            var _ = await _context.SqlQueryAsync<SP_GetPrivilegeByPIDResult>("EXEC @returnValue = [dbo].[SP_GetPrivilegeByPID] @PrivilegeID", sqlParameters, cancellationToken);

            returnValue?.SetValue(parameterreturnValue.Value);

            return _;
        }

        public virtual async Task<List<SP_GetPrivilegeFbyUandBtypeResult>> SP_GetPrivilegeFbyUandBtypeAsync(int? BusinessTypeID, string EmployeeID, OutputParameter<int> returnValue = null, CancellationToken cancellationToken = default)
        {
            var parameterreturnValue = new SqlParameter
            {
                ParameterName = "returnValue",
                Direction = System.Data.ParameterDirection.Output,
                SqlDbType = System.Data.SqlDbType.Int,
            };

            var sqlParameters = new []
            {
                new SqlParameter
                {
                    ParameterName = "BusinessTypeID",
                    Value = BusinessTypeID ?? Convert.DBNull,
                    SqlDbType = System.Data.SqlDbType.Int,
                },
                new SqlParameter
                {
                    ParameterName = "EmployeeID",
                    Size = 255,
                    Value = EmployeeID ?? Convert.DBNull,
                    SqlDbType = System.Data.SqlDbType.VarChar,
                },
                parameterreturnValue,
            };
            var _ = await _context.SqlQueryAsync<SP_GetPrivilegeFbyUandBtypeResult>("EXEC @returnValue = [dbo].[SP_GetPrivilegeFbyUandBtype] @BusinessTypeID, @EmployeeID", sqlParameters, cancellationToken);

            returnValue?.SetValue(parameterreturnValue.Value);

            return _;
        }

        public virtual async Task<List<SP_GetVendorOfferingsResult>> SP_GetVendorOfferingsAsync(OutputParameter<int> returnValue = null, CancellationToken cancellationToken = default)
        {
            var parameterreturnValue = new SqlParameter
            {
                ParameterName = "returnValue",
                Direction = System.Data.ParameterDirection.Output,
                SqlDbType = System.Data.SqlDbType.Int,
            };

            var sqlParameters = new []
            {
                parameterreturnValue,
            };
            var _ = await _context.SqlQueryAsync<SP_GetVendorOfferingsResult>("EXEC @returnValue = [dbo].[SP_GetVendorOfferings]", sqlParameters, cancellationToken);

            returnValue?.SetValue(parameterreturnValue.Value);

            return _;
        }

        public virtual async Task<List<SP_OfferingListResult>> SP_OfferingListAsync(OutputParameter<int> returnValue = null, CancellationToken cancellationToken = default)
        {
            var parameterreturnValue = new SqlParameter
            {
                ParameterName = "returnValue",
                Direction = System.Data.ParameterDirection.Output,
                SqlDbType = System.Data.SqlDbType.Int,
            };

            var sqlParameters = new []
            {
                parameterreturnValue,
            };
            var _ = await _context.SqlQueryAsync<SP_OfferingListResult>("EXEC @returnValue = [dbo].[SP_OfferingList]", sqlParameters, cancellationToken);

            returnValue?.SetValue(parameterreturnValue.Value);

            return _;
        }

        public virtual async Task<List<SP_PrivilegeCardListResult>> SP_PrivilegeCardListAsync(OutputParameter<int> returnValue = null, CancellationToken cancellationToken = default)
        {
            var parameterreturnValue = new SqlParameter
            {
                ParameterName = "returnValue",
                Direction = System.Data.ParameterDirection.Output,
                SqlDbType = System.Data.SqlDbType.Int,
            };

            var sqlParameters = new []
            {
                parameterreturnValue,
            };
            var _ = await _context.SqlQueryAsync<SP_PrivilegeCardListResult>("EXEC @returnValue = [dbo].[SP_PrivilegeCardList]", sqlParameters, cancellationToken);

            returnValue?.SetValue(parameterreturnValue.Value);

            return _;
        }

        public virtual async Task<List<SP_UserInfoResult>> SP_UserInfoAsync(OutputParameter<int> returnValue = null, CancellationToken cancellationToken = default)
        {
            var parameterreturnValue = new SqlParameter
            {
                ParameterName = "returnValue",
                Direction = System.Data.ParameterDirection.Output,
                SqlDbType = System.Data.SqlDbType.Int,
            };

            var sqlParameters = new []
            {
                parameterreturnValue,
            };
            var _ = await _context.SqlQueryAsync<SP_UserInfoResult>("EXEC @returnValue = [dbo].[SP_UserInfo]", sqlParameters, cancellationToken);

            returnValue?.SetValue(parameterreturnValue.Value);

            return _;
        }

        public virtual async Task<List<SP_UserInfoListResult>> SP_UserInfoListAsync(string jwToken, OutputParameter<int> returnValue = null, CancellationToken cancellationToken = default)
        {
            var parameterreturnValue = new SqlParameter
            {
                ParameterName = "returnValue",
                Direction = System.Data.ParameterDirection.Output,
                SqlDbType = System.Data.SqlDbType.Int,
            };

            var sqlParameters = new []
            {
                new SqlParameter
                {
                    ParameterName = "jwToken",
                    Size = -1,
                    Value = jwToken ?? Convert.DBNull,
                    SqlDbType = System.Data.SqlDbType.VarChar,
                },
                parameterreturnValue,
            };
            var _ = await _context.SqlQueryAsync<SP_UserInfoListResult>("EXEC @returnValue = [dbo].[SP_UserInfoList] @jwToken", sqlParameters, cancellationToken);

            returnValue?.SetValue(parameterreturnValue.Value);

            return _;
        }
    }
}
