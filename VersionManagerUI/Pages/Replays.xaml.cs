﻿using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows;
using VersionManager.GameVersion;
using VersionManagerUI.Data;
using VersionManagerUI.Services;
using System;
using System.Windows.Threading;
using System.Linq;
using System.Collections.Generic;
using System.Collections.Specialized;
using VersionManagerUI.MessageWindows;

namespace VersionManagerUI.Pages
{
    /// <summary>
    /// Interaction logic for Replay.xaml
    /// </summary>
    public partial class Replays : Page
    {
        private ManagedVersionsService _localVersionsService { get; set; }
        private Replay _selectedReplay { get; set; }
        private LocalGameVersion _selectedVersion { get; set; }
        private ReplayService _replayService { get; set; }
        private DispatcherTimer _buttonTimer;
        public Replays(ManagedVersionsService localVersionsService, ReplayService replayService)
        {
            _localVersionsService = localVersionsService;
            _replayService = replayService;
            InitializeComponent();
            ManagedVersionCollection versions = _localVersionsService.GetManagedVersions();
            versions.CollectionChanged += ContentCollectionChanged;
            UpdateVersions();
            versionPick.Visibility = Visibility.Hidden;
            warnNotAvailable.Visibility = Visibility.Hidden;
            _buttonTimer = new DispatcherTimer();
            _buttonTimer.Interval = new TimeSpan(0, 0, 5);
            _buttonTimer.Tick += OnReplayLaunched;
        }

        private void UpdateVersions()
        {
            cmbVersions.ItemsSource = _localVersionsService.GetManagedVersions().OrderByDescending(v => v.LocalVersion);
        }

        private void ContentCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            UpdateVersions();
        }

        private void btnPlay_Click(object sender, RoutedEventArgs e)
        {
            GameVersion version = _selectedReplay.Version == GameVersion.UNKNOWN ? _selectedVersion : _selectedReplay.Version;
            btnPlay.IsEnabled = false;
            btnPlayText.Text = "Launching ...";
            _buttonTimer.Start();
            _replayService.PlayReplay(_selectedReplay, version);
        }

        private void OnReplayLaunched(object sender, EventArgs e)
        {
            btnPlayText.Text = "Play Replay";
            btnPlay.IsEnabled = true;
            _buttonTimer.Stop();
        }

        private void btnBrowse_Click(object sender, RoutedEventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "Replay files (*.wotreplay)|*.wotreplay";

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    SelectReplay(ReplayParser.Parse(openFileDialog.FileName));
                }
            }
        }

        private void SelectReplay(Replay replay)
        {
            if (replay == null)
            {
                new MessageWindow("Invalid file", "This is not a valid .wotreplay file.", MessageWindowButtons.OK).ShowDialog();
                return;
            }
            _selectedReplay = replay;
            frmReplayDetails.Navigate(new ReplayDetails(replay));
            btnPlay.IsEnabled = false;
            if (_selectedReplay.Version != GameVersion.UNKNOWN)
            {
                versionPick.Visibility = Visibility.Hidden;
                bool isLocalVersionAvailable = _localVersionsService.Contains(replay.Version);
                btnPlay.IsEnabled = isLocalVersionAvailable;
                warnNotAvailable.Visibility = isLocalVersionAvailable ? Visibility.Hidden : Visibility.Visible;
            }
            else
            {
                cmbVersions.SelectedIndex = -1;
                versionPick.Visibility = Visibility.Visible;
                warnNotAvailable.Visibility = Visibility.Hidden;
            }
        }

        private void cmbVersions_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count == 0)
                return;
            _selectedVersion = (e.AddedItems[0] as ManagedGameVersion).LocalVersion;
            btnPlay.IsEnabled = true;
        }
    }
}
