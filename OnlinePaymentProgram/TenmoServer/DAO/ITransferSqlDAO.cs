using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TenmoServer.Models;

namespace TenmoServer.DAO
{
    public interface ITransferSqlDAO
    {
        Transfers MakeTransfer(Transfers newTransfer);

        List<Transfers> ViewTransfers(int userId);
    }
}
