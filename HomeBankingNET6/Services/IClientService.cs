using HomeBankingNET6.DTOs;
using HomeBankingNET6.Helpers;
using HomeBankingNET6.Models;
using System.Collections.Generic;

namespace HomeBankingNET6.Services
{
    public interface IClientService
    {
        List<ClientDTO> GetAllClients();
        ClientDTO GetClientById(long id);
        Result<ClientDTO> CreateClient(CreateClientRequestDTO client);
        ClientDTO GetCurrentClient();
    }
}
