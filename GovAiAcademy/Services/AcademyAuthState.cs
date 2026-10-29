using GovAiAcademy.Models;

namespace GovAiAcademy.Services;

/// <summary>
/// Circuit-scoped sign-in. Development mode uses sample personas. Entra mode does not accept them.
/// </summary>
public sealed class AcademyAuthState
{
    private readonly AcademyOptions _options;
    private readonly AcademyStore _store;

    public AcademyAuthState(AcademyOptions options, AcademyStore store)
    {
        _options = options;
        _store = store;
    }

    public Person? Current { get; private set; }

    public AcademyOptions Options => _options;

    public bool IsSignedIn => Current is not null;

    public bool CanInstruct =>
        Current?.Role is AcademyRole.Instructor or AcademyRole.Administrator;

    public bool TrySignInPersona(string personId, out string message)
    {
        if (!_options.PersonaSignInAllowed)
        {
            message = "This environment is set to Entra ID. Sample personas are turned off.";
            return false;
        }

        var person = _store.FindPerson(personId);
        if (person is null || !person.DevPersona)
        {
            message = "That sample account is not available.";
            return false;
        }

        Current = person;
        message = $"Signed in as {person.DisplayName}.";
        return true;
    }

    public void SignOut() => Current = null;
}
