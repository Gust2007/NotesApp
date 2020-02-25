using Microsoft.WindowsAzure.Storage;
using NotesApp.Model;
using NotesApp.View.UserControls;
using NotesApp.ViewModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Speech.Recognition;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;


// TODO: support two ore more Notes on a Notebook


namespace NotesApp.View
{
    /// <summary>
    /// Interaction logic for NotesWindow.xaml
    /// </summary>
    public partial class NotesWindow : Window
    {
        public NotesVM viewModel;

        public NotesWindow()
        {
            InitializeComponent();


            viewModel = new NotesVM();
            this.DataContext = viewModel;
            NotesContainer.DataContext = viewModel;

            viewModel.SelectedNoteChanged += ViewModel_SelectedNoteChanged;

            var fontFamilies = Fonts.SystemFontFamilies.OrderBy(f => f.Source);
            fontFamilyComboBox.ItemsSource = fontFamilies;

            List<double> fontSizes = new List<double>() { 8, 9, 10, 11, 12 ,14, 16, 28, 38, 40};
            fontSizeComboBox.ItemsSource = fontSizes;
        }

        private void ViewModel_SelectedNoteChanged(object sender, EventArgs e)
        {
            contentRichTextBox.Document.Blocks.Clear();

            if (viewModel.SelectedNote == null)
                return;

            if (!string.IsNullOrEmpty(viewModel.SelectedNote.FileLocation))
            {
                using (FileStream fileStream = new FileStream(viewModel.SelectedNote.FileLocation, FileMode.Open))
                {
                    TextRange range = new TextRange(contentRichTextBox.Document.ContentStart, contentRichTextBox.Document.ContentEnd);
                    range.Load(fileStream, DataFormats.Rtf);
                }
            }
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }


        protected override void OnActivated(EventArgs e)
        {
            base.OnActivated(e);

            if (string.IsNullOrEmpty(App.UserId))
            {
                LoginWindow loginWindow = new LoginWindow();
                loginWindow.LoginViewModel.HasLoggedIn += LoginVM_HasLoggedIn;

                loginWindow.ShowDialog();
            }
        }

        private void LoginVM_HasLoggedIn(object sender, EventArgs e)
        {
            viewModel.ReadNotebooks();
            viewModel.ReadNotes();
        }

        private void contentRichTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            int amountOfCharacters = new TextRange(contentRichTextBox.Document.ContentStart, contentRichTextBox.Document.ContentEnd).Text.Length;

            statusTextBlock.Text = $"Document length: {amountOfCharacters} characters";
        }

        private void boldButton_Click(object sender, RoutedEventArgs e)
        {
            bool isButtonEnabled = (sender as ToggleButton).IsChecked ?? false;
            if (isButtonEnabled)
                contentRichTextBox.Selection.ApplyPropertyValue(Inline.FontWeightProperty, FontWeights.Bold);
            else
            {
                contentRichTextBox.Selection.ApplyPropertyValue(Inline.FontWeightProperty, FontWeights.Normal);
            }
        }

        private void italicButton_Click(object sender, RoutedEventArgs e)
        {
            bool isButtonEnabled = (sender as ToggleButton).IsChecked ?? false;
            if (isButtonEnabled)
                contentRichTextBox.Selection.ApplyPropertyValue(Inline.FontStyleProperty, FontStyles.Italic);
            else
            {
                contentRichTextBox.Selection.ApplyPropertyValue(Inline.FontStyleProperty, FontStyles.Normal);
            }
        }

        private void underLineButton_Click(object sender, RoutedEventArgs e)
        {
            bool isButtonEnabled = (sender as ToggleButton).IsChecked ?? false;
            if (isButtonEnabled)
                contentRichTextBox.Selection.ApplyPropertyValue(Inline.TextDecorationsProperty, TextDecorations.Underline);
            else
            {
                contentRichTextBox.Selection.ApplyPropertyValue(Inline.TextDecorationsProperty, TextDecorations.Baseline);
            }

        }

        private void contentRichTextBox_SelectionChanged(object sender, RoutedEventArgs e)
        {
            var selectedState = contentRichTextBox.Selection.GetPropertyValue(Inline.FontWeightProperty);
            boldButton.IsChecked = (selectedState != DependencyProperty.UnsetValue) && (selectedState.Equals(FontWeights.Bold));

            var selectedStyle = contentRichTextBox.Selection.GetPropertyValue(Inline.FontStyleProperty);
            italicButton.IsChecked = (selectedStyle != DependencyProperty.UnsetValue) && (selectedStyle.Equals(FontStyles.Italic));

            var selectedDecoration = contentRichTextBox.Selection.GetPropertyValue(Inline.TextDecorationsProperty);
            underLineButton.IsChecked = (selectedDecoration != DependencyProperty.UnsetValue) && (selectedDecoration.Equals(TextDecorations.Underline));

            fontFamilyComboBox.SelectedItem = contentRichTextBox.Selection.GetPropertyValue(Inline.FontFamilyProperty);
            fontSizeComboBox.Text = contentRichTextBox.Selection.GetPropertyValue(Inline.FontSizeProperty).ToString();
        }

        private void fontFamilyComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (fontFamilyComboBox.SelectedItem != null)
            {
                contentRichTextBox.Selection.ApplyPropertyValue(Inline.FontFamilyProperty, fontFamilyComboBox.SelectedItem);
            }
        }


        private void fontSizeComboBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            contentRichTextBox.Selection.ApplyPropertyValue(Inline.FontSizeProperty, fontSizeComboBox.Text);
        }


        private void saveFileButton_Click(object sender, RoutedEventArgs e)
        {
            string fileName = $"{viewModel.SelectedNote.Id}.rtf";
            string rtfFile = System.IO.Path.Combine(Environment.CurrentDirectory, fileName);
            viewModel.SelectedNote.FileLocation = rtfFile;

            using (FileStream fileStream = new FileStream(rtfFile, FileMode.Create))
            {
                TextRange range = new TextRange(contentRichTextBox.Document.ContentStart, contentRichTextBox.Document.ContentEnd);
                range.Save(fileStream, DataFormats.Rtf);
            }

            UploadFile(rtfFile, fileName);

            viewModel.UpdateSelectedNote();
        }


        private async void UploadFile(string rtfFileLocation, string fileName)
        {
            var account = CloudStorageAccount.Parse("DefaultEndpointsProtocol=https;AccountName=gustlsevernotestorage;AccountKey=VnaSq4sBMFvJdEjS7XfCNsCn8SMpdtBhAaReA9xn4SBbzs9PmIaHbSN5bhyPqvxQlcFK4ppk34hVodvYMssROA==;EndpointSuffix=core.windows.net");
            var client = account.CreateCloudBlobClient();
            var container = client.GetContainerReference("notes");
            var blob = container.GetBlockBlobReference(fileName);

            using (FileStream fileStream = new FileStream(rtfFileLocation, FileMode.Open))
            {
                await blob.UploadFromStreamAsync(fileStream);
            }
        }


        private void NoteControl_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            NoteControl noteControl = (sender as NoteControl);

            if (noteControl != null)
            {
                noteControl.Note.IsEditing = true;

                // MessageBox.Show("Note Title: " + noteControl.Note.Title, "Clicked on Note");
            }
        }
    }
}
