using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

using System.Media;
using Engine;

namespace Engine
{
    public partial class Statistics : Form
    {
        int allTimeMin, allTimeSec, allSteps, allDmgMade, allDmgRec, allMnstrsKilled, allDeaths;
        int curTimeMin, curTimeSec, curSteps, curDmgMade, curDmgRec, curMnstrsKilled, curDeaths;

        public Statistics(int timeMin, int timeSec, int steps, int dmgMade, int dmgRec, int mnstrsKilled, int deaths) //int time,
        {
            InitializeComponent();

            curTimeMin = timeMin;
            curTimeSec = timeSec;
            curSteps = steps;
            curDmgMade = dmgMade;
            curDmgRec = dmgRec;
            curMnstrsKilled = mnstrsKilled;
            curDeaths = deaths;
        }
        private void Statistics_Load(object sender, EventArgs e)
        {
            if (File.Exists("StatSave"))
            {
                DataSet dsLoad = new DataSet();
                dsLoad.ReadXml("StatSave");
                dataGridView1.DataSource = dsLoad.Tables[0];
                dsLoad.DataSetName = "tblAllSet";
                DataTable tblAll = dsLoad.Tables[0];
                tblAll.TableName = "tblAll";

                allSteps = Convert.ToInt32(tblAll.Rows[1][1].ToString()) + curSteps;
                allDmgMade = Convert.ToInt32(tblAll.Rows[2][1].ToString()) + curDmgMade;
                allDmgRec = Convert.ToInt32(tblAll.Rows[3][1].ToString()) + curDmgRec;
                allMnstrsKilled = Convert.ToInt32(tblAll.Rows[4][1].ToString()) + curMnstrsKilled;
                allDeaths = Convert.ToInt32(tblAll.Rows[5][1].ToString()) + curDeaths;

                tblAll.Rows[1][1] = allSteps;
                tblAll.Rows[2][1] = allDmgMade;
                tblAll.Rows[3][1] = allDmgRec;
                tblAll.Rows[4][1] = allMnstrsKilled;
                tblAll.Rows[5][1] = allDeaths;

                dsLoad.WriteXml("StatSave");
            }
            else
            {
                DataTable tblAll = new DataTable();
                tblAll.TableName = "tblAll";
                DataSet dsA = new DataSet();
                dsA.DataSetName = "tblAllSet";

                tblAll.Columns.Add("Stats");
                tblAll.Columns.Add("All Time");

                tblAll.Rows.Add("Play Time", curTimeMin + "min " + (curTimeSec - curTimeMin * 60) + "sec");
                tblAll.Rows.Add("Steps", curSteps);
                tblAll.Rows.Add("Damage Made", curDmgMade);
                tblAll.Rows.Add("Damage Received", curDmgRec);
                tblAll.Rows.Add("Monsters Killed", curMnstrsKilled);
                tblAll.Rows.Add("Deaths", curDeaths);

                dataGridView1.DataSource = tblAll;

                dsA.Tables.Add(tblAll);
                dsA.WriteXml("StatSave");
            }



            //DataTable tblCur = new DataTable();
            //tblCur.TableName = "tblCur";
            //DataSet dsC = new DataSet();
            //dsC.DataSetName = "tblCurSet";

            //tblCur.Columns.Add("Current Game");

            //tblCur.Rows.Add(curTimeMin + "min " + (curTimeSec - curTimeMin * 60) + "sec");
            //tblCur.Rows.Add(curSteps);
            //tblCur.Rows.Add(curDmgMade);
            //tblCur.Rows.Add(curDmgRec);
            //tblCur.Rows.Add(curMnstrsKilled);
            //tblCur.Rows.Add(curDeaths);

            //dataGridView2.DataSource = tblCur;



        }
    }
}
