using HomeBankingNET6.Enums;
using HomeBankingNET6.Models;
using HomeBankingNET6.Repositories;
using System.Threading.Tasks;
using System;
using System.Linq;
using HomeBankingNET6.DTOs;
using System.Collections.Generic;
using HomeBankingNET6.Helpers;

namespace HomeBankingNET6.Services
{
    public class CardService : ICardService
    {
        private readonly ICardRepository _cardRepository;
        private readonly IClientRepository _clientRepository;
        private readonly IAuthService _authService;
        public CardService(ICardRepository cardRepository, IClientRepository clientRepository, IAuthService authService)
        {
            _cardRepository = cardRepository;
            _clientRepository = clientRepository;
            _authService = authService;
        }

        public Result<CardDTO> CreateCardForCurrentClient(string cardType, string cardColor)
        {
            string userAuthenticatedEmail = _authService.UserAuthenticated();
            if (userAuthenticatedEmail == null) return Result<CardDTO>.Unauthorized();

            Client currentClient = _clientRepository.FindByEmail(userAuthenticatedEmail);

            CardType parsedCardType;
            if (!Enum.TryParse(cardType, out parsedCardType))
            {
                return Result<CardDTO>.Failure(new ErrorResponseDTO
                {
                    Status = 403,
                    Error = "Forbidden",
                    Message = $"El tipo de tarjeta {cardType} no es válido"
                });
            }

            CardColor parsedCardColor;
            if (!Enum.TryParse(cardColor, out parsedCardColor))
            {
                return Result<CardDTO>.Failure(new ErrorResponseDTO
                {
                    Status = 403,
                    Error = "Forbidden",
                    Message = $"El color de tarjeta {cardColor} no es válido"
                });
            }

            int numberOfCards = currentClient.Cards.Count(c => c.Type == cardType);
            if (numberOfCards >= 3)
            {
                return Result<CardDTO>.Failure(new ErrorResponseDTO
                {
                    Status = 403,
                    Error = "Forbidden",
                    Message = $"El cliente alcanzó el número máximo de tarjetas {parsedCardType}"
                });
            }

            (string number, int cvv) cardData = CardDataGeneratorHelper.Generate();
            ///pending check existing card numbers
            Card newCard = new Card
            {
                ClientId = currentClient.Id,
                CardHolder = $"{currentClient.FirstName} {currentClient.LastName}",
                Type = cardType.ToString(),
                Color = cardColor.ToString(),
                Number = cardData.number,
                Cvv = cardData.cvv,
                FromDate = DateTime.Now,
                ThruDate = DateTime.Now.AddYears(4),
            };
            _cardRepository.Save(newCard);

            newCard = _cardRepository.FindByNumber(newCard.Number);
            CardDTO newCardDTO = new CardDTO
            {
                CardHolder = newCard.CardHolder,
                Type = newCard.Type,
                Color = newCard.Color,
                Number = newCard.Number,
                Cvv = newCard.Cvv,
                FromDate = newCard.FromDate,
                ThruDate = newCard.ThruDate,
            };
            return Result < CardDTO >.Success(newCardDTO);
        }
        public List<CardDTO> GetCurrentClientCards()
        {
            string UserAuthenticatedEmail = _authService.UserAuthenticated();
            if (UserAuthenticatedEmail == null) return null;

            Client currentClient = _clientRepository.FindByEmail(UserAuthenticatedEmail);
            if (currentClient == null) throw new Exception("Cliente no encontrado");

            List<CardDTO> userCards = currentClient.Cards.Select(c => new CardDTO
            {
                Id = c.Id,
                CardHolder = c.CardHolder,
                Type = c.Type,
                Color = c.Color,
                Number = c.Number,
                Cvv = c.Cvv,
                FromDate = c.FromDate,
                ThruDate = c.ThruDate,
            }).ToList();

            return userCards;
        }
    }
}
