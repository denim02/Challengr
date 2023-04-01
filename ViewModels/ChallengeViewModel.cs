using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace Notes.ViewModels;

internal class ChallengeViewModel : IQueryAttributable
{
    public ObservableCollection<HomeViewModel> AllNotes { get; private set; }
    public ICommand NewCommand { get; }
    public ICommand SelectNoteCommand { get; }

    public ChallengeViewModel()
    {
        //AllNotes = new ObservableCollection<HomeViewModel>(Models.Challenges.LoadAll().Select(n => new NoteViewModel(n)));
        //NewCommand = new AsyncRelayCommand(NewNoteAsync);
        //SelectNoteCommand = new AsyncRelayCommand<HomeViewModel>(SelectNoteAsync);
    }

    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        throw new NotImplementedException();
    }

    //private async Task NewNoteAsync()
    //{
    //    await Shell.Current.GoToAsync(nameof(Views.ChallengeView));
    //}

    //private async Task SelectNoteAsync(HomeViewModel note)
    //{
    //    if (note != null)
    //        await Shell.Current.GoToAsync($"{nameof(Views.ChallengeView)}?load={note.Identifier}");
    //}

    //void IQueryAttributable.ApplyQueryAttributes(IDictionary<string, object> query)
    //{
    //    if (query.ContainsKey("deleted"))
    //    {
    //        string noteId = query["deleted"].ToString();
    //        HomeViewModel matchedNote = AllNotes.Where((n) => n.Identifier == noteId).FirstOrDefault();

    //        // If note exists, delete it
    //        if (matchedNote != null)
    //            AllNotes.Remove(matchedNote);
    //    }
    //    else if (query.ContainsKey("saved"))
    //    {
    //        string noteId = query["saved"].ToString();
    //        HomeViewModel matchedNote = AllNotes.Where((n) => n.Identifier == noteId).FirstOrDefault();

    //        // If note is found, update it
    //        if (matchedNote != null)
    //        {
    //            matchedNote.Reload();
    //            AllNotes.Move(AllNotes.IndexOf(matchedNote), 0);
    //        }
    //        // If note isn't found, it's new; add it.
    //        //else
    //            //AllNotes.Insert(0, new ChallengesViewModel(Models.Challenges.Load(noteId)));
    //    }
    //}
}