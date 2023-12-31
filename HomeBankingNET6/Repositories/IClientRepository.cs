﻿using HomeBankingNET6.Models;
using System.Collections.Generic;

namespace HomeBankingNET6.Repositories
{
    public interface IClientRepository
    {
        IEnumerable<Client> GetAllClients(); //SELECT * FROM <Clients>. Devuelve de 0 a muchos registros (0 si no hay clients).
        void Save(Client client); //Se basa en el create del RepositoryBase y guarda los cambios en el contexto.
        Client FindById(long id); //SELECT * FROM <Clients> WHERE Id=id. Devuelve un registro o ninguno.
                                  //Como vemos el uso de la sentencia WHERE, usaremos el método FindByCondition del RepositoryBase.
        Client FindByEmail(string email); //SELECT * FROM <Clients> WHERE Email = email. Devuelve un registro o ninguno.
    }
}
