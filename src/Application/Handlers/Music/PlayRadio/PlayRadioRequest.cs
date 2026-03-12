using Application.Interfaces.Common;

namespace Application.Handlers.Music.PlayRadio;

public record PlayRadioRequest(
    string Url
) : IRequest<PlayRadioResponse>;