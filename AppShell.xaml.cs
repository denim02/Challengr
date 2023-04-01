namespace Notes;

public partial class AppShell : Shell
{
    public AppShell()
    {
        InitializeComponent();

        Routing.RegisterRoute(nameof(Views.ChallengeView), typeof(Views.ChallengeView));
    }
}