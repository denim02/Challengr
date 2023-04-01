//<full>
namespace Notes.Views;

public partial class AllChallengesView : ContentPage
{
    public AllChallengesView()
    {
        InitializeComponent();
    }

    //<event>
    private void ContentPage_NavigatedTo(object sender, NavigatedToEventArgs e)
    {
        notesCollection.SelectedItem = null;
    }
    //</event>
}
//</full>
