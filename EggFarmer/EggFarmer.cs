using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using ScriptSDK;
using StealthAPI;
using ScriptSDK.Engines;
using ScriptSDK.Items;
using ScriptSDK.Mobiles;
using ScriptSDK.Data;

namespace EggFarmer
{
    public partial class formEggFinder : Form
    {

        #region Properties

        private PlayerMobile Self = PlayerMobile.GetPlayer();
        private DateTime StopWatch, StopWatchBuff;
        private bool Working = false;
        private List<Snakes> Snakes = new List<Snakes>();
        private List<SerpentNest> Nests = new List<SerpentNest>();
        private readonly Object WorkLocker = new Object();
        private uint EggsFound = 0;
        private int[,] Path;
        private int[][,] Areas = new int[5][,]
        {
            new int[,] { {640,749},{634,740},{627,730},{617,720},{617,711},{627,707},{637,703},{647,699},{656,699},
            {664,701},{673,701},{675,707},{678,714},{673,719},{664,712},{658,721},{664,731},{661,740},{650,729},
            {663,742},{667,734},{662,725},{662,716},{668,715},{675,723},{680,717},{676,710},{674,705},{666,703},
            {658,703},{650,709},{642,710},{634,711},{628,718},{636,726},{640,733},{643,740},{644,746} },

            new int[,] { {691,729},{684,734},{675,740},{665,743},{659,753},{655,765},{660,773},{673,772},{683,775},
            {693,777},{701,770},{703,762},{709,753},{714,761},{717,768},{717,760},{712,752},{711,738},{712,732},
            {706,741},{706,750},{704,758},{698,768},{690,770},{682,769},{673,769},{663,767},{659,758},{663,750},
            {671,743},{680,740},{687,735},{693,731} },

            new int[,] { {694,709},{687,703},{688,700},{691,707},{698,705},{701,698},{708,691},{714,687},{715,680},
            {716,673},{723,666},{729,663},{735,661},{741,661},{746,667},{747,673},{740,679},{740,686},{740,693},
            {738,699},{732,701},{727,703},{726,700},{727,693},{729,688},{731,681},{735,680},{735,686},{733,693},
            {727,699},{729,702},{728,708},{725,713},{726,720},{730,726},{737,731},{740,736},{735,739},{728,738},
            {722,734},{715,734},{709,730},{709,723},{705,717},{698,710} },

            new int[,] { {812,646},{704,654},{793,660},{783,662},{785,670},{796,676},{803,685},{799,696},{790,702},
            {800,706},{805,714},{814,705},{809,693},{815,681},{826,671},{837,665},{830,656},{820,646} },

            new int[,] { {749,749},{758,760},{768,768},{777,771},{777,761},{772,753},{776,742},{786,736},{797,727},
            {803,719},{794,724},{784,727},{772,736},{761,738},{749,735},{740,740},{749,750} }
        };
        private static int CurrentSpot = 0;
        private bool EggFinderFormClosing, FluteRoutineComplete, EggFinderComplete, CombatComplete;
        /*private BaseCurePotion GreaterCurePotion
        {
            get { return Scanner.Find<BaseCurePotion>(Self.Backpack.Serial.Value, false).First(); }
        }
        private BaseSmokeBomb SmokeBomb
        {
            get { return Scanner.Find<BaseSmokeBomb>(Self.Backpack.Serial.Value, false).First(); }
        }
        private List<SnakeCharmerFlute> Flutes
        {
            get { return Scanner.Find<SnakeCharmerFlute>(Self.Backpack.Serial.Value, false); }
        }
        */
        private BaseCurePotion GreaterCurePotion;
        private BaseSmokeBomb SmokeBomb;
        private List<SnakeCharmerFlute> Flutes;
        #endregion

        public formEggFinder()
        {
            InitializeComponent();

            if (System.Deployment.Application.ApplicationDeployment.IsNetworkDeployed)
            {
                System.Deployment.Application.ApplicationDeployment ad =
                    System.Deployment.Application.ApplicationDeployment.CurrentDeployment;
                this.Text = String.Format("Egg Farmer - {0}", ad.CurrentVersion);
            }
        }

        #region Form Events

        private void formEggFinder_Load(object sender, EventArgs e)
        {
        }

        private void formEggFinder_FormClosing(object sender, FormClosingEventArgs e)
        {
            Working = false;

            if (!FluteRoutineComplete || !EggFinderComplete || !CombatComplete)
            {
                workerEggFinder.CancelAsync();
                workerFluteRoutine.CancelAsync();
                workerCombat.CancelAsync();
                this.Enabled = false;
                e.Cancel = true;
                EggFinderFormClosing = true;
                return;
            }

            base.OnFormClosing(e);
        }

        #endregion

        #region Buttons and Combo Boxes
        private void btnStart_Click(object sender, EventArgs e)
        {
            if (cboxAreas.SelectedIndex == 0)
                MessageBox.Show("No area has been selected.");
            else
            {
                try
                {
                    if (!Stealth.Client.GetConnectedStatus())
                        MessageBox.Show("The active profile on Stealth is disconnected.");
                    else
                    {
                        Scanner.Initialize();
                        Scanner.Range = 10;
                        Scanner.VerticalRange = 9;

                        Working = true;

                        lblStatus.Text = "Connected to " + Self.Name;

                        Self.Backpack.DoubleClick();
                        Thread.Sleep(750);

                        SmokeBomb = Scanner.Find<BaseSmokeBomb>(Self.Backpack.Serial.Value, false).First();
                        Thread.Sleep(250);
                        lblSmokebombsValue.Text = SmokeBomb.Amount.ToString();

                        GreaterCurePotion = Scanner.Find<BaseCurePotion>(Self.Backpack.Serial.Value, false).First();
                        Thread.Sleep(250);
                        lblGreaterCuresValue.Text = GreaterCurePotion.Amount.ToString();

                        //workerEggFinder.RunWorkerAsync();
                        workerFluteRoutine.RunWorkerAsync();
                        workerCombat.RunWorkerAsync();

                        //Stealth.Client.Buff_DebuffSystem += onBuff;
                    }
                }
                catch (Exception x)
                {
                    MessageBox.Show(x.Message.ToString() + " " + x.StackTrace.ToString());
                }
            }
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            Working = false;
            workerEggFinder.CancelAsync();
            workerFluteRoutine.CancelAsync();
            workerCombat.CancelAsync();
            EggsFound = 0;
            CurrentSpot = 0;
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnShowAreas_Click(object sender, EventArgs e)
        {
            MapAreas Areas = new MapAreas();
            Areas.Show();
        }

        private void cboxAreas_SelectedIndexChanged(object sender, EventArgs e)
        {

            lblStatus.Text = "Area " + (cboxAreas.SelectedIndex) + " selected";
            Path = Areas[cboxAreas.SelectedIndex - 1];
        }
        #endregion

        #region Workers
        private void workerFluteRoutine_DoWork(object sender, DoWorkEventArgs e)
        {
            while (Working)
            {
                if (workerFluteRoutine.CancellationPending)
                    break;

                Thread.Sleep(1000);

                try
                {

                    #region Search
                    Search:
                    lock (WorkLocker)
                    {
                        Scanner.Range = 5;
                        Scanner.VerticalRange = 5;
                        Stealth.Client.SetFindDistance(5);
                        Stealth.Client.SetFindVertical(0);

                        Snakes = Scanner.Find<Snakes>(0x0, false).OrderBy(x => x.Distance).ToList();
                        Thread.Sleep(250);

                        Nests = Scanner.Find<SerpentNest>(0x0, false).OrderBy(n => n.Distance).ToList();
                        Thread.Sleep(250);

                        Flutes = Scanner.Find<SnakeCharmerFlute>(Self.Backpack.Serial.Value, false);
                        Thread.Sleep(250);
                    }

                    if (Snakes.Count == 0 || Nests.Count == 0)
                    {
                        workerFluteRoutine.ReportProgress(0, "No nests or no snakes, moving to next spot...");
                        lock (WorkLocker)
                        {
                            MoveNextSpot();
                        }
                        goto Search;
                    }

                    if (Flutes.Count < 1)
                    {
                        workerFluteRoutine.ReportProgress(0, "Ran out of flutes.");
                        Working = false;
                        workerEggFinder.CancelAsync();
                        workerFluteRoutine.CancelAsync();
                        workerCombat.CancelAsync();
                    }

                    #endregion

                    for (int _i = 0; _i < Snakes.Count; _i++)
                    {
                        lock (WorkLocker)
                        {
                            if (GreaterCurePotion.Amount == 1)
                            {
                                workerFluteRoutine.ReportProgress(0, "Ran out of cures.");
                                Working = false;
                                workerEggFinder.CancelAsync();
                                workerFluteRoutine.CancelAsync();
                                workerCombat.CancelAsync();
                            }
                            if (SmokeBomb.Amount == 1)
                            {
                                workerFluteRoutine.ReportProgress(0, "Ran out of smokebombs.");
                                Working = false;
                                workerEggFinder.CancelAsync();
                                workerFluteRoutine.CancelAsync();
                                workerCombat.CancelAsync();
                            }

                            List<RareSerpentEgg> Eggs = Scanner.Find<RareSerpentEgg>(0x0, false);
                            Thread.Sleep(250);

                            if (Eggs.Count > 0)
                            {
                                lock (WorkLocker)
                                {
                                    workerFluteRoutine.ReportProgress(0, "Found egg");
                                    for (int i = 0; i < Eggs.Count; i++)
                                    {
                                        workerFluteRoutine.ReportProgress(0, "Moving to egg");
                                        while (Self.Location.X != Eggs[i].Location.X || Self.Location.Y != Eggs[i].Location.Y)
                                        {
                                            Thread.Sleep(750);
                                            Stealth.Client.MoveXY((ushort)Eggs[i].Location.X, (ushort)Eggs[0].Location.Y, false, 0, false);
                                            if (Self.Location.X != Eggs[i].Location.X || Self.Location.Y != Eggs[i].Location.Y)
                                                break;
                                        }
                                        Thread.Sleep(1000);
                                        Stealth.Client.MoveItem(Eggs[i].Serial.Value, 1, Self.Backpack.Serial.Value, 0, 0, 0);
                                        Thread.Sleep(250);

                                        workerFluteRoutine.ReportProgress(0, "Picking up egg");
                                        EggsFound++;
                                        Scanner.Ignore(Eggs[i].Serial);
                                        Thread.Sleep(1000);
                                    }
                                }
                            }

                            if (Nests[0].Distance > 9)
                            {
                                workerFluteRoutine.ReportProgress(0, "Nest too far, moving to next spot...");
                                MoveNextSpot();
                                break;
                            }

                            if (_i == 3)
                                break;

                            workerFluteRoutine.ReportProgress(0, "Trying to persuede snakes...");
                            Stealth.Client.CancelWaitTarget();
                            Stealth.Client.CancelTarget();

                            while (true)
                            {
                                StopWatch = DateTime.Now;
                                Flutes.First().DoubleClick();
                                if (!Stealth.Client.WaitJournalLine(StopWatch, "You must wait a moment for it to recharge.", 750))
                                    break;

                                workerFluteRoutine.ReportProgress(0, "Flute recharging...");
                                Thread.Sleep(1500);
                            }

                            StopWatch = DateTime.Now;
                            Stealth.Client.WaitTargetObject(Snakes[_i].Serial.Value);
                            Thread.Sleep(500);
                            Nests = Scanner.Find<SerpentNest>(0x0, false).OrderBy(n => n.Distance).ToList();
                            Thread.Sleep(250);
                            Stealth.Client.WaitTargetObject(Nests[0].Serial.Value);

                            if (Stealth.Client.WaitJournalLine(StopWatch, "Target cannot be seen.", 2000))
                            {
                                workerFluteRoutine.ReportProgress(0, "Snake couldn't be seen, checking for another...");
                                continue;
                            }
                            else if (Stealth.Client.WaitJournalLine(StopWatch, "That creature is too far away.", 2000))
                            {
                                workerFluteRoutine.ReportProgress(0, "Snake was too far away, checking for another...");
                                continue;
                            }
                            else if (Stealth.Client.WaitJournalLine(StopWatch, "You don't seem to be able to persuade that to move.", 2000))
                            {
                                workerFluteRoutine.ReportProgress(0, "Snake wasn't persuadable, checking for another...");
                                continue;
                            }
                            else if (Stealth.Client.WaitJournalLine(StopWatch, "That is not a snake or a serpent.", 2000))
                            {
                                workerFluteRoutine.ReportProgress(0, "Target wasn't a snake, trying another...");
                                continue;
                            }
                            else if (Stealth.Client.WaitJournalLine(StopWatch, "Someone else is already taming this.", 2000))
                            {
                                workerFluteRoutine.ReportProgress(0, "Snake hasn't finished digging, trying another...");
                                continue;
                            }
                            else if (Stealth.Client.WaitJournalLine(StopWatch, "The animal walks where it was instructed to.", 2000))
                            {
                                workerFluteRoutine.ReportProgress(0, "Successfully persuaded snake to nest.");
                                Thread.Sleep(10000);
                                continue;
                            }

                            if (workerFluteRoutine.CancellationPending)
                                break;

                            Thread.Sleep(10000);

                            workerEggFinder.ReportProgress(0, "Eggs Found " + EggsFound);
                            List<RareSerpentEgg> TotalEggs;
                            int TotalEggsCount;
                            TotalEggs = Scanner.Find<RareSerpentEgg>(Self.Backpack.Serial.Value, false);
                            Thread.Sleep(250);
                            TotalEggsCount = TotalEggs.Count;
                            workerEggFinder.ReportProgress(0, "Eggs Total " + TotalEggsCount);
                        }
                    }
                }
                catch (Exception x)
                {
                    MessageBox.Show(x.Message.ToString() + " " + x.StackTrace.ToString());
                    Stealth.Client.CancelTarget();
                }
            }
        }

        private void workerEggFinder_DoWork(object sender, DoWorkEventArgs e)
        {
            while (Working)
            {
                Thread.Sleep(1000);

                if (workerEggFinder.CancellationPending)
                    break;

                Scanner.Range = 12;
                List<RareSerpentEgg> Eggs = Scanner.Find<RareSerpentEgg>(0x0, false);

                if (Eggs.Count > 0)
                {
                    lock (WorkLocker)
                    {
                        workerEggFinder.ReportProgress(0, "Found egg");
                        for (int i = 0; i < Eggs.Count; i++)
                        {
                            workerEggFinder.ReportProgress(0, "Moving to egg");
                            while (Self.Location.X != Eggs[i].Location.X || Self.Location.Y != Eggs[i].Location.Y)
                            {
                                Thread.Sleep(750);
                                Stealth.Client.MoveXY((ushort)Eggs[i].Location.X, (ushort)Eggs[0].Location.Y, false, 0, false);
                                if (Self.Location.X != Eggs[i].Location.X || Self.Location.Y != Eggs[i].Location.Y)
                                    break;
                            }
                            Thread.Sleep(1000);
                            Stealth.Client.MoveItem(Eggs[i].Serial.Value, 1, Self.Backpack.Serial.Value, 0, 0, 0);
                            workerEggFinder.ReportProgress(0, "Picking up egg");
                            EggsFound++;
                            Scanner.Ignore(Eggs[i].Serial);
                            Thread.Sleep(1000);
                        }
                    }
                }
            }
        }

        private void workerCombat_DoWork(object sender, DoWorkEventArgs e)
        {
            while (true)
            {
                StopWatchBuff = DateTime.Now;

                if (workerCombat.CancellationPending)
                    break;

                if (!Self.Hidden)
                    if (SmokeBomb.Valid)
                        SmokeBomb.DoubleClick();
                
                if (Stealth.Client.WaitJournalLine(StopWatchBuff, "*You feel a bit nauseous*", 2000))
                {
                    if (GreaterCurePotion.Valid)
                        GreaterCurePotion.DoubleClick();

                    Thread.Sleep(2000);
                    continue;
                }

                Thread.Sleep(500);
            }
        }
        #endregion

        #region Worker Events
        private void workerFluteRoutine_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            lblStatus.Text = e.UserState.ToString();
        }

        private void workerFluteRoutine_WorkerComplete(object sender, RunWorkerCompletedEventArgs e)
        {
            MessageBox.Show("Flute Routine Complete");
            FluteRoutineComplete = true;
            if (EggFinderFormClosing) this.Close();
        }

        private void workerEggFinder_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            if (e.UserState.ToString().Contains("Eggs Found"))
                lblEggsFoundValue.Text = e.UserState.ToString().Split(' ')[2];
            else if (e.UserState.ToString().Contains("Eggs Total"))
                lblEggsTotalValue.Text = e.UserState.ToString().Split(' ')[2];
            else
                lblStatus.Text = e.UserState.ToString();
        }

        private void workerEggFinder_WorkerComplete(object sender, RunWorkerCompletedEventArgs e)
        {
            MessageBox.Show("Egg Finder Complete");
            EggFinderComplete = true;
            if (EggFinderFormClosing) this.Close();
        }

        private void workerCombat_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            lblStatus.Text = e.UserState.ToString();
        }

        private void workerCombat_WorkerComplete(object sender, RunWorkerCompletedEventArgs e)
        {
            MessageBox.Show("Combat Worker Complete");
            CombatComplete = true;
            if (EggFinderFormClosing) this.Close();
        }

        private void onBuff(object sender, Buff_DebuffSystemEventArgs e)
        {
            if (e.AttributeId == 1038)
            {
                if (GreaterCurePotion.Valid)
                    GreaterCurePotion.DoubleClick();
            }
        }
        #endregion

        #region Worker Methods

        private void MoveNextSpot()
        {
            while (Self.Location.X != Path[CurrentSpot, 0] || Self.Location.Y != Path[CurrentSpot, 1])
            {
                Thread.Sleep(750);
                Stealth.Client.MoveXY((ushort)Path[CurrentSpot, 0], (ushort)Path[CurrentSpot, 1], false, 0, false);
                if (Self.Location.X != Path[CurrentSpot, 0] || Self.Location.Y != Path[CurrentSpot, 1])
                    break;

                if (workerFluteRoutine.CancellationPending)
                    break;
            }

            ++CurrentSpot;
            if (CurrentSpot == (Path.Length / 2)) // path contains a pair of coordinates, divide by 2
                CurrentSpot = 0;
        }
        #endregion

    }

    #region Objects
    [QuerySearch(new ushort[] { 0x15, 0x34, 0x5A, 0x5B, 0x5C, 0x5D })]
    public class Snakes : Item
    {
        public Snakes(Serial serial)
            : base(serial)
        {

        }
    }

    [QueryType(typeof(BambooFlute))]
    public class SnakeCharmerFlute : Item
    {
        public SnakeCharmerFlute(Serial serial)
            : base(serial)
        {
        }
    }
    [QuerySearch(new ushort[] { 0x2233 })]
    public class SerpentNest : Item
    {
        public SerpentNest(Serial serial)
            : base(serial)
        {
        }
    }
    [QuerySearch(new ushort[] { 0x41BF })]
    public class RareSerpentEgg : Item
    {
        public RareSerpentEgg(Serial serial)
            : base(serial)
        {
        }
    }
    [QuerySearch(new ushort[] { 0x2809 })]
    public class SmokeBomb : BaseSmokeBomb
    {
        public SmokeBomb(Serial serial)
            : base(serial)
        {
        }
    }
    [QuerySearch(new ushort[] { 0x2808 })]
    public class EggBomb : BaseSmokeBomb
    {
        public EggBomb(Serial serial)
            : base(serial)
        {
        }
    }
    [QueryType(typeof(EggBomb), typeof(SmokeBomb))]
    public class BaseSmokeBomb : Item
    {
        public BaseSmokeBomb(Serial serial)
            : base(serial)
        {
        }
    }
    [QuerySearch(new ushort[] { 0x2805, 0x504, 0x503, 0x2807 })]
    public class BambooFlute : Item
    {
        public BambooFlute(Serial serial)
            : base(serial)
        {
        }
    }
    [QueryType(typeof(BaseCurePotion))]
    public class BasePotion : Item
    {
        public BasePotion(Serial serial)
            : base(serial)
        {
        }
    }
    [QueryType(typeof(LesserCurePotion), typeof(CurePotion), typeof(GreaterCurePotion))]
    public class BaseCurePotion : BasePotion
    {
        public BaseCurePotion(Serial serial)
            : base(serial)
        {
        }
    }

    [QuerySearch(new ushort[] { 0x0F07 })]
    public class LesserCurePotion : BaseCurePotion
    {
        public LesserCurePotion(Serial serial)
            : base(serial)
        {
        }
    }

    [QuerySearch(new ushort[] { 0x0F07 })]
    public class CurePotion : BaseCurePotion
    {
        public CurePotion(Serial serial)
            : base(serial)
        {
        }
    }

    [QuerySearch(new ushort[] { 0x0F07 })]
    public class GreaterCurePotion : BaseCurePotion
    {
        public GreaterCurePotion(Serial serial)
            : base(serial)
        {
        }
    }
    #endregion 

}
