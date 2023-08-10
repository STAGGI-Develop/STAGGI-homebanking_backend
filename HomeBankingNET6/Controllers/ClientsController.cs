using HomeBankingNET6.DTOs;
using HomeBankingNET6.Services;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using HomeBankingNET6.Models;
using System.Linq;
using System;
using Microsoft.Extensions.Logging.Abstractions;
using HomeBankingNET6.Helpers;


namespace HomeBankingNET6.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClientsController : ControllerBase
    {
        private readonly IClientService _clientService;
        public ClientsController(IClientService clientService)
        {
            _clientService = clientService;
        }

        [HttpGet]
        public IActionResult Get()
        {
            var clientsDTO = _clientService.GetAllClients();
            return Ok(clientsDTO);
        }

        [HttpGet("{id}")]
        public IActionResult Get(long id)
        {
            var clientDTO = _clientService.GetClientById(id);
            if (clientDTO == null)
                return NoContent();
            else
                return Ok(clientDTO);
        }

        [HttpPost]
        public IActionResult Post([FromBody] CreateClientRequestDTO newClient)
        {
            ClientDTO createdClientDTO = _clientService.CreateClient(newClient);
            if (createdClientDTO == null)
                return StatusCode(403, "No se pudo crear el cliente.");

            return Created("Cliente creado", createdClientDTO);
        }

        [HttpGet("current")]
        public IActionResult GetCurrent()
        {
            var currentClientDTO = _clientService.GetCurrentClient();
            if (currentClientDTO == null)
                return Unauthorized();

            return Ok(currentClientDTO);
        }

    }
}
