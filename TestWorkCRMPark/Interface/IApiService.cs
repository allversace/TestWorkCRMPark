using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestWorkCRMPark.Interface
{
    public interface IApiService
    {
        Task<string> GetCompaniesByINN(string innList);
    }
}
