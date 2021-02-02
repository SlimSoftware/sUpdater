using System.Collections.Generic;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;

namespace sUpdater
{
    /// <summary>
    /// Interaction logic for CreditsPage.xaml
    /// </summary>
    public partial class CreditsPage : Page
    {
        public CreditsPage()
        {
            InitializeComponent();

            List<Credit> creditList = new List<Credit>
            {
                new Credit("Inno Setup", "JRSoftware", "http://www.jrsoftware.org/isinfo.php"),
                new Credit("AutoUpdater.NET", "RBSoft", "https://github.com/ravibpatel/AutoUpdater.NET"),
                new Credit("7-Zip", "Igor Pavlov", "https://www.7-zip.org"),
                new Credit("WPF NotifyIcon", "Philipp Sumi", "http://www.hardcodet.net/wpf-notifyicon"),
                new Credit("AsyncEnumerable", "D·ASYNC", "https://github.com/Dasync/AsyncEnumerable") 
            };

            foreach (Credit credit in creditList)
            {
                TextBlock nameLabel = CreateBoldTextBlock(credit.Name);
                nameLabel.Margin = new Thickness(5, 0, 0, 0);
                Label authorLabel = new Label();
                authorLabel.Content = credit.Author;
                authorLabel.Foreground = Brushes.White;

                TextBlock licenseLink = CreateUnderlinedTextBlock("License");
                licenseLink.Cursor = Cursors.Hand;
                licenseLink.VerticalAlignment = VerticalAlignment.Center;
                licenseLink.Margin = new Thickness(0, -15, 0, 0);
                licenseLink.MouseLeftButtonDown += (sender, e) =>
                {
                    LicensePage licensePage = new LicensePage(credit.Name);
                    NavigationService.Navigate(licensePage);
                };

                TextBlock siteLink = CreateUnderlinedTextBlock("Website");
                siteLink.Cursor = Cursors.Hand;
                siteLink.VerticalAlignment = VerticalAlignment.Center;
                siteLink.Margin = new Thickness(10, -15, 0, 0);
                siteLink.MouseLeftButtonDown += (sender, e) =>
                {
                    Process.Start(credit.URL);
                };

                StackPanel nameAuthorStackPanel = new StackPanel();
                nameAuthorStackPanel.Orientation = Orientation.Vertical;
                nameAuthorStackPanel.Children.Add(nameLabel);
                nameAuthorStackPanel.Children.Add(authorLabel);

                StackPanel licenseSiteStackPanel = new StackPanel();
                licenseSiteStackPanel.Orientation = Orientation.Horizontal;
                licenseSiteStackPanel.Children.Add(licenseLink);
                licenseSiteStackPanel.Children.Add(siteLink);

                RowDefinition rowDefinition = new RowDefinition();
                rowDefinition.Height = new GridLength(50);
                grid.RowDefinitions.Add(rowDefinition);

                if (grid.RowDefinitions.Count == 1)
                {
                    Border firstSeparator = CreateSeparator();
                    firstSeparator.Margin = new Thickness(0, -60, 0, 0);
                    Grid.SetRow(firstSeparator, grid.RowDefinitions.Count - 1);
                    Grid.SetColumn(firstSeparator, 0);
                    Grid.SetColumnSpan(firstSeparator, 3);
                    grid.Children.Add(firstSeparator);
                }

                Grid.SetRow(nameAuthorStackPanel, grid.RowDefinitions.Count - 1);
                Grid.SetColumn(nameAuthorStackPanel, 0);
                grid.Children.Add(nameAuthorStackPanel);

                Grid.SetRow(licenseSiteStackPanel, grid.RowDefinitions.Count - 1);
                Grid.SetColumn(licenseSiteStackPanel, 2);
                grid.Children.Add(licenseSiteStackPanel);

                Border separator = CreateSeparator();
                separator.Margin = new Thickness(0, 40, 0, 0);
                Grid.SetRow(separator, grid.RowDefinitions.Count - 1);
                Grid.SetColumn(separator, 0);
                Grid.SetColumnSpan(separator, 3);
                grid.Children.Add(separator);
            }
        }

        private Border CreateSeparator()
        {
            Border separator = new Border();
            separator.Height = 1;
            separator.Background = Brushes.White;
            return separator;
        }

        private TextBlock CreateBoldTextBlock(string text)
        {
            TextBlock textBlock = new TextBlock();
            Bold boldText = new Bold(new Run(text));
            boldText.Foreground = Brushes.White;
            textBlock.Inlines.Add(boldText);
            return textBlock;
        }

        private TextBlock CreateUnderlinedTextBlock(string text)
        {
            TextBlock textBlock = new TextBlock();
            Underline underlinedText = new Underline(new Run(text));
            underlinedText.Foreground = Brushes.White;
            textBlock.Inlines.Add(underlinedText);
            return textBlock;
        }
    }
}
