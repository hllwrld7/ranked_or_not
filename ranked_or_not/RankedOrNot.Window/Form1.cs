using Microsoft.Extensions.Logging;
using RankedOrNot.Core;
using RankedOrNot.Core.Interfaces;

namespace RankedOrNot.Window
{
    public partial class Form1 : Form
    {
        private readonly IMatchInfoHelper _matchInfoHelper;
        private readonly ILogger<Form1> _logger;
        private readonly CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();

        public Form1(IMatchInfoHelper matchInfoHelper, ILogger<Form1> logger)
        {
            InitializeComponent();
            _matchInfoHelper = matchInfoHelper;
            _logger = logger;
            label1.ForeColor = Color.Black;
            Task.Run(() => CheckForMatches(_cancellationTokenSource.Token), _cancellationTokenSource.Token);
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            _cancellationTokenSource.Cancel();
        }

        private void ReportError()
        {
            SetLabelText("An error has occured...");
            SetLabelColour(Color.Red);
        }

        private void UpdateMatchInfo(MatchInfo matchInfo)
        {
            if (matchInfo == null)
                return;


            if (!matchInfo.IsOngoing)
            {
                SetLabelText("You're not in game right now :(");
                SetLabelColour(Color.Black);
            }
                
            else if(matchInfo.IsRanked)
            {
                SetLabelText("Current match is ranked !");
                SetLabelColour(Color.Red);
            }
            else
            {
                SetLabelText("Current match is not ranked !");
                SetLabelColour(Color.Green);
            }
        }

        private async Task CheckForMatches(CancellationToken stoppingToken)
        {
            while (true)
            {
                await CheckIfInGame();
                await Task.Delay(60000, stoppingToken);
            }
        }

        delegate void SetTextCallback(string text);

        private void SetLabelText(string text) 
        {
            if (label1.InvokeRequired)
            {
                var d = new SetTextCallback(SetLabelText);
                Invoke(d, new object[] { text });
            }
            else
            {
                label1.Text = text;
            }
        }

        delegate void SetColourCallback(Color color);

        private void SetLabelColour(Color color) 
        {
            if (label1.InvokeRequired)
            {
                var d = new SetColourCallback(SetLabelColour);
                Invoke(d, new object[] { color });
            }
            else
            {
                label1.ForeColor = color;
            }
        }

        private async Task CheckIfInGame()
        {
            try
            {
                var matchInfo = await _matchInfoHelper.GetMatchInfo();
                if (matchInfo != null)
                    UpdateMatchInfo(matchInfo);
            }
            catch (Exception ex)
            {
                ReportError();
            }
        }
    }
}
