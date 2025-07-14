namespace KCSCommunity.Abstractions.Models.Dtos;

public record PasskeyDto(
    string Id,
    string DeviceName,
    DateTime RegistrationDate,
    uint SignatureCounter
);