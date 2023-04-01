using Notes.Models;
using Notes.Services;

namespace Notes.Views;

public partial class ChallengeView : ContentPage
{
    public ChallengeView()
    {
        InitializeComponent();

        BaseClient client = new BaseClient();
        ChallengeServices challengeService = new ChallengeServices(client);

        challengeService.GetChallenge("challenge_1").ContinueWith((task) =>
        {
            var challenge = task.Result;
            this.BindingContext = challenge;
        });
    }
}