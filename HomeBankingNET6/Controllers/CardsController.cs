using HomeBankingNET6.DTOs;
using HomeBankingNET6.Services;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace HomeBankingNET6.Controllers
{
    [Route("api")]
    [ApiController]
    public class CardsController : ControllerBase
    {
        private readonly ICardService _cardService;
        public CardsController(ICardService cardService)
        {
            _cardService = cardService;
        }

        [HttpPost("clients/current/cards")]
        public IActionResult CreateCardToCurrent([FromBody] CreateCardRequestDTO cardRequest)
        {
            var result = _cardService.CreateCardForCurrentClient(cardRequest.Type, cardRequest.Color);

            if (!result.IsSuccess)
            {
                return StatusCode(result.Error.Status, result.Error);
            }

            return StatusCode(201, result.Ok);
        }

        [HttpGet("clients/current/cards")]
        public IActionResult GetCurrentCards()
        {
            List<CardDTO> clientCards = _cardService.GetCurrentClientCards();
            if (clientCards == null)
                return Unauthorized();

            return Ok(clientCards);
        }

    }
}
