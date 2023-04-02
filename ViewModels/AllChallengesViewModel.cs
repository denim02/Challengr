using CommunityToolkit.Mvvm.Input;
using Notes.Models;
using Notes.Services;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Notes.Views;

namespace Notes.ViewModels;

public class AllChallengesViewModel : BaseViewModel
{
    private readonly BaseClient _client;
    private readonly ChallengeServices _challengeServices;

    public ObservableCollection<Challenges> AllChallenges { get; set; }
    public ICommand SelectChallengeCommand { get; }

    public AllChallengesViewModel()
    {
        _client = new BaseClient();
        _challengeServices = new ChallengeServices(_client);

        AllChallenges = new ObservableCollection<Challenges>();

        GetChallenges();        

        SelectChallengeCommand = new AsyncRelayCommand<Challenges>(SelectChallengeAsync);
    }

    public async Task SelectChallengeAsync(Challenges challenge)
    {
        if (challenge != null)
        {
            await Shell.Current.GoToAsync($"{nameof(Views.ChallengeView)}?load={new ChallengeViewModel()}");
        }
    }

    private async void GetChallenges()
    {
        IEnumerable<Challenges> challenges = await _challengeServices.GetAllChallenges();

        foreach (Challenges challenge in challenges)
        {
            AllChallenges.Add(challenge);
        }
    }
}